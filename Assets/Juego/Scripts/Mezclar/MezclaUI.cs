using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MezclaUI : MonoBehaviour
{
    public Slider barraProgreso;
    public TextMeshProUGUI texto;
    public int clicsNecesarios = 5;
    private int clicsActuales = 0;
    private System.Action onCompletar;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void IniciarProgreso(System.Action onCompletarCallback)
    {
        clicsActuales = 0;
        barraProgreso.value = 0;
        texto.text = "Mezclando... 0%";
        onCompletar = onCompletarCallback;
        gameObject.SetActive(true);
    }

    public void ClicMezclar()
    {
        if (!gameObject.activeSelf) return;

        clicsActuales++;
        float progreso = (float)clicsActuales / clicsNecesarios;
        barraProgreso.value = progreso;
        texto.text = $"Mezclando... {(int)(progreso * 100)}%";

        if (clicsActuales >= clicsNecesarios)
        {
            gameObject.SetActive(false);
            onCompletar?.Invoke();
        }
    }
}
