using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestTiendaManager : MonoBehaviour
{
    //[Header("Slots de Mejora")]
    public SpriteRenderer mejoraSlot1;
    public SpriteRenderer mejoraSlot2;

    //[Header("Sprites de Mejora")]
    public Sprite[] mejorasSprites; // Debe coincidir el orden con MejoraType

    //[Header("Textos de Botones")]
    public TextMeshProUGUI textoBotonSlot1;
    public TextMeshProUGUI textoBotonSlot2;

    //[Header("Precio de cada mejora")]
    public int precioMejora = 100;

    // Índices (MejoraType) que estamos mostrando
    private int mejoraMostradaSlot1 = -1;
    private int mejoraMostradaSlot2 = -1;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TestTienda")
            MostrarMejorasAleatorias();
    }

    void Start()
    {
        // En caso de que la escena ya esté cargada al arrancar
        if (SceneManager.GetActiveScene().name == "TestTienda")
            MostrarMejorasAleatorias();
    }

    /// <summary>
    /// Elige dos mejoras aleatorias que aún NO estén activadas en MejorasManager.
    /// </summary>
    public void MostrarMejorasAleatorias()
    {
        // 1) Recopilar los tipos de mejora NO activados
        List<MejoraType> disponibles = new List<MejoraType>();
        var mgr = MejorasManager.Instance;
        if (!mgr.dineroTripleActivado) disponibles.Add(MejoraType.DineroTriple);
        if (!mgr.coctelesDoblesActivado) disponibles.Add(MejoraType.CoctelesDobles);
        if (!mgr.clientesExtraActivado) disponibles.Add(MejoraType.ClientesExtra);
        if (!mgr.preparacionRapidaActivado) disponibles.Add(MejoraType.PreparacionRapida);

        // 2) Si no hay ninguna disponible, ocultamos ambos slots
        if (disponibles.Count == 0)
        {
            SetSlot(mejoraSlot1, textoBotonSlot1, -1);
            SetSlot(mejoraSlot2, textoBotonSlot2, -1);
            return;
        }

        // 3) Slot 1
        int idx = Random.Range(0, disponibles.Count);
        mejoraMostradaSlot1 = (int)disponibles[idx];
        disponibles.RemoveAt(idx);
        SetSlot(mejoraSlot1, textoBotonSlot1, mejoraMostradaSlot1);

        // 4) Slot 2 (si queda)
        if (disponibles.Count > 0)
        {
            idx = Random.Range(0, disponibles.Count);
            mejoraMostradaSlot2 = (int)disponibles[idx];
            SetSlot(mejoraSlot2, textoBotonSlot2, mejoraMostradaSlot2);
        }
        else
        {
            SetSlot(mejoraSlot2, textoBotonSlot2, -1);
        }
    }

    /// <summary>
    /// Configura visualmente un slot: sprite + botón.
    /// </summary>
    private void SetSlot(SpriteRenderer slotRenderer, TextMeshProUGUI slotText, int mejoraIndex)
    {
        if (mejoraIndex < 0)
        {
            slotRenderer.enabled = false;
            slotText.text = "";
        }
        else
        {
            slotRenderer.sprite = mejorasSprites[mejoraIndex];
            slotRenderer.enabled = true;
            slotText.text = "Comprar";
        }
    }

    /// <summary>
    /// Se llama al pulsar el botón Comprar del slot 1.
    /// </summary>
    public void ComprarMejoraSlot1()
    {
        ComprarEnSlot(mejoraMostradaSlot1, mejoraSlot1, textoBotonSlot1);
    }

    /// <summary>
    /// Se llama al pulsar el botón Comprar del slot 2.
    /// </summary>
    public void ComprarMejoraSlot2()
    {
        ComprarEnSlot(mejoraMostradaSlot2, mejoraSlot2, textoBotonSlot2);
    }

    /// <summary>
    /// Lógica común de compra: comprueba dinero, activa la mejora, descuenta, actualiza UI.
    /// </summary>
    private void ComprarEnSlot(int mejoraIndex, SpriteRenderer slotRenderer, TextMeshProUGUI slotText)
    {
        if (mejoraIndex < 0) return;

        var money = MoneyManager.Instance.Ganancias;
        if (money < precioMejora)
        {
            Debug.Log("No tienes suficiente dinero para comprar esa mejora.");
            return;
        }

        // 1) Descontar el dinero
        MoneyManager.Instance.Comprar(precioMejora);

        // 2) Activar la mejora en MejorasManager
        MejorasManager.Instance.ActivarMejora((MejoraType)mejoraIndex);

        // 3) Actualizar visualmente el slot: marcar como comprado
        slotRenderer.enabled = false;
        slotText.text = "Comprado";

        Debug.Log($"Mejora {(MejoraType)mejoraIndex} comprada satisfactoriamente.");
    }

    public void OnSiguienteButtonClick()
    {
        TestGameManager.Instance.LoadNextNight();

    }
}
