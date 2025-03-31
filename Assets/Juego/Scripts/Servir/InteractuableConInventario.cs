using UnityEngine;
using System.Collections;

public class InteractuableConInventario : MonoBehaviour
{
    public InventoryManager2 inventario;
    public Transform player;
    public float distanciaMaxima = 2.0f;

    private SpriteRenderer spriteRenderer; // Solo se usa para "barra"
    private MovimientoClientesMultiple cliente; // Solo se usa para "cliente"

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cliente = GetComponent<MovimientoClientesMultiple>(); // cliente tendrá un sprite de pedido y lógica asociada
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
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }

        int selectedIndex = inventario.selectedSlot;
        Sprite selectedSprite = inventario.slots[selectedIndex].sprite;

        if (selectedSprite == null)
        {
            Debug.Log("No hay objeto seleccionado en el inventario.");
            yield break;
        }

        switch (tag)
        {
            case "barra":
                if (spriteRenderer != null && spriteRenderer.sprite == null)
                {
                    spriteRenderer.sprite = selectedSprite;
                    inventario.BorrarItem(selectedIndex, selectedSprite);
                }
                break;

            case "cliente":
                if (cliente != null)
                {
                    // Obtener el Item asociado al sprite seleccionado
                    item servedItem = inventario.GetItemBySprite(selectedSprite);
                    // Compara con el cóctel pedido del cliente
                    if (servedItem != null && cliente.requestedCoctel != null &&
                        servedItem.sprite == cliente.requestedCoctel.sprite)  // o compara otro identificador único
                    {
                        Debug.Log("Pedido correcto. Cliente servido.");
                        inventario.BorrarItem(selectedIndex, selectedSprite);

                        // Obtener el precio desde el item y sumarlo
                        GameObject barraUI = GameObject.Find("BarraCobroUI");
                        BarraCobroUI barra = barraUI.GetComponent<BarraCobroUI>();
                        barra.AñadirDinero(servedItem.precio);

                        Destroy(gameObject); // El cliente desaparece
                    }
                    else
                    {
                        Debug.Log("Pedido incorrecto.");
                    }
                }
                break;


            case "basura":
                Debug.Log("Objeto tirado a la basura.");
                inventario.BorrarItem(selectedIndex, selectedSprite);

                break;
        }
    }

}
