using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoClientesMultiple : MonoBehaviour
{
    private int[] pathIndices;
    private int clientWidth = 1;
    private int currentPoint = 0;
    private float speed = 2.0f;
    private float reachDistance = 0.1f;
    public bool hasReachedEnd = false;
    private Transform targetPoint;
    private ClientType clientType; // Referencia al tipo de cliente para obtener la lista de c�cteles

    [SerializeField]
    public SpriteRenderer coctelRenderer; // Asigna este componente desde el Inspector

    public item requestedCoctel;  // Nuevo campo para guardar el c�ctel pedido

    // Animator para controlar las animaciones
    private Animator animator;

    // Componentes de audio
    private AudioSource audioSource;
    private bool isMoving = false;
    private bool wasMovingLastFrame = false;
    
    // Variables de control de pausa
    private bool wasPlayingBeforePause = false;
    private bool isPaused = false;

    public int GetCurrentWidth()
    {
        return clientWidth;
    }

    // M�todos para configurar el cliente externamente
    public void SetPathIndices(int[] indices)
    {
        pathIndices = indices;
    }

    public void SetClientWidth(int width)
    {
        clientWidth = width;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetClientType(ClientType type)
    {
        clientType = type;
    }

    void Start()
    {
        // Configurar el componente AudioSource
        SetupAudioSource();

        // Asegurarse de que el PathManager existe
        if (PathManager.Instance == null)
        {
            Debug.LogError("No hay PathManager en la escena");
            enabled = false;
            return;
        }

        // Verificar que tenemos caminos asignados
        if (pathIndices == null || pathIndices.Length == 0)
        {
            Debug.LogError("No hay caminos asignados para este cliente");
            enabled = false;
            return;
        }

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Caminar", false);

        // Verificar que tenemos suficientes caminos definidos para nuestro ancho
        if (pathIndices.Length < clientWidth)
        {
            Debug.LogError("No hay suficientes caminos definidos para el ancho del cliente");
            enabled = false;
            return;
        }

        // Ocultar el sprite del c�ctel al inicio
        if (coctelRenderer != null)
        {
            coctelRenderer.enabled = false;
        }

        // Inicializar el punto objetivo y ocupar el primer punto
        UpdateTargetPoint();
        PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);
    }

    void Update()
    {
        // Verificar si el juego est� en pausa
        CheckPauseState();
        
        if (hasReachedEnd || targetPoint == null)
        {
            HandleMovementAudio(false);
            return;
        }

        // Verificar si se ha llegado al punto actual
        if (Vector3.Distance(transform.position, targetPoint.position) < reachDistance)
        {
            // Verificar si se lleg� al final del camino
            if (currentPoint + 1 >= PathManager.Instance.GetPathLength(pathIndices[0]))
            {
                hasReachedEnd = true;
                HandleMovementAudio(false);
                animator?.SetTrigger("Sentarse");
                Invoke(nameof(PlayIdleSentado), 15f); // Ajusta este tiempo a la duraci�n real de Sentarse

                ShowRandomCoctel(); // Mostrar el c�ctel al llegar al final
                return;
            }

            // Verificar si se puede avanzar al siguiente punto
            if (!PathManager.Instance.CanOccupyMultipleHorizontal(pathIndices, currentPoint + 1, clientWidth, gameObject))
            {
                animator.SetBool("Caminar", false);
                HandleMovementAudio(false);
                return;
            }

            // Liberar el punto actual, avanzar y ocupar el nuevo punto
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
            currentPoint++;
            PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);
            UpdateTargetPoint();
        }

        // Moverse hacia el punto objetivo
        Vector3 dir = targetPoint.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        
        // Disparar animaci�n Caminar mientras se mueve
        animator?.SetBool("Caminar", true);
        
        // Manejar el audio de movimiento
        HandleMovementAudio(true);
    }

    private void CheckPauseState()
    {
        bool currentlyPaused = Time.timeScale == 0f;
        
        if (currentlyPaused != isPaused)
        {
            isPaused = currentlyPaused;
            
            if (isPaused)
            {
                // El juego acaba de pausarse
                OnGamePaused();
            }
            else
            {
                // El juego acaba de reanudarse
                OnGameUnpaused();
            }
        }
    }

    private void OnGamePaused()
    {
        if (audioSource != null)
        {
            wasPlayingBeforePause = audioSource.isPlaying;
            if (wasPlayingBeforePause)
            {
                audioSource.Pause();
            }
        }
    }

    private void OnGameUnpaused()
    {
        if (audioSource != null && wasPlayingBeforePause)
        {
            audioSource.UnPause();
        }
    }

    private void SetupAudioSource()
    {
        // Obtener o a�adir el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el AudioSource para sonidos de movimiento
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0.7f; // Audio espacial 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 10f;
    }

    private void HandleMovementAudio(bool shouldBeMoving)
    {
        // No reproducir sonidos si el juego est� en pausa
        if (isPaused)
            return;
            
        isMoving = shouldBeMoving;

        // Verificar si el estado de movimiento cambi�
        if (isMoving != wasMovingLastFrame)
        {
            if (isMoving)
            {
                PlayMovementSound();
            }
            else
            {
                StopMovementSound();
            }
        }

        wasMovingLastFrame = isMoving;
    }

    private void PlayMovementSound()
    {
        if (clientType != null && clientType.movementSound != null && audioSource != null && !isPaused)
        {
            audioSource.clip = clientType.movementSound;
            audioSource.Play();
        }
    }

    private void StopMovementSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void UpdateTargetPoint()
    {
        int centralPathIndex = pathIndices[clientWidth / 2];
        targetPoint = PathManager.Instance.GetPathPoint(centralPathIndex, currentPoint);
    }

    // M�todo que se llama al llegar al final del camino para mostrar un c�ctel aleatorio
    public void ShowRandomCoctel()
    {
        if (clientType != null && clientType.Cocteles != null && clientType.Cocteles.Count > 0)
        {
            int randomIndex = Random.Range(0, clientType.Cocteles.Count);
            item randomCoctel = clientType.Cocteles[randomIndex];
            requestedCoctel = randomCoctel;
            if (coctelRenderer != null)
            {
                coctelRenderer.sprite = randomCoctel.sprite;
                coctelRenderer.enabled = true; // Activar el SpriteRenderer para mostrar el sprite
                Invoke("HideCoctelRenderer", 5f); // Invoca el m�todo para desactivar despu�s de 5 segundos
            }
        }
    }


    // Tras Sentarse, cambia el modelo y dispara IdleSentado.
    
    private void PlayIdleSentado()
    {
        animator?.SetTrigger("Sentado");
    }

    
    // Dispara la animaci�n de negaci�n.
    
    public void PlayNegacion()
    {
        animator?.SetTrigger("Negacion");
    }

    // Agregamos OnMouseEnter y OnMouseExit para que el render se muestre mientras el cursor est� encima.
    void OnMouseEnter()
    {
        if (coctelRenderer != null)
        {
            // Cancelamos el Invoke si est� programado para ocultar el sprite.
            CancelInvoke("HideCoctelRenderer");
            coctelRenderer.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (coctelRenderer != null)
        {
            coctelRenderer.enabled = false;
        }
    }

    // M�todo que desactiva el SpriteRenderer
    private void HideCoctelRenderer()
    {
        coctelRenderer.enabled = false;
    }

    void OnDestroy()
    {
        // Detener cualquier sonido que est� reproduci�ndose
        StopMovementSound();
        
        if (PathManager.Instance != null && !hasReachedEnd)
        {
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
        }
    }


    //Funcion solo para el Tutorial
    /// <summary>
    /// Muestra un c�ctel espec�fico, sin elegirlo aleatoriamente.
    /// </summary>
    public void ShowSpecificCoctel(item specificCoctel)
    {
        requestedCoctel = specificCoctel;
        if (coctelRenderer != null)
        {
            coctelRenderer.sprite = specificCoctel.sprite;
            coctelRenderer.enabled = true;
            // Si quieres ocultarlo despu�s de X segundos:
            // Invoke(nameof(HideCoctelRenderer), 5f);
        }
    }

    public void StartFadeOut(float duration = 1.5f)
    {
        StartCoroutine(FadeOutAndDestroy(duration));
    }

    private IEnumerator FadeOutAndDestroy(float duration)
    {
        // Buscar todos los Renderers del cliente, incluyendo Skinned y Mesh
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>(true);

        List<Material> fadeMaterials = new List<Material>();

        foreach (Renderer rend in allRenderers)
        {
            // Ignorar el renderer del sprite del cóctel si está definido
            if (coctelRenderer != null && rend == coctelRenderer)
                continue;

            // Crear un nuevo material independiente (no afectar otros clientes)
            Material mat = new Material(rend.material);
            SetMaterialToFadeMode(mat);
            rend.material = mat;

            fadeMaterials.Add(mat);
        }

        // Desactivar colisiones
        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            foreach (Material mat in fadeMaterials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color c = mat.color;
                    c.a = alpha;
                    mat.color = c;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetMaterialToFadeMode(Material mat)
    {
        if (mat.shader.name != "Standard") return; // Solo si usa el Standard Shader

        mat.SetFloat("_Mode", 2); // Fade
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }




}
