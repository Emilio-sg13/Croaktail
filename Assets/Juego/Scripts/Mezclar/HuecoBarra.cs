using UnityEngine;
using System.Collections;

public class HuecoBarra : MonoBehaviour
{
    public InventoryManager2 inventario;
    public Transform player;
    public float distanciaMaxima = 2f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {

        if (player == null || inventario == null) return;

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.MoverHacia(transform.position);
        }

        StartCoroutine(EsperarYActuar());
    }

    IEnumerator EsperarYActuar()
    {
        // Espera hasta que el jugador est?lo suficientemente cerca
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }
        int selected = inventario.selectedSlot;
        // Caso 1: Si el hueco ya tiene un ingrediente depositado, se intenta recogerlo
        if (spriteRenderer.sprite != null)
        {
            // Se obtiene el item asociado al sprite del hueco
            item pickedItem = inventario.GetItemBySprite(spriteRenderer.sprite);
            if (pickedItem != null)
            {
                // Se intenta añadir el item al inventario (sin depender del slot seleccionado)
                bool added = inventario.TryAddItem(pickedItem);
                if (added)
                {
                    Debug.Log("Item recogido desde HuecoBarra y añadido al inventario.");
                    // Vaciar el hueco para permitir nuevos depósitos
                    spriteRenderer.sprite = null;
                }
                else
                {
                    Debug.Log("Inventario lleno, no se puede recoger el item desde HuecoBarra.");
                }
            }
            else
            {
                Debug.Log("No se pudo identificar el item en el HuecoBarra.");
            }
        }
        else // Caso 2: Hueco vacío intentar depositar el item seleccionado del inventario
        {

            Sprite selectedSprite = inventario.slots[selected].sprite;
            if (selectedSprite != null)
            {
                spriteRenderer.sprite = selectedSprite;
                inventario.BorrarItem(selected, selectedSprite);
            }
            else
            {
                Debug.Log("No hay objeto en el inventario para depositar en la barra.");
            }
        }
    }

    public void Vaciar()
    {
        spriteRenderer.sprite = null;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }
}
