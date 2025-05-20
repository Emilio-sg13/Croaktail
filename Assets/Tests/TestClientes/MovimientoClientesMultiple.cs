using UnityEngine;

/// <summary>
/// Controla el movimiento de clientes a lo largo de varios puntos,
/// así como la animación y visualización de pedidos de cócteles.
/// </summary>
public class MovimientoClientesMultiple : MonoBehaviour
{
    // Índices de los caminos por los que se desplaza el cliente
    private int[] pathIndices;
    // Ancho en número de rutas que ocupa el cliente
    private int clientWidth = 1;
    // Punto actual en el camino
    private int currentPoint = 0;
    // Velocidad de movimiento
    private float speed = 2.0f;
    // Distancia para considerar que se ha llegado a un punto
    private float reachDistance = 0.1f;
    // Indica si el cliente ha llegado al final del trayecto
    private bool hasReachedEnd = false;
    // Indica si el cliente ya se ha sentado
    private bool hasSeated = false;
    // Transform del punto objetivo actual
    private Transform targetPoint;
    // Datos del cliente, incluidos los cócteles que puede pedir
    private ClientType clientType;

    public GameObject standingModel;
    public GameObject sittingModel;

    [SerializeField]
    private SpriteRenderer coctelRenderer; // Renderer para mostrar el sprite del cóctel pedido

    public item requestedCoctel;  // Guarda el cóctel que se ha pedido

    // Referencia al Animator para controlar las animaciones
    private Animator animator;

    /// <summary>
    /// Devuelve el ancho del cliente en número de rutas.
    /// </summary>
    public int GetCurrentWidth() => clientWidth;

    /// <summary>
    /// Configura los índices de los caminos disponibles.
    /// </summary>
    public void SetPathIndices(int[] indices) => pathIndices = indices;

    /// <summary>
    /// Ajusta el ancho del cliente (número de rutas que ocupa).
    /// </summary>
    public void SetClientWidth(int width) => clientWidth = width;

    /// <summary>
    /// Establece la velocidad de movimiento del cliente.
    /// </summary>
    public void SetSpeed(float newSpeed) => speed = newSpeed;

    /// <summary>
    /// Asigna el tipo de cliente para determinar sus cócteles posibles.
    /// </summary>
    public void SetClientType(ClientType type) => clientType = type;

    void Start()
    {
        if (sittingModel != null) sittingModel.SetActive(false);
        if (standingModel != null) standingModel.SetActive(true);

        // Validar que exista un PathManager en la escena
        if (PathManager.Instance == null)
        {
            Debug.LogError("No hay PathManager en la escena");
            enabled = false;
            return;
        }

        // Validar que se hayan asignado suficientes caminos
        if (pathIndices == null || pathIndices.Length < clientWidth)
        {
            Debug.LogError("Caminos no válidos para el cliente");
            enabled = false;
            return;
        }

        // Obtener el Animator del hijo que contiene el modelo 3D
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator no encontrado en el cliente o sus hijos.");
        else
            // Empezar en estado Idle (Walking = false)
            animator.SetBool("Caminar", false);

        // Ocultar al inicio el sprite del cóctel
        if (coctelRenderer != null)
            coctelRenderer.enabled = false;

        // Iniciar la ocupación del primer punto
        UpdateTargetPoint();
        PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);
    }

    void Update()
    {
        // Si ya terminó o no hay punto objetivo, no hacer nada
        if (hasReachedEnd || targetPoint == null)
            return;

        // Calcular distancia al punto objetivo
        float dist = Vector3.Distance(transform.position, targetPoint.position);

        // Animación de caminar: activo mientras no haya llegado al final
        animator?.SetBool("Caminar", !hasReachedEnd);

        // Si llegamos lo suficientemente cerca
        if (dist < reachDistance)
        {
            int pathLength = PathManager.Instance.GetPathLength(pathIndices[0]);

            /*// Sentarse si estamos en el penúltimo punto
            if (!hasSeated && currentPoint == pathLength - 2)
            {
                hasSeated = true;
                animator?.SetTrigger("Sentarse"); // Dispara animación Sentarse
                // Tras la duración de Sentarse, cambiar a IdleSentado
                Invoke(nameof(PlayIdleSentado), 1.5f); // Ajustar tiempo al clip real
            }*/

            // Si llegamos al final del camino
            if (currentPoint + 1 >= pathLength)
            {
                hasReachedEnd = true;
                animator?.SetBool("Caminar", false);
                ShowRandomCoctel(); // Mostrar sprite de cóctel
                return;
            }

            // Avanzar al siguiente punto si está libre
            if (PathManager.Instance.CanOccupyMultipleHorizontal(pathIndices, currentPoint + 1, clientWidth, gameObject))
            {
                // Sentarse si estamos en el penúltimo punto
                if (!hasSeated && currentPoint == pathLength - 2)
                {
                    hasSeated = true;
                    animator?.SetTrigger("Sentarse"); // Dispara animación Sentarse
                                                      // Tras la duración de Sentarse, cambiar a IdleSentado
                    Invoke(nameof(PlayIdleSentado), 1.5f); // Ajustar tiempo al clip real
                }
                PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
                currentPoint++;
                PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);
                UpdateTargetPoint();
            }
            else
            {
                animator?.SetBool("Caminar", false);
            }
            return;
        }

        // Moverse hacia el punto objetivo
        Vector3 dir = (targetPoint.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Actualiza el punto objetivo según el índice central del ancho.
    /// </summary>
    private void UpdateTargetPoint()
    {
        int central = pathIndices[clientWidth / 2];
        targetPoint = PathManager.Instance.GetPathPoint(central, currentPoint);
    }

    /// <summary>
    /// Elige al azar un cóctel pedido y lo muestra temporalmente.
    /// </summary>
    public void ShowRandomCoctel()
    {
        if (clientType?.Cocteles == null || clientType.Cocteles.Count == 0)
            return;

        // Selección aleatoria de cóctel
        item coctel = clientType.Cocteles[Random.Range(0, clientType.Cocteles.Count)];
        requestedCoctel = coctel;

        if (coctelRenderer != null)
        {
            coctelRenderer.sprite = coctel.sprite;
            coctelRenderer.enabled = true;
            Invoke(nameof(HideCoctelRenderer), 5f); // Ocultar tras 5s
        }
    }

    /// <summary>
    /// Oculta el SpriteRenderer del cóctel.
    /// </summary>
    private void HideCoctelRenderer()
    {
        if (coctelRenderer != null)
            coctelRenderer.enabled = false;
    }

    /// <summary>
    /// Dispara el trigger IdleSentado en el Animator.
    /// </summary>
    private void PlayIdleSentado()
    {
        // Disparamos la animación idle sentado
        animator?.SetTrigger("IdleSentado");

        // Ahora intercambiamos las mallas
        if (standingModel != null) standingModel.SetActive(false);
        if (sittingModel != null) sittingModel.SetActive(true);
    }


    /// <summary>
    /// Dispara la animación de negación desde InteractuableConInventario.
    /// </summary>
    public void PlayNegacion()
    {
        Debug.Log("aaaaaaaa");
        animator?.SetTrigger("Negacion");
    }

    // Mostrar cóctel mientras el cursor esté encima
    void OnMouseEnter()
    {
        if (coctelRenderer != null)
        {
            CancelInvoke(nameof(HideCoctelRenderer));
            coctelRenderer.enabled = true;
        }
    }

    // Ocultar cóctel cuando el cursor sale
    void OnMouseExit()
    {
        if (coctelRenderer != null)
            coctelRenderer.enabled = false;
    }

    // Liberar puntos al destruir el objeto
    void OnDestroy()
    {
        if (PathManager.Instance != null && !hasReachedEnd)
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
    }
}
