using UnityEngine;

public enum TipoItem
{
    Ingrediente,
    Coctel
}

[CreateAssetMenu(fileName = "NuevoItem", menuName = "Inventario/Item")]
public class item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int slotsNeeded = 1;
    public int precio = 10;
    public TipoItem tipo;
}
