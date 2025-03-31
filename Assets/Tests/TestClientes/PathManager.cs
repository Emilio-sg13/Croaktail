using UnityEngine;
using System.Collections.Generic;

// Gestor central de caminos
public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    [System.Serializable]
    public class Path
    {
        public string pathName;
        public Transform[] points;
    }

    [SerializeField]
    public Path[] paths;

    // Estructura para mantener el estado de ocupación de cada punto
    private Dictionary<Transform, GameObject> occupiedPoints = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Inicializar todos los puntos como no ocupados
        foreach (var path in paths)
        {
            foreach (var point in path.points)
            {
                if (point != null && !occupiedPoints.ContainsKey(point))
                {
                    occupiedPoints.Add(point, null);
                }
            }
        }
    }

    public bool IsPointOccupied(Transform point)
    {
        return occupiedPoints.ContainsKey(point) && occupiedPoints[point] != null;
    }

    public void SetPointOccupation(Transform point, GameObject occupier)
    {
        if (occupiedPoints.ContainsKey(point))
        {
            occupiedPoints[point] = occupier;
        }
    }

    public Transform GetPathPoint(int pathIndex, int pointIndex)
    {
        if (pathIndex >= 0 && pathIndex < paths.Length &&
            pointIndex >= 0 && pointIndex < paths[pathIndex].points.Length)
        {
            return paths[pathIndex].points[pointIndex];
        }
        return null;
    }

    public int GetPathLength(int pathIndex)
    {
        if (pathIndex >= 0 && pathIndex < paths.Length)
        {
            return paths[pathIndex].points.Length;
        }
        return 0;
    }

    public int GetPathCount()
    {
        return paths.Length;
    }

    // Verificar si el punto inicial de un camino está libre
    public bool IsPathStartFree(int pathIndex)
    {
        if (pathIndex >= 0 && pathIndex < paths.Length && paths[pathIndex].points.Length > 0)
        {
            return !IsPointOccupied(paths[pathIndex].points[0]);
        }
        return false;
    }


    // Encontrar caminos adyacentes disponibles para un cliente de cierto ancho
    public int[] FindAvailableAdjacentPaths(int width)
    {
        // Lista para almacenar todos los conjuntos de caminos libres
        List<int[]> validCombinations = new List<int[]>();

        // Si solo necesita un camino
        if (width == 1)
        {
            // Recorremos todos los caminos
            for (int i = 0; i < paths.Length; i++)
            {
                // Si el camino i está libre en su punto inicial
                if (IsPathStartFree(i))
                {
                    // Añadimos este camino como opción válida
                    validCombinations.Add(new int[] { i });
                }
            }
        }
        else
        {
            // Para clientes más anchos, buscar todos los bloques de caminos adyacentes
            for (int i = 0; i <= paths.Length - width; i++)
            {
                bool allFree = true;
                for (int j = 0; j < width; j++)
                {
                    // Si cualquiera de los caminos en el bloque no está libre, descartamos
                    if (!IsPathStartFree(i + j))
                    {
                        allFree = false;
                        break;
                    }
                }

                // Si todos los caminos del bloque están libres
                if (allFree)
                {
                    int[] block = new int[width];
                    for (int j = 0; j < width; j++)
                    {
                        block[j] = i + j;
                    }
                    validCombinations.Add(block);
                }
            }
        }

        // Si no hay combinaciones libres, retornamos null
        if (validCombinations.Count == 0)
            return null;

        // Elegimos una combinación al azar entre todas las disponibles
        int randomIndex = Random.Range(0, validCombinations.Count);
        return validCombinations[randomIndex];
    }


    // Comprobar si un cliente puede ocupar múltiples puntos adyacentes horizontalmente
    public bool CanOccupyMultipleHorizontal(int[] pathIndices, int pointIndex, int width, GameObject client)
    {
        // Si el cliente solo necesita un camino, usamos la función original
        if (width <= 1 || pathIndices.Length < width)
            return !IsPointOccupied(GetPathPoint(pathIndices[0], pointIndex));

        // Comprobar si todos los caminos en el ancho requerido están libres en el mismo punto
        for (int i = 0; i < width; i++)
        {
            if (pathIndices[i] < 0 || pathIndices[i] >= paths.Length)
                return false;

            Transform point = GetPathPoint(pathIndices[i], pointIndex);
            if (point == null || IsPointOccupied(point))
                return false;
        }
        return true;
    }

    // Ocupar múltiples puntos horizontalmente
    public void OccupyMultipleHorizontal(int[] pathIndices, int pointIndex, int width, GameObject client)
    {
        for (int i = 0; i < width; i++)
        {
            Transform point = GetPathPoint(pathIndices[i], pointIndex);
            if (point != null)
            {
                SetPointOccupation(point, client);
            }
        }
    }

    // Liberar múltiples puntos horizontalmente
    public void ReleaseMultipleHorizontal(int[] pathIndices, int pointIndex, int width)
    {
        for (int i = 0; i < width; i++)
        {
            Transform point = GetPathPoint(pathIndices[i], pointIndex);
            if (point != null)
            {
                SetPointOccupation(point, null);
            }
        }
    }
}