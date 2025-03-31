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
        if (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
            return;

        int selected = inventario.selectedSlot;
        Sprite selectedSprite = inventario.slots[selected].sprite;
        if (selectedSprite == null) return;

        item item = inventario.GetItemBySprite(selectedSprite);
        if (item == null || item.tipo != TipoItem.Ingrediente)
        {
            Debug.Log("Solo puedes colocar ingredientes aquí.");
            return;
        }

        if (spriteRenderer.sprite != null)
        {
            Debug.Log("Este hueco ya tiene un ingrediente.");
            return;
        }

        // Mostrar el sprite en el hueco
        spriteRenderer.sprite = selectedSprite;

        // Quitar del inventario
        inventario.BorrarItem(selected, selectedSprite);
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
