using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;

public class InteractuableConInventario : MonoBehaviour
{
    public InventoryManager2 inventario;
    public Transform player;
    public float distanciaMaxima = 2.0f;

    private SpriteRenderer spriteRenderer; // Usado solo para barra
    private MovimientoClientesMultiple cliente; // Usado para clientes

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cliente = GetComponent<MovimientoClientesMultiple>();
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
            Debug.Log(selectedSprite);
            Debug.Log("No hay objeto seleccionado en el inventario.");
            yield break;
        }

        // Lógica según el tag del objeto
        switch (tag)
        {
            case "cliente":
                if (cliente != null)
                {
                    item coctelSeleccionado = inventario.GetItemBySprite(selectedSprite);
                    item coctelPedido = cliente.requestedCoctel;

                    if (coctelSeleccionado != null && coctelPedido != null &&
                        coctelSeleccionado.itemName == coctelPedido.itemName)
                    {
                        Debug.Log("Pedido correcto. Cliente servido.");

                        // Eliminar el cóctel del inventario
                        inventario.BorrarItem(selectedIndex, selectedSprite);

                        // Sumar dinero a la barra de cobro
                        GameObject barraUIObj = GameObject.Find("BarraCobroUI");
                        if (barraUIObj != null)
                        {
                            BarraCobroUI barra = barraUIObj.GetComponent<BarraCobroUI>();
                            barra?.AñadirDinero(coctelSeleccionado.precio);
                        }

                        // Destruir al cliente servido
                        Destroy(cliente.gameObject);
                    }
                    else
                    {
                        Debug.Log("Pedido incorrecto.");
                    }
                }
                break;

            case "barra":
                Debug.Log(selectedSprite);
                
                if (spriteRenderer != null && spriteRenderer.sprite == null)
                {
                    spriteRenderer.sprite = selectedSprite;
                    inventario.BorrarItem(selectedIndex, selectedSprite);
                }
                break;

            case "basura":
                Debug.Log("Objeto tirado a la basura.");
                inventario.BorrarItem(selectedIndex, selectedSprite);
                break;
        }
    }
}
