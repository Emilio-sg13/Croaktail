using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager2 : MonoBehaviour
{
    // Asigna en el Inspector las imágenes correspondientes a los 3 slots.
    public List<Image> slots = new List<Image>();

    // Índice del slot actualmente seleccionado
    public int selectedSlot = 0;

    public item[] todosLosItems;

    public GameObject slotPrefab;    // un prefab con una Image


    void Start()
    {

        // Si la mejora de slot extra está activa, añade un slot extra
        if (MejorasManager.Instance != null && MejorasManager.Instance.inventorySlotExtraActivado)
        {
            AñadirSlotExtra();
        }

        ActualizarSeleccionSlot();
    }

    void Update()
    {
        // Selección mediante la rueda del ratón
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            selectedSlot = (selectedSlot + 1) % slots.Count;
            ActualizarSeleccionSlot();
        }
        else if (scroll < 0)
        {
            selectedSlot = (selectedSlot - 1 + slots.Count) % slots.Count;
            ActualizarSeleccionSlot();
        }
    }

    // Método para seleccionar un slot haciendo clic en el mismo (enlázalo al OnClick del botón o imagen)
    public void SeleccionarSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            selectedSlot = index;
            ActualizarSeleccionSlot();
        }
    }

    // Actualiza visualmente la selección de slots (por ejemplo, cambiando el color)
    void ActualizarSeleccionSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].color = (i == selectedSlot) ? Color.green : Color.white;
        }
    }

    // Intenta añadir un item al inventario buscando espacios contiguos libres.
    public bool TryAddItem(item newItem)
    {
        int requiredSlots = newItem.slotsNeeded;

        // Recorre los slots hasta el último índice que permita ubicar los requiredSlots contiguos.
        for (int i = 0; i <= slots.Count - requiredSlots; i++)
        {
            bool canPlace = true;
            // Verifica que cada uno de los slots contiguos esté libre.
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
        return false; // No se encontró espacio suficiente.
    }

    public item GetItemBySprite(Sprite sprite)
    {
        foreach (item item in todosLosItems) // Crea un array público con todos tus Items
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

        if (selectedIndex < slots.Count - 1 && slots[selectedIndex + 1].sprite == selectedSprite)
            slots[selectedIndex + 1].sprite = null;
    }

    void AñadirSlotExtra()
    {
        // Instancia un nuevo UI Slot
        GameObject go = Instantiate(slotPrefab, slotsParentTransform);
        Image img = go.GetComponent<Image>();
        slots.Add(img);

        // Opcional: Ajusta selectedSlot si era out-of-range
        if (selectedSlot >= slots.Count)
            selectedSlot = slots.Count - 1;
    }


}
