using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class Contador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contadorTexto;
    [SerializeField] float tiempoRestante;
    public BarraCobroUI barraCobroUI;
    public GameManager gameManager;

    void Update()
    {
        // Formatear y mostrar minutos:segundos
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        contadorTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (tiempoRestante <= 0f)
        {
            // Aseguramos 00:00
            contadorTexto.text = "00:00";

            // Comprobamos si se ha alcanzado el objetivo al finalizar el contador
            int dineroConseguido = barraCobroUI.GetTotalActual();
            int dineroObjetivo = GameManager.Instance.CurrentTargetMoney;

            if (dineroConseguido < dineroObjetivo)
            {
                Debug.Log("No se cumplió el objetivo. Dinero conseguido: " + dineroConseguido);
                SceneManager.LoadScene("PantallaDerrota");
            }
            else
            {
                Debug.Log("¡Objetivo cumplido! Dinero conseguido: " + dineroConseguido);
                MoneyManager.Instance.IrTienda(dineroConseguido, dineroObjetivo);
                SceneManager.LoadScene("PantallaVictoria");
            }
        }
        else
        {
            // Reducir el tiempo restante
            tiempoRestante -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Llama a este método desde el OnClick de un botón para reiniciar el tiempo a 20s.
    /// </summary>
    public void ResetearTiempo()
    {
        tiempoRestante = 20f;
        Debug.Log("Tiempo restablecido a 20 segundos.");
    }
}
