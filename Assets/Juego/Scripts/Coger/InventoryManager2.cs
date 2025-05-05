using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager2 : MonoBehaviour
{

    public List<Image> slots;

    public int selectedSlot = 0;

    public item[] todosLosItems;     // Array con todos los ScriptableObjects item

    // Número real de slots activos en el inventario
    private int activeSlotsCount;

    void Start()
    {
        // Comprueba si la mejora de slot extra está activa
        bool extra = UpgradeData.inventorySlotExtraActivado;

        // Slot extra está en la posición 3 del List (0-based).
        if (slots.Count < 4)
        {
            Debug.LogError("Debes tener 4 elementos en la lista 'slots': 3 base + slot extra.");
            activeSlotsCount = Mathf.Min(slots.Count, 3);
        }
        else
        {
            // Habilita o deshabilita el slot extra visualmente
            slots[3].enabled = extra;
            activeSlotsCount = extra ? 4 : 3;
        }

        // Ajusta selectedSlot dentro de los slots activos
        selectedSlot = Mathf.Clamp(selectedSlot, 0, activeSlotsCount - 1);

        ActualizarSeleccionSlot();
    }

    void Update()
    {
        // Selección con rueda del ratón, solo sobre los slots activos
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            selectedSlot = (selectedSlot + 1) % activeSlotsCount;
            ActualizarSeleccionSlot();
        }
        else if (scroll < 0f)
        {
            selectedSlot = (selectedSlot - 1 + activeSlotsCount) % activeSlotsCount;
            ActualizarSeleccionSlot();
        }
    }

    /// <summary>
    /// Llamar desde OnClick de cada slot (configura en el Inspector un Button que pase su índice).
    /// Solo permite seleccionar dentro de [0..activeSlotsCount-1].
    /// </summary>
    public void SeleccionarSlot(int index)
    {
        if (index >= 0 && index < activeSlotsCount)
        {
            selectedSlot = index;
            ActualizarSeleccionSlot();
        }
    }

    /// <summary>
    /// Refresca el color de los slots para indicar cuál está seleccionado.
    /// </summary>
    private void ActualizarSeleccionSlot()
    {
        for (int i = 0; i < activeSlotsCount; i++)
            slots[i].color = (i == selectedSlot) ? Color.green : Color.white;
    }

    /// <summary>
    /// Intenta añadir un ítem al primer espacio contiguo libre dentro de los slots activos.
    /// </summary>
    public bool TryAddItem(item newItem)
    {
        int needed = newItem.slotsNeeded;

        // Recorre hasta el último índice donde cupiesen needed slots
        for (int i = 0; i <= activeSlotsCount - needed; i++)
        {
            bool free = true;
            for (int j = 0; j < needed; j++)
            {
                if (slots[i + j].sprite != null)
                {
                    free = false;
                    break;
                }
            }
            if (!free) continue;

            // Coloca el sprite en los slots libres encontrados
            for (int j = 0; j < needed; j++)
                slots[i + j].sprite = newItem.sprite;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Obtiene el objeto item asociado a un sprite, o null si no existe.
    /// </summary>
    public item GetItemBySprite(Sprite sprite)
    {
        foreach (var it in todosLosItems)
            if (it.sprite == sprite)
                return it;
        return null;
    }

    /// <summary>
    /// Borra un ítem del slot dado y limpia adyacentes que coincidan (para items de slotsNeeded=2).
    /// </summary>
    public void BorrarItem(int index, Sprite selectedSprite)
    {
        if (index < 0 || index >= activeSlotsCount) return;

        slots[index].sprite = null;
        if (index > 0 && slots[index - 1].sprite == selectedSprite)
            slots[index - 1].sprite = null;
        if (index < activeSlotsCount - 1 && slots[index + 1].sprite == selectedSprite)
            slots[index + 1].sprite = null;
    }
}
