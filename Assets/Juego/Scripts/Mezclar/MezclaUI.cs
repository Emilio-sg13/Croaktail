using UnityEngine;
using UnityEngine.UI;      // Para usar el componente Slider


public class MezclaUI : MonoBehaviour
{
    // Referencia al Slider que representa el progreso de la mezcla
    public Slider barraProgreso;

    // Número de clics necesarios para completar la mezcla
    public int clicsNecesarios = 5;
    // Contador de clics realizados hasta el momento
    private int clicsActuales = 0;
    // Acción que se invocará cuando se complete la mezcla
    private System.Action onCompletar;

    // Se ejecuta al iniciar, desactiva la UI de mezcla por defecto
    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Inicializa la barra de progreso y el texto, y activa la UI de mezcla.
    /// </summary>
    /// <param name="onCompletarCallback">Callback que se ejecutará al completar la mezcla</param>
    public void IniciarProgreso(System.Action onCompletarCallback)
    {
        clicsActuales = 0;                      // Reinicia el contador de clics
        barraProgreso.value = 0;                // Reinicia el slider a 0%

        onCompletar = onCompletarCallback;        // Asigna la acción a ejecutar al terminar
        gameObject.SetActive(true);             // Activa la UI de mezcla
    }

    /// <summary>
    /// Método que se debe llamar cada vez que el jugador hace clic en el mezclador.
    /// Incrementa el contador y actualiza el slider y el porcentaje mostrado.
    /// Si se alcanza el número de clics necesarios, se oculta la UI y se invoca el callback.
    /// </summary>
    public void ClicMezclar()
    {
        // Si la UI de mezcla no está activa, no se realiza nada
        if (!gameObject.activeSelf) return;

        clicsActuales++;    // Incrementa el contador de clics
        // Calcula el progreso en forma de valor entre 0 y 1
        float progreso = (float)clicsActuales / clicsNecesarios;
        // Actualiza el valor del slider con el progreso calculado
        barraProgreso.value = progreso;


        // Si se han realizado clics suficientes para completar la mezcla:
        if (clicsActuales >= clicsNecesarios)
        {
            gameObject.SetActive(false);   // Se oculta la UI de mezcla
            onCompletar?.Invoke();           // Se invoca el callback asignado (si existe)
        }
    }
}
