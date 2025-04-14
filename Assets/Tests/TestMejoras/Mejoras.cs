using UnityEngine;

public class Mejoras : MonoBehaviour
{
    [SerializeField]
    private ClientGenerator clientGenerator;

    // Llama a la función de mejora solo si UpgradeData.mejora1 es true.
    public void AplicarMejoraDeSpawnInterval()
    {
        if (UpgradeData.masClientes)
        {
            clientGenerator.DoblarSpawnInterval();
        }
        else
        {
            Debug.Log("UpgradeData.masClientes es false. No se aplica la mejora.");
        }
    }
}
