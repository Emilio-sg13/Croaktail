using UnityEngine;
using System.Collections;
public class InteractableItem : MonoBehaviour
{
    public ParticleSystem efectoChispas;  


    // Referencia al gestor del inventario y al jugador (c醦sula)
    public InventoryManager2 inventario;
    public PlayerController jugador;
    // Distancia m韓ima para considerar que el jugador "ha llegado" al objeto
    public float distanciaRecogida = 0.5f;

    // Referencia al ScriptableObject que define este item (configurable para cada objeto)
    public item itemData;



    // Al hacer clic sobre el objeto (aseg鷕ate de que el collider est?configurado y que haya un Physics Raycaster en la c醡ara)
    void OnMouseDown()
    {

        // Mover al jugador hacia este objeto
        jugador.MoverHacia(transform.position);
        // Inicia la rutina para comprobar si el jugador ha llegado y recoger el objeto
        StartCoroutine(ComprobarRecogida());
    }

    IEnumerator ComprobarRecogida()
    {
        // Espera hasta que el jugador est?lo suficientemente cerca
        while (Vector3.Distance(jugador.transform.position, transform.position) > distanciaRecogida)
        {
            yield return null;
        }
        // Intenta añadido el objeto al inventario usando la informaci髇 del ScriptableObject
        bool añadido = inventario.TryAddItem(itemData);
        if (añadido)
        {
            jugador.AnimacionCoger();
            Debug.Log("Ingrediente recogido.");
            Instantiate(efectoChispas, transform.position, Quaternion.identity);

        }

    }
}
