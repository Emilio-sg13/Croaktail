using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour
{
    // Referencia al gestor del inventario y al jugador (cápsula)
    public InventoryManager2 inventario;
    public PlayerController jugador;
    // Distancia mínima para considerar que el jugador "ha llegado" al objeto
    public float distanciaRecogida = 0.5f;

    // Referencia al ScriptableObject que define este item (configurable para cada objeto)
    public item itemData;

    private bool recogido = false;



    // Al hacer clic sobre el objeto (asegúrate de que el collider esté configurado y que haya un Physics Raycaster en la cámara)
    void OnMouseDown()
    {
        if (recogido) return;

        // Mover al jugador hacia este objeto
        jugador.MoverHacia(transform.position);
        // Inicia la rutina para comprobar si el jugador ha llegado y recoger el objeto
        StartCoroutine(ComprobarRecogida());
    }

    IEnumerator ComprobarRecogida()
    {
        // Espera hasta que el jugador esté lo suficientemente cerca
        while (Vector3.Distance(jugador.transform.position, transform.position) > distanciaRecogida)
        {
            yield return null;
        }
        // Intenta añadir el objeto al inventario usando la información del ScriptableObject
        bool añadido = inventario.TryAddItem(itemData);
        if (añadido)
        {
            // Se puede agregar una animación o efecto extra al recoger el objeto
            recogido = true;
            //Destroy(gameObject);
        }
    }
}
