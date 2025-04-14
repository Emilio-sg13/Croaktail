using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mezclador : MonoBehaviour
{
    // Lista de huecos donde se depositan los ingredientes
    public List<HuecoBarra> huecosIngredientes;
    // Transform del jugador para moverlo hacia el mezclador
    public Transform player;
    // Distancia m�xima para iniciar la acci�n de mezcla
    public float distanciaMaxima = 2f;
    // Referencia al inventario (por ejemplo, InventoryManager2)
    public InventoryManager2 inventario;
    // Referencia a la UI de mezcla que contiene el slider y el texto
    public MezclaUI mezclaUI;
    // Objeto donde se mostrar� el c�ctel si el inventario est� lleno (por ejemplo, barra de salida)
    public GameObject barraSalida;
    // Lista de recetas para determinar el c�ctel resultante
    public List<CoctelReceta> recetas;

    // Bandera para controlar que el proceso de mezcla no se dispare varias veces simult�neamente
    private bool mezclando = false;

    /// <summary>
    /// Al hacer clic (bot�n izquierdo) en el mezclador, se ordena que el jugador se mueva hacia �l y se inicia la rutina.
    /// </summary>
    void OnMouseDown()
    {
        // Evita la acci�n si faltan referencias o ya se est� mezclando
        if (player == null || inventario == null || mezclando)
            return;

        // Ordena al PlayerController mover al jugador hacia la posici�n del mezclador
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.MoverHacia(transform.position);
        }

        // Inicia la coroutine que espera a que el jugador est� en rango para proceder
        StartCoroutine(EsperarYMezclar());
    }

    /// <summary>
    /// Coroutine que espera hasta que el jugador est� dentro de la distancia permitida.
    /// Si la UI de mezcla (slider) no est� activa, inicia el proceso de mezcla.
    /// Si ya est� activa, no hace nada (ahora se capturar�n clics con el bot�n derecho en Update).
    /// </summary>
    IEnumerator EsperarYMezclar()
    {
        // Espera hasta que la distancia entre el jugador y el mezclador sea menor o igual a la requerida
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }

        // Si la UI de mezcla NO est� activa, inicia el proceso de mezcla
        if (!mezclaUI.gameObject.activeSelf)
        {
            // Recopila los ingredientes depositados en los huecos
            List<item> ingredientes = new List<item>();
            foreach (HuecoBarra h in huecosIngredientes)
            {
                Sprite sp = h.GetSprite();
                if (sp != null)
                {
                    item itemIngrediente = inventario.GetItemBySprite(sp);
                    if (itemIngrediente != null)
                        ingredientes.Add(itemIngrediente);
                }
            }

            // Si no hay ingredientes, cancelamos el proceso
            if (ingredientes.Count == 0)
            {
                Debug.Log("No hay ingredientes para mezclar.");
                yield break;
            }

            // Vaciar los huecos para iniciar la mezcla
            foreach (HuecoBarra h in huecosIngredientes)
            {
                h.Vaciar();
            }

            // Inicia el slider de mezcla e indica el callback a ejecutar al completarse
            mezclaUI.IniciarProgreso(() =>
            {
                // Al completarse la mezcla, se obtiene el c�ctel resultante a partir de los ingredientes
                item coctel = ObtenerCoctel(ingredientes);
                if (coctel != null)
                {
                    // Si se activa la actualizaci�n doble, se intenta a�adir dos c�cteles
                    if (UpgradeData.coctelesDobles)
                    {
                        bool addedPrimer = inventario.TryAddItem(coctel);
                        bool addedSegundo = inventario.TryAddItem(coctel);

                        if (addedPrimer && addedSegundo)
                        {
                            Debug.Log("Dos c�cteles a�adidos al inventario.");
                        }
                        else if (addedPrimer && !addedSegundo)
                        {
                            Debug.Log("Primer c�ctel a�adido, pero el inventario estaba lleno para el segundo c�ctel.");
                            // Se muestra el segundo c�ctel en la barra de salida
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Segundo c�ctel depositado en la barra.");
                        }
                        else if (!addedPrimer)
                        {
                            // Si ni el primero ni el segundo se pueden agregar, se deposita al menos uno en la barra de salida
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Inventario lleno. C�cteles depositados en la barra.");
                        }
                    }
                    else
                    {
                        // Flujo original: se a�ade un solo c�ctel
                        bool added = inventario.TryAddItem(coctel);
                        if (added)
                        {
                            Debug.Log("C�ctel a�adido al inventario.");
                        }
                        else
                        {
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Inventario lleno. C�ctel depositado en la barra.");
                        }
                    }
                }
                else
                {
                    Debug.Log("No se ha creado ning�n c�ctel.");
                }
                mezclando = false; // Finaliza el proceso de mezcla
            });


            mezclando = true; // Marca que se inici� la mezcla
        }
        // Si la UI de mezcla ya est� activa, se deja que el Update capture el clic derecho
        yield break;
    }

    /// <summary>
    /// En Update se captura el clic derecho para incrementar el slider de mezcla
    /// siempre que el proceso de mezcla est� activo y la UI de mezcla se encuentre visible.
    /// </summary>
    void Update()
    {
        if (mezclando && mezclaUI.gameObject.activeSelf && Input.GetMouseButtonDown(1))
        {
            // El bot�n derecho (Input.GetMouseButtonDown(1)) incrementa el progreso de la mezcla
            mezclaUI.ClicMezclar();
        }
    }

    /// <summary>
    /// Recorre la lista de recetas y devuelve el c�ctel resultante que coincida con la combinaci�n de ingredientes.
    /// Si no se encuentra ninguna coincidencia, devuelve null.
    /// </summary>
    /// <param name="ingredientes">Lista de ingredientes depositados.</param>
    /// <returns>El c�ctel resultante o null.</returns>
    item ObtenerCoctel(List<item> ingredientes)
    {
        foreach (CoctelReceta receta in recetas)
        {
            if (receta.MismaCombinacion(ingredientes))
                return receta.resultado;
        }
        return null;
    }
}
