using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mezclador : MonoBehaviour
{
    // Lista de huecos donde se depositan los ingredientes
    public List<HuecoBarra> huecosIngredientes;
    // Transform del jugador para moverlo hacia el mezclador
    public Transform player;
    // Distancia máxima para iniciar la acción de mezcla
    public float distanciaMaxima = 2f;
    // Referencia al inventario (por ejemplo, InventoryManager2)
    public InventoryManager2 inventario;
    // Referencia a la UI de mezcla que contiene el slider y el texto
    public MezclaUI mezclaUI;
    // Objeto donde se mostrará el cóctel si el inventario está lleno (por ejemplo, barra de salida)
    public GameObject barraSalida;
    // Lista de recetas para determinar el cóctel resultante
    public List<CoctelReceta> recetas;

    // 🔥 NUEVO: Referencia al efecto de chispas (drag desde el inspector)
    public ParticleSystem efectoChispas;

    // 🔥 NUEVO: Posición en la UI donde deben aparecer las chispas
    public Transform puntoChispaUI;
    // Bandera para controlar que el proceso de mezcla no se dispare varias veces simult醤eamente
    private bool mezclando = false;

    /// <summary>
    /// Al hacer clic (botton izquierdo) en el mezclador, se ordena que el jugador se mueva hacia 閘 y se inicia la rutina.
    /// </summary>
    void OnMouseDown()
    {
        // Evita la accion si faltan referencias o ya se est?mezclando
        if (player == null || inventario == null || mezclando)
            return;

        // Ordena al PlayerController mover al jugador hacia la posici髇 del mezclador
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.MoverHacia(transform.position);
        }

        // Inicia la coroutine que espera a que el jugador est?en rango para proceder
        StartCoroutine(EsperarYMezclar());
    }

    /// <summary>
    /// Coroutine que espera hasta que el jugador est?dentro de la distancia permitida.
    /// Si la UI de mezcla (slider) no est?activa, inicia el proceso de mezcla.
    /// Si ya est?activa, no hace nada (ahora se capturar醤 clics con el botton derecho en Update).
    /// </summary>
    IEnumerator EsperarYMezclar()
    {
        // Espera hasta que la distancia entre el jugador y el mezclador sea menor o igual a la requerida
        while (Vector3.Distance(player.position, transform.position) > distanciaMaxima)
        {
            yield return null;
        }

        // Si la UI de mezcla NO est?activa, inicia el proceso de mezcla
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
                // Al completarse la mezcla, se obtiene el cócteles resultante a partir de los ingredientes
                item coctel = ObtenerCoctel(ingredientes);
                if (coctel != null)
                {
                    // Si se activa la actualizacion doble, se intenta anadir dos cócteles
                    if (UpgradeData.coctelesDobles)
                    {
                        bool addedPrimer = inventario.TryAddItem(coctel);
                        bool addedSegundo = inventario.TryAddItem(coctel);

                        if (addedPrimer && addedSegundo)
                        {
                            Debug.Log("Dos cócteles añadidos al inventario.");
                        }
                        else if (addedPrimer && !addedSegundo)
                        {
                            Debug.Log("Primer cóctel añadido, pero el inventario estaba lleno para el segundo cóctel.");
                            // Se muestra el segundo cócteles en la barra de salida
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Segundo cóctel depositado en la barra.");
                        }
                        else if (!addedPrimer)
                        {
                            // Si ni el primero ni el segundo se pueden agregar, se deposita al menos uno en la barra de salida
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Inventario lleno. C骳teles depositados en la barra.");
                        }
                    }
                    else
                    {
                        // Flujo original: se anade un solo cócteles
                        bool added = inventario.TryAddItem(coctel);
                        if (added)
                        {
                            Debug.Log("cóctele anadido al inventario.");
                        }
                        else
                        {
                            SpriteRenderer sr = barraSalida.GetComponent<SpriteRenderer>();
                            sr.sprite = coctel.sprite;
                            Debug.Log("Inventario lleno. cócteles depositado en la barra.");
                        }
                    }

                    // ✅ NUEVO：Reproducir efecto de chispas en la UI
                    if (efectoChispas != null && puntoChispaUI != null)
                    {
                        Instantiate(efectoChispas, puntoChispaUI.position, Quaternion.identity);
                    }
                }
                else
                {
                    Debug.Log("No se ha creado ningún cóctel.");
                }
                mezclando = false; // Finaliza el proceso de mezcla
            });

            mezclando = true; // Marca que se inici?la mezcla
        }
        // Si la UI de mezcla ya est?activa, se deja que el Update capture el clic derecho
        yield break;
    }


    /// <summary>
    /// En Update se captura el clic derecho para incrementar el slider de mezcla
    /// siempre que el proceso de mezcla est?activo y la UI de mezcla se encuentre visible.
    /// </summary>
    void Update()
    {
        if (mezclando && mezclaUI.gameObject.activeSelf && Input.GetMouseButtonDown(1))
        {
            mezclaUI.ClicMezclar();

            if (efectoChispas != null)
            {
                efectoChispas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                efectoChispas.Play();
                Debug.Log("🔥 Chispa reproducida al hacer clic derecho");
            }
        }
    }



    /// <summary>
    /// Recorre la lista de recetas y devuelve el c骳tel resultante que coincida con la combinaci髇 de ingredientes.
    /// Si no se encuentra ninguna coincidencia, devuelve null.
    /// </summary>
    /// <param name="ingredientes">Lista de ingredientes depositados.</param>
    /// <returns>El cóctele resultante o null.</returns>
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
