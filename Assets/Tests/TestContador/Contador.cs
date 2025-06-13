using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Para cambiar de escena
using System.Collections;

public class Contador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contadorTexto;
    [SerializeField] float tiempoRestante;
    public BarraCobroUI barraCobroUI;
    public GameManager gameManager;
    public PlayerController jugador;


    async void Update()
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
                Debug.Log("No se cumple el objetivo. Dinero conseguido: " + dineroConseguido);
                SceneManager.LoadScene("PantallaDerrota");
            }
            else
            {
                StartCoroutine(VictorySequence());

            }
        }
        else
        {
            // Reducir el tiempo restante
            tiempoRestante -= Time.deltaTime;
        }
    }


    IEnumerator VictorySequence()
    {
        // 1) Lanzar animación de victoria
        if (jugador != null)
        {
            Debug.Log("bbbbbbbbbbbbbbb");
            jugador.AnimacionVictoria();
        }


        else { Debug.Log("cccccccccccccccc"); }

        // 2) Opcional: notificar al MoneyManager
        int dineroConseguido = barraCobroUI.GetTotalActual();
        int dineroObjetivo = GameManager.Instance.CurrentTargetMoney;
        

        // 3) Esperar 5 segundos antes de cambiar de escena
        yield return new WaitForSeconds(5f);

        // 4) Cargar la escena de victoria
        MoneyManager.Instance.IrTienda(dineroConseguido, dineroObjetivo);
        SceneManager.LoadScene("PantallaVictoria");
    }

    /// <summary>
    /// Llama a este m閠odo desde el OnClick de un bot髇 para reiniciar el tiempo a 20s.
    /// </summary>
    public void ResetearTiempo()
    {
        tiempoRestante = 20f;
        Debug.Log("Tiempo restablecido a 20 segundos.");

        
        FindFirstObjectByType<BGMController>()?.SaltarUltimos20Segundos();
    }
}
