using UnityEngine;
using UnityEngine.UI;      // Para usar el componente Slider


public class MezclaUI : MonoBehaviour
{
    // Referencia al Slider que representa el progreso de la mezcla
    public Slider barraProgreso;

    // N鷐ero de clics necesarios para completar la mezcla
    public int clicsNecesarios = 5;
    // Contador de clics realizados hasta el momento
    private int clicsActuales = 0;
    // Acci髇 que se invocar?cuando se complete la mezcla
    private System.Action onCompletar;
   
    public ParticleSystem efectoChispasUI;
    public Transform puntoChispaUI;

    // Se ejecuta al iniciar, desactiva la UI de mezcla por defecto
    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Inicializa la barra de progreso y el texto, y activa la UI de mezcla.
    /// </summary>
    /// <param name="onCompletarCallback">Callback que se ejecutar?al completar la mezcla</param>
    public void IniciarProgreso(System.Action onCompletarCallback)
    {
        clicsActuales = 0;                      // Reinicia el contador de clics
        barraProgreso.value = 0;                // Reinicia el slider a 0%

        onCompletar = onCompletarCallback;        // Asigna la acci髇 a ejecutar al terminar
        gameObject.SetActive(true);             // Activa la UI de mezcla
    }

    /// <summary>
    /// M閠odo que se debe llamar cada vez que el jugador hace clic en el mezclador.
    /// Incrementa el contador y actualiza el slider y el porcentaje mostrado.
    /// Si se alcanza el n鷐ero de clics necesarios, se oculta la UI y se invoca el callback.
    /// </summary>
    public void ClicMezclar()
    {
        // Si la UI de mezcla no est?activa, no se realiza nada
        if (!gameObject.activeSelf) return;

        if (UpgradeData.mezcladoRapido)
        {
            clicsNecesarios = 2;
        }

        clicsActuales++;    // Incrementa el contador de clics
        // Calcula el progreso en forma de valor entre 0 y 1
        float progreso = (float)clicsActuales / clicsNecesarios;
        // Actualiza el valor del slider con el progreso calculado
        barraProgreso.value = progreso;

        // Reproducir chispa
        if (efectoChispasUI != null && puntoChispaUI != null)
        {
            efectoChispasUI.transform.position = puntoChispaUI.position;
            efectoChispasUI.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            efectoChispasUI.Play();

            Debug.Log("Chispa UI reproducida en: " + puntoChispaUI.position);
        }
        else
        {
            Debug.LogWarning("No se encontró el efecto de chispas o el punto de aparición.");
        }


        // Si se han realizado clics suficientes para completar la mezcla:
        if (clicsActuales >= clicsNecesarios)
        {
            gameObject.SetActive(false);   // Se oculta la UI de mezcla
            onCompletar?.Invoke();           // Se invoca el callback asignado (si existe)
        }
    }
}
