using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MovimientoClientesMultiple : MonoBehaviour
{
    private int[] pathIndices;
    private int clientWidth = 1;
    private int currentPoint = 0;
    private float speed = 2.0f;
    private float reachDistance = 0.1f;
    private bool hasReachedEnd = false;
    private Transform targetPoint;
    private ClientType clientType; // Referencia al tipo de cliente para obtener la lista de cócteles

    [SerializeField]
    private SpriteRenderer coctelRenderer; // Asigna este componente desde el Inspector

    public int GetCurrentWidth()
    {
        return clientWidth;
    }

    // Métodos para configurar el cliente externamente
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

        // Verificar que tenemos suficientes caminos definidos para nuestro ancho
        if (pathIndices.Length < clientWidth)
        {
            Debug.LogError("No hay suficientes caminos definidos para el ancho del cliente");
            enabled = false;
            return;
        }

        // Ocultar el sprite del cóctel al inicio
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
        if (hasReachedEnd || targetPoint == null)
            return;

        // Verificar si se ha llegado al punto actual
        if (Vector3.Distance(transform.position, targetPoint.position) < reachDistance)
        {
            // Verificar si se llegó al final del camino
            if (currentPoint + 1 >= PathManager.Instance.GetPathLength(pathIndices[0]))
            {
                hasReachedEnd = true;
                ShowRandomCoctel(); // Mostrar el cóctel al llegar al final
                return;
            }

            // Verificar si se puede avanzar al siguiente punto
            if (!PathManager.Instance.CanOccupyMultipleHorizontal(pathIndices, currentPoint + 1, clientWidth, gameObject))
            {
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
    }

    private void UpdateTargetPoint()
    {
        int centralPathIndex = pathIndices[clientWidth / 2];
        targetPoint = PathManager.Instance.GetPathPoint(centralPathIndex, currentPoint);
    }

    // Método que se llama al llegar al final del camino para mostrar un cóctel aleatorio
    private void ShowRandomCoctel()
    {
        if (clientType != null && clientType.Cocteles != null && clientType.Cocteles.Count > 0)
        {
            int randomIndex = Random.Range(0, clientType.Cocteles.Count);
            item randomCoctel = clientType.Cocteles[randomIndex];
            if (coctelRenderer != null)
            {
                coctelRenderer.sprite = randomCoctel.sprite;
                coctelRenderer.enabled = true; // Activar el SpriteRenderer para mostrar el sprite
            }
        }
    }

    void OnDestroy()
    {
        if (PathManager.Instance != null && !hasReachedEnd)
        {
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
        }
    }
}
