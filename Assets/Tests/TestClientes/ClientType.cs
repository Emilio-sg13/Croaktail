// Definición de tipos de clientes
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClientType
{
    //public string[] cocteles;
    public List<item> Cocteles;
    public string typeName;
    public GameObject clientPrefab;
    public int width = 1;
    public float speed = 2.0f;
    public float spawnWeight = 1.0f; // Probabilidad relativa de generar este tipo
}