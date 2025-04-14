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
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        contadorTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (tiempoRestante <= 0)
        {
            minutos = 0;
            segundos = 0;
            contadorTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Comprobamos si se ha alcanzado el objetivo al finalizar el contador
            int dineroConseguido = barraCobroUI.GetTotalActual();
            int dineroObjetivo = GameManager.Instance.CurrentTargetMoney;

            if (dineroConseguido < dineroObjetivo)
            {
                Debug.Log("No se cumplió el objetivo. Dinero conseguido: " + dineroConseguido);
                // Redirigir a la pantalla de derrota
                SceneManager.LoadScene("PantallaDerrota");
            }
            else
            {
                Debug.Log("¡Objetivo cumplido! Dinero conseguido: " + dineroConseguido);
                // Procedemos a la tienda si se cumplió el objetivo
                barraCobroUI.OnIrtiendaButtonClick();
            }
        }
        else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }
}
