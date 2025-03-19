using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance;

    [System.Serializable]
    public class PathPoint
    {
        public Transform point;
        public bool isOccupied = false;
        public GameObject occupiedBy = null;
    }

    [SerializeField]
    public PathPoint[] pathPoints;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsPointOccupied(int pointIndex)
    {
        if (pointIndex >= 0 && pointIndex < pathPoints.Length)
            return pathPoints[pointIndex].isOccupied;
        return false;
    }

    public void SetPointOccupation(int pointIndex, GameObject occupier, bool occupied)
    {
        if (pointIndex >= 0 && pointIndex < pathPoints.Length)
        {
            pathPoints[pointIndex].isOccupied = occupied;
            pathPoints[pointIndex].occupiedBy = occupied ? occupier : null;
        }
    }

    public Transform GetPathPoint(int index)
    {
        if (index >= 0 && index < pathPoints.Length)
            return pathPoints[index].point;
        return null;
    }

    public int GetPathLength()
    {
        return pathPoints.Length;
    }
}