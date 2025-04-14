using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI contadorTexto;
    [SerializeField] float tiempoRestante;

    void Update()
    {
        tiempoRestante -= Time.deltaTime;
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        contadorTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
