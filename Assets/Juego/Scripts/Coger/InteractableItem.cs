using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour
{
    // Referencia al gestor del inventario y al jugador (c�psula)
    public InventoryManager2 inventario;
    public PlayerController jugador;
    // Distancia m�nima para considerar que el jugador "ha llegado" al objeto
    public float distanciaRecogida = 0.5f;

    // Referencia al ScriptableObject que define este item (configurable para cada objeto)
    public item itemData;

    private bool recogido = false;



    // Al hacer clic sobre el objeto (aseg�rate de que el collider est� configurado y que haya un Physics Raycaster en la c�mara)
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
        // Espera hasta que el jugador est� lo suficientemente cerca
        while (Vector3.Distance(jugador.transform.position, transform.position) > distanciaRecogida)
        {
            yield return null;
        }
        // Intenta a�adir el objeto al inventario usando la informaci�n del ScriptableObject
        bool a�adido = inventario.TryAddItem(itemData);
        if (a�adido)
        {
            // Se puede agregar una animaci�n o efecto extra al recoger el objeto
            recogido = true;
            //Destroy(gameObject);
        }
    }
}
