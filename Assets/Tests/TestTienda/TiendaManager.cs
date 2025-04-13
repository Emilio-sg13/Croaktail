using UnityEngine;
using System.Collections.Generic;
using TMPro; // Asegúrate de incluir la librería para TextMeshPro

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

    // Simulación del dinero del jugador y el precio de cada mejora
    public int dineroJugador = 500;      // Ejemplo de cantidad de dinero
    public int precioMejora = 100;         // Precio de cada mejora

    /*
    public int precioMejora1 = 0;
    public int precioMejora2 = 0;  
    public int precioMejora3 = 0;
    public int precioMejora4 = 0;
    public int precioMejora5 = 0;
    */

    void Start()
    {
        MostrarMejorasAleatorias();
    }

    // Método para seleccionar dos mejoras aleatorias entre las que aún no se han comprado
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
            // Opcionalmente, puedes desactivar o modificar los textos de los botones aquí
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

    // Método que se invoca al pulsar el botón de compra de la mejora del slot 1
    public void ComprarMejoraSlot1()
    {
        if (mejoraMostradaSlot1 == -1)
            return; // No hay mejora asignada en este slot

        // Comprobar si el jugador tiene suficiente dinero
        if (dineroJugador >= precioMejora)
        {
            dineroJugador -= precioMejora; // se descuenta el precio

            // Marcar la mejora como comprada en UpgradeData
            switch (mejoraMostradaSlot1)
            {
                case 0: UpgradeData.mejora1 = true; break;
                case 1: UpgradeData.mejora2 = true; break;
                case 2: UpgradeData.mejora3 = true; break;
                case 3: UpgradeData.mejora4 = true; break;
                case 4: UpgradeData.mejora5 = true; break;
            }

            // Ocultar el sprite de la mejora y actualizar el texto del botón a "Comprado"
            mejoraSlot1.enabled = false;
            textoBotonSlot1.text = "Comprado";
            Debug.Log("Compraste la mejora del slot 1.");

            // Opcionalmente, podrías refrescar la selección de mejoras si lo deseas:
            // MostrarMejorasAleatorias();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para comprar la mejora del slot 1.");
        }
    }

    // Método que se invoca al pulsar el botón de compra de la mejora del slot 2
    public void ComprarMejoraSlot2()
    {
        if (mejoraMostradaSlot2 == -1)
            return;

        if (dineroJugador >= precioMejora)
        {
            dineroJugador -= precioMejora;

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
}
