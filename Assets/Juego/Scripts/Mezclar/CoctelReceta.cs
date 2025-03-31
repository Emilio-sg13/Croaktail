using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CoctelReceta
{
    public List<item> ingredientes;
    public item resultado;

    public bool MismaCombinacion(List<item> input)
    {
        if (input.Count != ingredientes.Count) return false;

        // Compara si contienen los mismos ingredientes sin importar el orden
        var recetaSet = new HashSet<string>(ingredientes.Select(i => i.itemName));
        var inputSet = new HashSet<string>(input.Select(i => i.itemName));

        return recetaSet.SetEquals(inputSet);
    }
}
