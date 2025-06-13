using UnityEngine;
using UnityEngine.UI;      // Para usar el componente Image

public class MezclaUI : MonoBehaviour
{
    [Header("UI de progreso")]
    // Referencia a la Image que representa el progreso (debe estar en modo Filled)
    public Image imagenProgreso;

    [Header("Configuración de clics")]
    public int clicsNecesarios = 5;
    private int clicsActuales = 0;

    // Callback que se invocará cuando se complete la mezcla
    private System.Action onCompletar;

    [Header("Efecto de chispas")]
    public ParticleSystem efectoChispasUI;
    public Transform puntoChispaUI;

    void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Inicializa la imagen de progreso a 0 y activa la UI.
    /// </summary>
    public void IniciarProgreso(System.Action onCompletarCallback)
    {
        clicsActuales = 0;
        if (imagenProgreso != null)
            imagenProgreso.fillAmount = 0f;

        onCompletar = onCompletarCallback;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Debe llamarse al hacer clic; actualiza el fill de la imagen y dispara chispa.
    /// </summary>
    public void ClicMezclar()
    {
        if (!gameObject.activeSelf) return;

        if (UpgradeData.mezcladoRapido)
            clicsNecesarios = 2;

        clicsActuales++;
        float progreso = (float)clicsActuales / clicsNecesarios;

        // Actualiza la imagen de progreso
        if (imagenProgreso != null)
            imagenProgreso.fillAmount = Mathf.Clamp01(progreso);
        else
            Debug.LogWarning("⚠️ No se asignó la imagen de progreso.");

        // Reproduce chispa
        if (efectoChispasUI != null && puntoChispaUI != null)
        {
            efectoChispasUI.transform.position = puntoChispaUI.position;
            efectoChispasUI.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            efectoChispasUI.Play();
            Debug.Log("💥 Chispa UI reproducida en: " + puntoChispaUI.position);
        }

        // Completado
        if (clicsActuales >= clicsNecesarios)
        {
            gameObject.SetActive(false);
            onCompletar?.Invoke();
        }
    }
}
