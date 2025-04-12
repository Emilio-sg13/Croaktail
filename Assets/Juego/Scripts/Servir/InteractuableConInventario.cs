using UnityEngine;
using System.Collections;


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
        // Espera hasta que el jugador esté lo suficientemente cerca
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }

        int selectedIndex = inventario.selectedSlot;

        switch (tag)
        {
            case "cliente":
                {
                    // Para clientes, se requiere que haya un objeto seleccionado en el inventario.
                    Sprite selectedSprite = inventario.slots[selectedIndex].sprite;
                    if (selectedSprite == null)
                    {
                        Debug.Log("No hay objeto seleccionado en el inventario.");
                        yield break;
                    }

                    item coctelSeleccionado = inventario.GetItemBySprite(selectedSprite);
                    item coctelPedido = cliente.requestedCoctel;

                    if (coctelSeleccionado != null && coctelPedido != null &&
                        coctelSeleccionado.itemName == coctelPedido.itemName)
                    {
                        Debug.Log("Pedido correcto. Cliente servido.");

                        // Eliminar el cóctel del inventario.
                        inventario.BorrarItem(selectedIndex, selectedSprite);

                        // Sumar dinero a la barra de cobro.
                        GameObject barraUIObj = GameObject.Find("UI");
                        if (barraUIObj != null)
                        {
                            BarraCobroUI barra = barraUIObj.GetComponent<BarraCobroUI>();
                            if (barra != null)
                            {
                                barra.AñadirDinero(coctelSeleccionado.precio);
                            }
                        }


                        // Destruir al cliente servido.
                        Destroy(cliente.gameObject);
                    }
                    else
                    {
                        Debug.Log("Pedido incorrecto.");
                    }
                }
                break;

            case "barra":
                {
                    // Caso "barra": se puede recoger o depositar, según si hay un item depositado.
                    // Primero, comprobamos si la barra ya tiene un objeto (pickup).
                    if (spriteRenderer.sprite != null)
                    {
                        // Recoger el item depositado en la barra.
                        item pickedItem = inventario.GetItemBySprite(spriteRenderer.sprite);
                        if (pickedItem != null)
                        {
                            // Se intenta añadir el item al inventario sin depender del slot seleccionado.
                            bool added = inventario.TryAddItem(pickedItem);
                            if (added)
                            {
                                Debug.Log("Item recogido desde la barra y añadido al inventario.");
                                // Vaciar la barra para permitir nuevos depósitos.
                                spriteRenderer.sprite = null;
                            }
                            else
                            {
                                Debug.Log("Inventario lleno, no se puede recoger el item desde la barra.");
                            }
                        }
                        else
                        {
                            Debug.Log("No se pudo identificar el item en la barra.");
                        }
                    }
                    else
                    {
                        // Si la barra está vacía, depositar el item seleccionado, solo si existe.
                        Sprite selectedSprite = inventario.slots[selectedIndex].sprite;
                        if (selectedSprite != null)
                        {
                            spriteRenderer.sprite = selectedSprite;
                            inventario.BorrarItem(selectedIndex, selectedSprite);
                        }
                        else
                        {
                            Debug.Log("No hay objeto en el inventario para depositar en la barra.");
                        }
                    }
                }
                break;

            case "basura":
                {
                    // En basura sí se requiere tener un objeto seleccionado en el inventario.
                    Sprite selectedSprite = inventario.slots[selectedIndex].sprite;
                    if (selectedSprite == null)
                    {
                        Debug.Log("No hay objeto en el inventario para tirar a la basura.");
                        yield break;
                    }

                    Debug.Log("Objeto tirado a la basura.");
                    inventario.BorrarItem(selectedIndex, selectedSprite);
                }
                break;
        }
    }

}
