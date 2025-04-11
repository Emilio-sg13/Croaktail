using UnityEngine;

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
        // Si el jugador está lejos, no hacer nada.
        if (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
            return;

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
            int selected = inventario.selectedSlot;
            Sprite selectedSprite = inventario.slots[selected].sprite;
            if (selectedSprite == null)
            {
                Debug.Log("No hay objeto seleccionado en el inventario para depositar en el HuecoBarra.");
                return;
            }

            item itemToDeposit = inventario.GetItemBySprite(selectedSprite);
            if (itemToDeposit == null || itemToDeposit.tipo != TipoItem.Ingrediente)
            {
                Debug.Log("Solo puedes depositar ingredientes aquí.");
                return;
            }

            // Depositar el ingrediente: se muestra el sprite en el hueco...
            spriteRenderer.sprite = selectedSprite;
            // ...y se quita del inventario
            inventario.BorrarItem(selected, selectedSprite);
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
