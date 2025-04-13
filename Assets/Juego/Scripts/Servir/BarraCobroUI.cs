using UnityEngine;
using TMPro;

public class BarraCobroUI : MonoBehaviour
{
    // Slider que representa gráficamente el progreso hasta la cuota.

    // Componente de texto que muestra el dinero acumulado y la cuota (por ejemplo, "X€ / 100€")
    public TextMeshProUGUI texto;
    // La cuota total que se usará como referencia para llenar la barra (por ejemplo, 100€).
    // Cuando totalActual llegue a este valor, el slider se llenará.
    public int cuotaTotal = 100;

    // El total acumulado se irá sumando según sirvas cócteles.
    private int totalActual = 0;

    void Start()
    {


        // Inicializamos el total y actualizamos la UI.
        totalActual = 0;
        ActualizarUI();
    }

    /// <summary>
    /// Se suma la cantidad indicada (por ejemplo, el precio del cóctel servido) al total acumulado.
    /// Luego se actualizan el slider y el texto de la barra.
    /// </summary>
    /// <param name="cantidad">La cantidad a sumar (precio del cóctel servido).</param>
    public void AñadirDinero(int cantidad)
    {
        // Suma la cantidad al total acumulado
        totalActual += cantidad;
        // Actualiza el slider y el texto.
        ActualizarUI();
    }

    /// <summary>
    /// Actualiza la UI:
    /// - El slider se fija a totalActual hasta cuotaTotal (más allá de ese valor, se mantiene en el máximo).
    /// - El texto muestra el total acumulado y la cuota (por ejemplo, "120€ / 100€").
    /// </summary>
    private void ActualizarUI()
    {
        // Si totalActual es menor o igual a la cuota, el slider se llena proporcionalmente.


        // Actualiza el texto con el formato deseado.
        if (texto != null)
            texto.text = $"{totalActual}€ / {cuotaTotal}€";
    }
}
