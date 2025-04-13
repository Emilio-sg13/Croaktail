using UnityEngine;
using System.Collections.Generic;
using TMPro; 

public class TiendaManager : MonoBehaviour
{
    // Referencias a los SpriteRenderer donde se mostrarán las dos mejoras
    public SpriteRenderer mejoraSlot1;
    public SpriteRenderer mejoraSlot2;

    // Arreglo de sprites para las 5 mejoras (el orden del array corresponde a mejora1, mejora2, ...)
    public Sprite[] mejorasSprites;

    // Referencias a los textos de los botones (TextMeshProUGUI) que muestran la acción a realizar
    public TextMeshProUGUI textoBotonSlot1;
    public TextMeshProUGUI textoBotonSlot2;

    // Variables para recordar qué mejora se muestra en cada slot
    private int mejoraMostradaSlot1 = -1;
    private int mejoraMostradaSlot2 = -1;

    // Precio de cada mejora; se usa para determinar si el jugador tiene suficiente dinero.
    public int precioMejora = 100;

    void Start()
    {
        MostrarMejorasAleatorias();
    }

    /// <summary>
    /// Selecciona aleatoriamente dos mejoras que aún no han sido compradas y las muestra en los slots.
    /// </summary>
    public void MostrarMejorasAleatorias()
    {
        // 1. Crear una lista de índices de mejoras que el jugador aún no ha comprado
        List<int> mejorasNoCompradas = new List<int>();

        if (!UpgradeData.mejora1) mejorasNoCompradas.Add(0); // índice 0 -> mejora1
        if (!UpgradeData.mejora2) mejorasNoCompradas.Add(1); // índice 1 -> mejora2
        if (!UpgradeData.mejora3) mejorasNoCompradas.Add(2); // índice 2 -> mejora3
        if (!UpgradeData.mejora4) mejorasNoCompradas.Add(3); // índice 3 -> mejora4
        if (!UpgradeData.mejora5) mejorasNoCompradas.Add(4); // índice 4 -> mejora5

        // 2. Si la lista está vacía, no hay mejoras disponibles
        if (mejorasNoCompradas.Count == 0)
        {
            Debug.Log("No hay mejoras disponibles para comprar.");
            mejoraSlot1.enabled = false;
            mejoraSlot2.enabled = false;
            textoBotonSlot1.text = "";
            textoBotonSlot2.text = "";
            return;
        }

        // 3. Escoger aleatoriamente la primera mejora
        int randomIndex = Random.Range(0, mejorasNoCompradas.Count);
        mejoraMostradaSlot1 = mejorasNoCompradas[randomIndex];
        mejorasNoCompradas.RemoveAt(randomIndex); // para evitar repetirla

        // 4. Asignar el sprite de la mejora seleccionada al primer slot y poner el texto "Comprar"
        mejoraSlot1.sprite = mejorasSprites[mejoraMostradaSlot1];
        mejoraSlot1.enabled = true;
        textoBotonSlot1.text = "Comprar";

        // 5. Si quedan mejoras sin comprar, escoger la segunda; en caso contrario, deshabilitar el segundo slot
        if (mejorasNoCompradas.Count > 0)
        {
            randomIndex = Random.Range(0, mejorasNoCompradas.Count);
            mejoraMostradaSlot2 = mejorasNoCompradas[randomIndex];
            mejorasNoCompradas.RemoveAt(randomIndex);

            mejoraSlot2.sprite = mejorasSprites[mejoraMostradaSlot2];
            mejoraSlot2.enabled = true;
            textoBotonSlot2.text = "Comprar";
        }
        else
        {
            mejoraMostradaSlot2 = -1;
            mejoraSlot2.enabled = false;
            textoBotonSlot2.text = "";
        }
    }

    /// <summary>
    /// Se invoca al pulsar el botón de compra del slot 1.
    /// Comprueba si hay suficiente dinero en el MoneyManager y, si es así, se descuenta el precio y se marca la mejora como comprada.
    /// </summary>
    public void ComprarMejoraSlot1()
    {
        if (mejoraMostradaSlot1 == -1)
            return; // No hay mejora asignada en este slot

        // Usamos el MoneyManager para comprobar el dinero acumulado.
        if (MoneyManager.Instance != null && MoneyManager.Instance.Ganancias >= precioMejora)
        {
            // Se descuenta el coste de la mejora.
            MoneyManager.Instance.Comprar(precioMejora);

            // Marcar la mejora como comprada en UpgradeData
            switch (mejoraMostradaSlot1)
            {
                case 0: UpgradeData.mejora1 = true; break;
                case 1: UpgradeData.mejora2 = true; break;
                case 2: UpgradeData.mejora3 = true; break;
                case 3: UpgradeData.mejora4 = true; break;
                case 4: UpgradeData.mejora5 = true; break;
            }

            // Se oculta el sprite de la mejora y se actualiza el texto del botón a "Comprado"
            mejoraSlot1.enabled = false;
            textoBotonSlot1.text = "Comprado";
            Debug.Log("Compraste la mejora del slot 1.");
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para comprar la mejora del slot 1.");
        }
    }

    /// <summary>
    /// Se invoca al pulsar el botón de compra del slot 2.
    /// Comprueba si hay suficiente dinero en el MoneyManager y, si es así, se descuenta el precio y se marca la mejora como comprada.
    /// </summary>
    public void ComprarMejoraSlot2()
    {
        if (mejoraMostradaSlot2 == -1)
            return;

        if (MoneyManager.Instance != null && MoneyManager.Instance.Ganancias >= precioMejora)
        {
            MoneyManager.Instance.Comprar(precioMejora);

            switch (mejoraMostradaSlot2)
            {
                case 0: UpgradeData.mejora1 = true; break;
                case 1: UpgradeData.mejora2 = true; break;
                case 2: UpgradeData.mejora3 = true; break;
                case 3: UpgradeData.mejora4 = true; break;
                case 4: UpgradeData.mejora5 = true; break;
            }

            mejoraSlot2.enabled = false;
            textoBotonSlot2.text = "Comprado";
            Debug.Log("Compraste la mejora del slot 2.");
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para comprar la mejora del slot 2.");
        }
    }

    public void OnSiguienteButtonClick()
    {
        GameManager.Instance.LoadNextNight();

    }
}
