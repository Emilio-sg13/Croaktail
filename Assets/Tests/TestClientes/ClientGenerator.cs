// Generador de clientes
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
        GameObject client = Instantiate(selectedType.clientPrefab, spawnPoint.position, Quaternion.identity);

        // Configurar el componente de movimiento
        MovimientoClientesMultiple movement = client.GetComponent<MovimientoClientesMultiple>();
        if (movement != null)
        {
            movement.SetPathIndices(availablePaths);
            movement.SetClientWidth(selectedType.width);
            movement.SetSpeed(selectedType.speed);
        }
    }

    private ClientType GetRandomClientType()
    {
        if (clientTypes.Length == 0)
            return null;

        // Calcular el peso total
        float totalWeight = 0;
        foreach (var type in clientTypes)
        {
            totalWeight += type.spawnWeight;
        }

        // Seleccionar un tipo basado en su peso
        float randomValue = Random.Range(0, totalWeight);
        float weightSum = 0;

        foreach (var type in clientTypes)
        {
            weightSum += type.spawnWeight;
            if (randomValue <= weightSum)
            {
                return type;
            }
        }

        return clientTypes[0]; // Fallback
    }
}
