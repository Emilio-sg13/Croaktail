using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using System.Collections.Generic;

/// <summary>
/// Gestiona el flujo del tutorial: muestra pasos de UI, instancia al cliente,
/// asigna un cóctel específico, espera a que recorra su path y procesa cada etapa.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("UI Tutorial")]
    public GameObject tutorialPanel;        // Panel que contiene la UI del tutorial
    public Image tutorialImage;             // Imagen de cada paso
    public TextMeshProUGUI tutorialText;    // Texto de cada paso
    public Button tutorialButton;           // Botón final para continuar

    [Header("Cliente de Tutorial")]
    public GameObject tutorialClientPrefab; // Prefab del cliente
    public ClientType tutorialClientType;   // Tipo de cliente (configura ancho, speed, etc.)
    public Transform tutorialSpawnPoint;    // Punto donde aparece el cliente
    public item specificCoctelTutorial;    // Cóctel específico que el cliente pedirá

    [Header("Efecto Chispa")]
    public ParticleSystem sparkEffectPrefab; // Partículas para resaltar hueco
    [Tooltip("Asignar múltiples HuecoMezclador donde aparecerán las chispas")]
    public List<Transform> huecoMezcladores;

    [Header("Recursos del Tutorial")]
    public Sprite[] stepImages;             // Sprites de cada paso (0-4)
    [TextArea]
    public string[] stepTexts;              // Textos de cada paso

    private MovimientoClientesMultiple tutorialClient;

    void Start()
    {
        tutorialButton.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        // Paso 0: Introducción
        ShowStep(0);
        yield return new WaitForSeconds(5f);

        // Paso 1: Aparece el cliente y se configura su movimiento
        ShowStep(1);
        if (PathManager.Instance == null)
        {
            Debug.LogError("PathManager no encontrado.");
            yield break;
        }
        int[] paths = PathManager.Instance.FindAvailableAdjacentPaths(tutorialClientType.width);
        if (paths == null)
        {
            Debug.LogError("No hay caminos disponibles para tutorial.");
            yield break;
        }
        GameObject go = Instantiate(tutorialClientPrefab, tutorialSpawnPoint.position, tutorialSpawnPoint.rotation);
        tutorialClient = go.GetComponent<MovimientoClientesMultiple>();
        if (tutorialClient == null)
        {
            Debug.LogError("MovimientoClientesMultiple no encontrado en prefab.");
            yield break;
        }
        tutorialClient.SetClientType(tutorialClientType);
        tutorialClient.SetClientWidth(tutorialClientType.width);
        tutorialClient.SetSpeed(tutorialClientType.speed);
        tutorialClient.SetPathIndices(paths);
        //Asignar referencias al script InteractuableConInventario del cliente
        InteractuableConInventario interact = tutorialClient.GetComponent<InteractuableConInventario>();
        if (interact != null)
        {
            // Buscar el inventario y el jugador dentro de la escena
            interact.inventario = FindFirstObjectByType<InventoryManager2>(); // o InventoryManager2, según tu script real
            interact.player = GameObject.FindWithTag("Player")?.transform;
        }
        // Ocupar inicio del path
        PathManager.Instance.OccupyMultipleHorizontal(paths, 0, tutorialClientType.width, go);

        // Paso 2: Esperar a que el cliente llegue al último punto
        while (!tutorialClient.hasReachedEnd)
            yield return null;
        ShowStep(2);
        // Mostrar el cóctel específico y chispa
        tutorialClient.ShowSpecificCoctel(specificCoctelTutorial);
        if (sparkEffectPrefab != null && huecoMezcladores != null)
        {
            foreach (var t in huecoMezcladores)
            {
                Instantiate(sparkEffectPrefab, t.position, Quaternion.identity);
            }
        }

        // Paso 3: Esperar a que el jugador recoja el cóctel en el inventario
        InventoryManager2 inv = FindFirstObjectByType<InventoryManager2>();
        if (inv == null)
        {
            Debug.LogError("InventoryManager2 no encontrado en la escena.");
        }
        else
        {
            while (true)
            {
                foreach (var slotImg in inv.slots)
                {
                    if (slotImg.sprite == specificCoctelTutorial.sprite)
                        goto CoctelRecogido;
                }
                yield return null;
            }

        CoctelRecogido:;
        }
        ShowStep(3);

        // Paso 4: Esperar a que el cliente desaparezca
        while (tutorialClient != null)
            yield return null;
        ShowStep(4);

        yield return new WaitForSeconds(7f);
        ShowStep(5);
        // Habilitar botón final
        tutorialButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Actualiza la UI con la imagen y texto del paso dado.
    /// </summary>
    void ShowStep(int index)
    {
        if (index < 0 || index >= stepImages.Length || index >= stepTexts.Length)
            return;
        tutorialImage.sprite = stepImages[index];
        tutorialText.text = stepTexts[index];
    }

    /// <summary>
    /// Llamar desde el OnClick("Continuar") al final del tutorial.
    /// </summary>
    public void OnTutorialComplete()
    {
        tutorialPanel.SetActive(false);
        GameManager.Instance.CompleteTutorial();
    }
}
