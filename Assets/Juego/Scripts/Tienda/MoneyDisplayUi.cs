using UnityEngine;
using TMPro;

public class MoneyDisplayUI : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI
    public TextMeshProUGUI moneyText;

    void Update()
    {
        // Asegurarse de que MoneyManager.Instance esté asignado
        if (MoneyManager.Instance != null)
        {
            // Actualiza el texto con el valor actual de Ganancias
            moneyText.text = "Ganancias: " + MoneyManager.Instance.Ganancias + "€";
        }
    }
}
