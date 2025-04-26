using System.Collections.Generic;
using UnityEngine;

public class ClientGenerator : MonoBehaviour
{
    [SerializeField]
    private ClientType[] clientTypes;

    private float spawnInterval = 3.0f;

    [SerializeField]
    private Transform spawnPoint;

    private float nextSpawnTime;

    Quaternion rot = Quaternion.Euler(0, 90, 0);

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
        GameObject client = Instantiate(selectedType.clientPrefab, spawnPoint.position, rot); //Instantiate(selectedType.clientPrefab, spawnPoint.position, spawnPoint.rotation);  //Instantiate(selectedType.clientPrefab, spawnPoint.position, Quaternion.identity);

        // Configurar el componente de movimiento
        MovimientoClientesMultiple movement = client.GetComponent<MovimientoClientesMultiple>();
        if (movement != null)
        {
            movement.SetPathIndices(availablePaths);
            movement.SetClientWidth(selectedType.width);
            movement.SetSpeed(selectedType.speed);

            // Aquí pasamos la referencia al tipo de cliente
            movement.SetClientType(selectedType);
        }

        
        //Asignar referencias al script InteractuableConInventario del cliente
        InteractuableConInventario interact = client.GetComponent<InteractuableConInventario>();
        if (interact != null)
        {
            // Buscar el inventario y el jugador dentro de la escena
            interact.inventario = FindFirstObjectByType<InventoryManager2>(); // o InventoryManager2, según tu script real
            interact.player = GameObject.FindWithTag("Player")?.transform;
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

}
