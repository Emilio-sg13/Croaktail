using UnityEngine;
using TMPro;


public class BarraCobroUI : MonoBehaviour
{
    
    public TextMeshProUGUI textoCantidad;

    public int cuotaTotal = 100;
    private int totalActual = 0;

    void Start()
    {
        // Inicializa la UI al comenzar el juego
        textoCantidad.text = $"0€ / {cuotaTotal}€";
    }

    public void AñadirDinero(int cantidad)
    {
        totalActual += cantidad;
        totalActual = Mathf.Min(totalActual, cuotaTotal); // No pasar del 100%

        

        textoCantidad.text = $"{totalActual}€ / {cuotaTotal}€";
    }
}
