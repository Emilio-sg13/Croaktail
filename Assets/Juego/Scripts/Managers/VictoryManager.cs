using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{

    public TextMeshProUGUI gananciasText;

    void Start()
    {

        if(MoneyManager.Instance != null)
        {
            Debug.Log(MoneyManager.Instance.Ganancias);
        }

        // Obtiene las ganancias acumuladas
        int gan = MoneyManager.Instance != null ? MoneyManager.Instance.Ganancias : 0;
        // Muestra en pantalla
        gananciasText.text = $"Ganaste: {gan}€";
    }

    public void OnIrTiendaButtonClick()
    {
        SceneManager.LoadScene("Tienda");
    }
}
