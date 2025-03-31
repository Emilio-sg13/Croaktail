using UnityEngine;
using System.Collections.Generic;

public class Mezclador : MonoBehaviour
{
    public InventoryManager2 inventario;
    public List<HuecoBarra> huecosIngredientes;
    public Transform player;
    public float distanciaMaxima = 2f;

    public MezclaUI mezclaUI; // Referencia a la UI que se muestra al mezclar
    public GameObject barraObj; // Donde se colocará el cóctel
    public List<CoctelReceta> recetas;

    private bool mezclando = false;

    void OnMouseDown()
    {
        if (mezclando) return;
        if (Vector3.Distance(player.position, transform.position) > distanciaMaxima) return;

        List<item> ingredientes = new List<item>();

        foreach (var hueco in huecosIngredientes)
        {
            Sprite sprite = hueco.GetSprite();
            if (sprite != null)
            {
                item ingrediente = inventario.GetItemBySprite(sprite);
                if (ingrediente != null)
                    ingredientes.Add(ingrediente);
            }
        }


        if (ingredientes.Count == 0)
        {
            Debug.Log("No hay ingredientes.");
            return;
        }

        // Vaciar los huecos
        foreach (var h in huecosIngredientes) h.Vaciar();

        // Activar la UI de mezcla
        mezclaUI.IniciarProgreso(() =>
        {
            // Cuando la barra se llene...
            item coctelResultante = ObtenerCoctelDesdeIngredientes(ingredientes);
            if (coctelResultante != null)
            {
                // Colocar en la barra
                SpriteRenderer rend = barraObj.GetComponent<SpriteRenderer>();
                rend.sprite = coctelResultante.sprite;
            }
            else
            {
                Debug.Log("No se ha podido crear un cóctel válido.");
            }

            mezclando = false;
        });

        mezclando = true;
    }

    item ObtenerCoctelDesdeIngredientes(List<item> ingredientes)
    {
        foreach (var receta in recetas)
        {
            if (receta.MismaCombinacion(ingredientes))
                return receta.resultado;
        }
        return null;
    }
}
