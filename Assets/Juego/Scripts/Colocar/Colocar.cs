using UnityEngine;
using System.Collections;

public class Colocar : MonoBehaviour
{
    // Referencia al InventoryManager para acceder al slot seleccionado.
    public InventoryManager2 inventario;

    // Referencia al Transform del jugador para medir la distancia y moverlo.
    public Transform player;

    // Distancia máxima a la que el jugador debe estar para poder colocar el objeto.
    public float distanciaMaxima = 2.0f;

    // Componente SpriteRenderer que se usará para mostrar el sprite en "barra".
    private SpriteRenderer barraSpriteRenderer;

    void Start()
    {
        // Obtenemos el SpriteRenderer del objeto "barra".
        barraSpriteRenderer = GetComponent<SpriteRenderer>();
        if (barraSpriteRenderer == null)
        {
            Debug.LogError("No se encontró el SpriteRenderer en el objeto 'barra'.");
        }
    }

    void OnMouseDown()
    {
        // Verifica que se haya asignado el Transform del jugador.
        if (player == null)
        {
            Debug.LogError("El Transform del jugador no está asignado en Colocar.cs.");
            return;
        }

        // Verifica si ya hay un objeto en la barra.
        if (barraSpriteRenderer.sprite != null)
        {
            Debug.Log("La barra ya tiene un objeto. No se puede colocar otro.");
            return;
        }

        if (inventario == null)
        {
            Debug.LogError("El InventoryManager no está asignado en el objeto 'barra'.");
            return;
        }

        // Obtiene el índice del slot seleccionado y el sprite del slot.
        int selectedIndex = inventario.selectedSlot;
        Sprite selectedSprite = inventario.slots[selectedIndex].sprite;
        if (selectedSprite == null)
        {
            Debug.Log("No hay objeto en el slot seleccionado.");
            return;
        }

        // Verifica la distancia entre el jugador y la barra.
        float distancia = Vector3.Distance(player.position, transform.position);
        if (distancia > distanciaMaxima)
        {
            // El jugador no está lo suficientemente cerca: se le ordena moverse hacia la barra.
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.MoverHacia(transform.position);
            }
            // Inicia una coroutine que espera a que el jugador se acerque.
            StartCoroutine(EsperarYColocar(selectedSprite, selectedIndex));
        }
        else
        {
            // Si ya está cerca, coloca el objeto inmediatamente.
            ColocarObjeto(selectedSprite, selectedIndex);
        }
    }

    // Coroutine que espera hasta que el jugador esté lo suficientemente cerca de la barra.
    IEnumerator EsperarYColocar(Sprite selectedSprite, int selectedIndex)
    {
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }
        ColocarObjeto(selectedSprite, selectedIndex);
    }

    // Método que coloca el objeto en la barra y lo elimina del inventario.
    void ColocarObjeto(Sprite selectedSprite, int selectedIndex)
    {
        // Verifica nuevamente que la barra esté vacía.
        if (barraSpriteRenderer.sprite != null)
        {
            Debug.Log("La barra ya tiene un objeto, no se puede colocar.");
            return;
        }

        // Asigna el sprite del slot seleccionado al SpriteRenderer de "barra".
        barraSpriteRenderer.sprite = selectedSprite;

        // Elimina el sprite del slot seleccionado.
        inventario.slots[selectedIndex].sprite = null;

        // Si el objeto ocupa dos slots, elimina también el sprite en el slot adyacente.
        if (selectedIndex > 0 && inventario.slots[selectedIndex - 1].sprite == selectedSprite)
        {
            inventario.slots[selectedIndex - 1].sprite = null;
        }
        else if (selectedIndex < inventario.slots.Length - 1 && inventario.slots[selectedIndex + 1].sprite == selectedSprite)
        {
            inventario.slots[selectedIndex + 1].sprite = null;
        }
    }
}
