using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contadorTexto;
    [SerializeField] float tiempoRestante;
    public BarraCobroUI barraCobroUI;


    /// <summary>
    /// Funcionamiento del contador
    /// </summary>
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
            barraCobroUI.OnIrtiendaButtonClick();
        } else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }
}

