using UnityEngine;
using UnityEngine.UI;

public class InventoryManager2 : MonoBehaviour
{
    // Asigna en el Inspector las im�genes correspondientes a los 3 slots.
    public Image[] slots;

    // �ndice del slot actualmente seleccionado
    public int selectedSlot = 0;

    public item[] todosLosItems;


    void Start()
    {
        ActualizarSeleccionSlot();
    }

    void Update()
    {
        // Selecci�n mediante la rueda del rat�n
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            selectedSlot = (selectedSlot + 1) % slots.Length;
            ActualizarSeleccionSlot();
        }
        else if (scroll < 0)
        {
            selectedSlot = (selectedSlot - 1 + slots.Length) % slots.Length;
            ActualizarSeleccionSlot();
        }
    }

    // M�todo para seleccionar un slot haciendo clic en el mismo (enl�zalo al OnClick del bot�n o imagen)
    public void SeleccionarSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            selectedSlot = index;
            ActualizarSeleccionSlot();
        }
    }

    // Actualiza visualmente la selecci�n de slots (por ejemplo, cambiando el color)
    void ActualizarSeleccionSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].color = (i == selectedSlot) ? Color.green : Color.white;
        }
    }

    // Intenta a�adir un item al inventario buscando espacios contiguos libres.
    public bool TryAddItem(item newItem)
    {
        int requiredSlots = newItem.slotsNeeded;

        // Recorre los slots hasta el �ltimo �ndice que permita ubicar los requiredSlots contiguos.
        for (int i = 0; i <= slots.Length - requiredSlots; i++)
        {
            bool canPlace = true;
            // Verifica que cada uno de los slots contiguos est� libre.
            for (int j = 0; j < requiredSlots; j++)
            {
                if (slots[i + j].sprite != null)
                {
                    canPlace = false;
                    break;
                }
            }
            if (canPlace)
            {
                // Asigna el sprite del item a todos los slots requeridos.
                for (int j = 0; j < requiredSlots; j++)
                {
                    slots[i + j].sprite = newItem.sprite;
                }
                return true;
            }
        }
        return false; // No se encontr� espacio suficiente.
    }

    public item GetItemBySprite(Sprite sprite)
    {
        foreach (item item in todosLosItems) // Crea un array p�blico con todos tus Items
        {
            if (item.sprite == sprite)
                return item;
        }
        return null;
    }

    public void BorrarItem(int selectedIndex, Sprite selectedSprite)
    {
        slots[selectedIndex].sprite = null;

        // Limpia slot adyacente si el objeto ocupa dos
        if (selectedIndex > 0 && slots[selectedIndex - 1].sprite == selectedSprite)
            slots[selectedIndex - 1].sprite = null;

        if (selectedIndex < slots.Length - 1 && slots[selectedIndex + 1].sprite == selectedSprite)
            slots[selectedIndex + 1].sprite = null;
    }


}
