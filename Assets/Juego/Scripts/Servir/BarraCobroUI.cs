using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class BarraCobroUI : MonoBehaviour
{

    // Componente de texto que mostrará el progreso en formato "X€ / Y€".
    public TextMeshProUGUI texto;
    // Valor de respaldo en caso de que el GameManager no esté presente.
    public int cuotaTotalFallback = 100;

    // Total acumulado de dinero.
    private int totalActual = 0;

    // Propiedad que devuelve la cuota total a partir del GameManager,
    // o usa el valor de respaldo (cuotaTotalFallback) si no hay GameManager.
    private int cuotaTotal
    {
        get
        {
            if (GameManager.Instance != null)
                return GameManager.Instance.CurrentTargetMoney;
            return cuotaTotalFallback;
        }
    }

    // Inicializa el slider y el texto con el valor de objetivo obtenido del GameManager.
    void Start()
    {

        // Se muestra el estado inicial, por ejemplo "0€ / 100€".
        texto.text = $"0€ / {cuotaTotal}€";
        ActualizarUI();
    }

    /// <summary>
    /// Suma la cantidad indicada (precio del cóctel servido) al total acumulado y actualiza la UI.
    /// El slider se incrementa hasta el máximo de cuotaTotal, y el texto se actualiza con el formato "X€ / Y€".
    /// </summary>
    /// <param name="cantidad">Cantidad a sumar al total (precio del cóctel servido).</param>
    public void AñadirDinero(int cantidad)
    {
        // Suma la cantidad al total acumulado.
        totalActual += cantidad;
        // Asegurarse de no superar la cuota definida por GameManager.
        totalActual = Mathf.Min(totalActual, cuotaTotal);

        // Actualiza el texto, mostrando el total y la cuota.
        texto.text = $"{totalActual}€ / {cuotaTotal}€";

        ActualizarUI();
    }

    public void OnFinalizarButtonClick()
    {
        GameManager.Instance.FinalizeGame();
        SceneManager.LoadScene("MainMenu");

    }

    public void OnIrtiendaButtonClick()
    {
        SceneManager.LoadScene("Tienda");

    }

    /// <summary>
    /// Actualiza la UI:
    /// - El slider se fija a totalActual hasta cuotaTotal (más allá de ese valor, se mantiene en el máximo).
    /// - El texto muestra el total acumulado y la cuota (por ejemplo, "120€ / 100€").
    /// </summary>
    private void ActualizarUI()
    {
        // Actualiza el texto con el formato deseado.
        if (texto != null)
            texto.text = $"{totalActual}€ / {cuotaTotal}€";
    }
}
