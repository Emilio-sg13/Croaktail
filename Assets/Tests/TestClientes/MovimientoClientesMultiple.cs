using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MovimientoClientesMultiple : MonoBehaviour
{
    private int[] pathIndices; // Los índices de los caminos que este cliente puede usar
    private int clientWidth = 1; // Cuántos caminos ocupa horizontalmente
    private int currentPoint = 0;
    private float speed = 2.0f;
    private float reachDistance = 0.1f;
    private bool hasReachedEnd = false;
    private Transform targetPoint;

    // Métodos para configurar el cliente externamente (desde el generador)
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

    void Start()
    {
        // Verificar que el PathManager existe
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

        // Inicializar el punto objetivo
        UpdateTargetPoint();

        // Ocupar el punto inicial
        PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);
    }

    void Update()
    {
        if (hasReachedEnd || targetPoint == null)
            return;

        // Comprobar si hemos llegado al punto actual
        if (Vector3.Distance(transform.position, targetPoint.position) < reachDistance)
        {
            // Verificar si hemos llegado al final del camino
            if (currentPoint + 1 >= PathManager.Instance.GetPathLength(pathIndices[0]))
            {
                hasReachedEnd = true;
                return;
            }

            // Verificar si podemos avanzar al siguiente punto en todos los caminos necesarios
            if (!PathManager.Instance.CanOccupyMultipleHorizontal(pathIndices, currentPoint + 1, clientWidth, gameObject))
            {
                // No podemos avanzar, nos quedamos aquí
                return;
            }

            // Liberar el punto actual
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);

            // Avanzar al siguiente punto
            currentPoint++;

            // Ocupar el nuevo punto
            PathManager.Instance.OccupyMultipleHorizontal(pathIndices, currentPoint, clientWidth, gameObject);

            // Actualizar el punto objetivo
            UpdateTargetPoint();
        }

        // Moverse hacia el punto objetivo
        Vector3 dir = targetPoint.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }

    private void UpdateTargetPoint()
    {
        // Usamos el punto central como objetivo de movimiento (para clientes anchos)
        int centralPathIndex = pathIndices[clientWidth / 2];
        targetPoint = PathManager.Instance.GetPathPoint(centralPathIndex, currentPoint);
    }

    void OnDestroy()
    {
        // Liberar todos los puntos ocupados
        if (PathManager.Instance != null && !hasReachedEnd)
        {
            PathManager.Instance.ReleaseMultipleHorizontal(pathIndices, currentPoint, clientWidth);
        }
    }
}