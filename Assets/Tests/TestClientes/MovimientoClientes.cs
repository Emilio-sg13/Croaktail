using UnityEngine;

public class MovimientoClientes : MonoBehaviour
{
    int currentPoint = 0;

    [SerializeField]
    float speed = 2.0f;

    [SerializeField]
    float reachDistance = 0.1f;

    void Start()
    {
        // Verificar que el PathManager existe
        if (PathManager.Instance == null)
        {
            Debug.LogError("No hay PathManager en la escena");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Verificar que existe un PathManager y un punto válido
        if (PathManager.Instance == null || currentPoint >= PathManager.Instance.GetPathLength())
            return;

        Transform currentPathPoint = PathManager.Instance.GetPathPoint(currentPoint);
        if (currentPathPoint == null)
            return;

        // Si estamos en un punto y el siguiente está ocupado, nos quedamos quietos
        if (Vector3.Distance(transform.position, currentPathPoint.position) < reachDistance)
        {
            // Marcar el punto actual como ocupado por nosotros
            PathManager.Instance.SetPointOccupation(currentPoint, gameObject, true);

            // Comprobar si hay siguiente punto y si está ocupado
            if (currentPoint + 1 < PathManager.Instance.GetPathLength() &&
                PathManager.Instance.IsPointOccupied(currentPoint + 1))
            {
                // No avanzamos, nos quedamos en este punto
                return;
            }

            // Si el siguiente punto no está ocupado o no hay siguiente punto
            // Liberar el punto actual
            // PathManager.Instance.SetPointOccupation(currentPoint, gameObject, false);

            // Avanzar al siguiente punto
            currentPoint++;

            // Si hemos llegado al final del camino
            if (currentPoint >= PathManager.Instance.GetPathLength())
            {
                // Decide qué hacer al final del camino
                return;
            }
        }

        // Moverse hacia el punto actual
        Vector3 dir = currentPathPoint.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }
}