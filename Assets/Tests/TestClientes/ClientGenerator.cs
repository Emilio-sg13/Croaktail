using System.Collections.Generic;
using UnityEngine;

public class ClientGenerator : MonoBehaviour
{
    [SerializeField]
    private ClientType[] clientTypes;

    [SerializeField]
    private float spawnInterval = 3.0f;

    [SerializeField]
    private Transform spawnPoint;

    private float nextSpawnTime;

    // Activación del AudioManager para poder utilizar el SFX del cliente cuando entra el bar
    AudioManager audioManager;

    [Header("Audio Settings")]
    [SerializeField]
    [Range(0f, 1f)]
    private float movementSoundVolume = 0.5f;
    
    [SerializeField]
    private bool enableMovementSounds = true;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        // Validar que los tipos de cliente tengan sonidos de movimiento si el audio está habilitado
        if (enableMovementSounds)
        {
            ValidateClientAudioSetup();
        }
    }

    // Quaternion rot = Quaternion.Euler(0, 0, 0);

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnClient();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void TrySpawnClient()
    {
        if (PathManager.Instance == null)
            return;

        // Seleccionar un tipo de cliente aleatoriamente basado en su peso
        ClientType selectedType = GetRandomClientType();
        if (selectedType == null)
            return;

        // Encontrar caminos disponibles para este tipo de cliente
        int[] availablePaths = PathManager.Instance.FindAvailableAdjacentPaths(selectedType.width);
        if (availablePaths == null)
            return; // No hay caminos disponibles

        // Crear el cliente
        GameObject client = Instantiate(selectedType.clientPrefab, spawnPoint.position, spawnPoint.rotation);

        // REPRODUCIR SFX CUANDO APARECE UN NUEVO CLIENTE - Solo si no está en pausa
        if (audioManager != null && Time.timeScale != 0f)
        {
            audioManager.PlaySFX(audioManager.entraCliente);
        }

        // Configurar el componente de movimiento
        MovimientoClientesMultiple movement = client.GetComponent<MovimientoClientesMultiple>();
        if (movement != null)
        {
            movement.SetPathIndices(availablePaths);
            movement.SetClientWidth(selectedType.width);
            movement.SetSpeed(selectedType.speed);

            // Aquí pasamos la referencia al tipo de cliente
            movement.SetClientType(selectedType);
            
            // Configurar ajustes de audio para este cliente
            ConfigureClientAudio(client, selectedType);
        }

        // Asignar referencias al script InteractuableConInventario del cliente
        InteractuableConInventario interact = client.GetComponent<InteractuableConInventario>();
        if (interact != null)
        {
            // Buscar el inventario y el jugador dentro de la escena
            interact.inventario = FindFirstObjectByType<InventoryManager2>(); // o InventoryManager2, según tu script real
            interact.player = GameObject.FindWithTag("Player")?.transform;
        }
    }

    private void ConfigureClientAudio(GameObject client, ClientType clientType)
    {
        if (!enableMovementSounds)
            return;

        // Obtener o agregar AudioSource para sonidos de movimiento
        AudioSource audioSource = client.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = client.AddComponent<AudioSource>();
        }

        // Configurar ajustes de AudioSource
        audioSource.volume = movementSoundVolume;
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        
        // Ajustes de audio espacial 3D
        audioSource.spatialBlend = 0.7f; // Mezcla entre 2D (0) y 3D (1)
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;
        
        // Opcional: añadir variación de pitch en base a la velocidad del cliente
        if (clientType.speed != 0)
        {
            float pitchVariation = Mathf.Clamp(clientType.speed / 2f, 0.8f, 1.2f);
            audioSource.pitch = pitchVariation;
        }

        // Advertencia si no se asignó sonido de movimiento
        if (clientType.movementSound == null)
        {
            Debug.LogWarning($"ClientType '{clientType.typeName}' no tiene un sonido de movimiento asignado.");
        }
    }

    private void ValidateClientAudioSetup()
    {
        if (clientTypes == null || clientTypes.Length == 0)
            return;

        int clientsWithoutSound = 0;
        foreach (var clientType in clientTypes)
        {
            if (clientType.movementSound == null)
            {
                clientsWithoutSound++;
                Debug.LogWarning($"ClientType '{clientType.typeName}' no tiene sonido de movimiento asignado.");
            }
        }

        if (clientsWithoutSound > 0)
        {
            Debug.LogWarning($"{clientsWithoutSound} tipo(s) de cliente no tienen sonidos de movimiento asignados. " +
                           "Considera asignar AudioClips o desactivar los sonidos de movimiento.");
        }
    }

    private bool IsClientOfWidthPresent(int width)
    {
        // Esto buscará todos los objetos que tengan MovimientoClientesMultiple
        // Ten en cuenta que FindObjectsOfType puede ser costoso si se llama muy frecuentemente.
        MovimientoClientesMultiple[] allClients = FindObjectsByType<MovimientoClientesMultiple>(FindObjectsSortMode.None);

        foreach (var client in allClients)
        {
            if (client.GetCurrentWidth() == width) // Método o variable para obtener el width
            {
                return true;
            }
        }
        return false;
    }

    private ClientType GetRandomClientType()
    {
        if (clientTypes.Length == 0)
            return null;

        // 1. Reunimos los tipos de cliente que sí están permitidos
        //    (es decir, que no estén bloqueados por haber ya uno en escena)
        var allowedTypes = new List<ClientType>();
        foreach (var type in clientTypes)
        {
            // Si el cliente es de width=2 o 3, comprobamos si ya existe uno en escena
            if ((type.width == 2 || type.width == 3) && IsClientOfWidthPresent(type.width))
            {
                // Si ya hay un cliente de ese width, NO lo añadimos a la lista
                continue;
            }

            // De lo contrario, lo incluimos en las opciones
            allowedTypes.Add(type);
        }

        // 2. Si después de filtrar no hay ningún tipo permitido, retornamos null
        if (allowedTypes.Count == 0)
        {
            return null;
        }

        // 3. Calculamos el peso total sólo de los tipos permitidos
        float totalWeight = 0f;
        foreach (var type in allowedTypes)
        {
            totalWeight += type.spawnWeight;
        }

        // 4. Seleccionamos un tipo aleatoriamente con la lógica de pesos
        float randomValue = Random.Range(0, totalWeight);
        float weightSum = 0f;
        foreach (var type in allowedTypes)
        {
            weightSum += type.spawnWeight;
            if (randomValue <= weightSum)
            {
                return type;
            }
        }

        // Fallback, por si acaso
        return allowedTypes[0];
    }

    // Función pública que duplica el valor de spawnInterval
    public void DoblarSpawnInterval()
    {
        spawnInterval = 1.0f;
    }

    // Métodos públicos para controlar el audio en tiempo de ejecución
    public void SetMovementSoundVolume(float volume)
    {
        movementSoundVolume = Mathf.Clamp01(volume);
        
        // Actualizar todos los clientes existentes
        MovimientoClientesMultiple[] allClients = FindObjectsByType<MovimientoClientesMultiple>(FindObjectsSortMode.None);
        foreach (var client in allClients)
        {
            AudioSource audioSource = client.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = movementSoundVolume;
            }
        }
    }

    public void ToggleMovementSounds(bool enable)
    {
        enableMovementSounds = enable;
        
        // Si se desactiva, detener todos los sonidos de movimiento actuales
        if (!enable)
        {
            MovimientoClientesMultiple[] allClients = FindObjectsByType<MovimientoClientesMultiple>(FindObjectsSortMode.None);
            foreach (var client in allClients)
            {
                AudioSource audioSource = client.GetComponent<AudioSource>();
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    // Método público para pausar o reanudar todos los sonidos de movimiento de los clientes
    public void PauseAllClientSounds()
    {
        MovimientoClientesMultiple[] allClients = FindObjectsByType<MovimientoClientesMultiple>(FindObjectsSortMode.None);
        foreach (var client in allClients)
        {
            AudioSource audioSource = client.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public void UnpauseAllClientSounds()
    {
        MovimientoClientesMultiple[] allClients = FindObjectsByType<MovimientoClientesMultiple>(FindObjectsSortMode.None);
        foreach (var client in allClients)
        {
            AudioSource audioSource = client.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.UnPause();
            }
        }
    }
}
