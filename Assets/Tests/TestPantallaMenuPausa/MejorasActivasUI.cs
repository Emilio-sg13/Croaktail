using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MejorasActivasUI : MonoBehaviour
{
    public static MejorasActivasUI Instance;


    // --- Variables de configuración ---
    // Configuracion de efecto fade
    public float fadeDuration = 1f;
    public float textoVisibleTime = 3f;

    // Sprites asignables de mejoras
    public Sprite spriteMasClientes;
    public Sprite spriteDineroTriple;
    public Sprite spriteCoctelesDobles;
    public Sprite spriteMezcladoRapido;
    public Sprite spriteInventorySlotExtra;

    // GameObjects que hay en escena donde se mostrarán las mejoras
    public GameObject CasillaMejora1;
    public GameObject CasillaMejora2;

    // Mejoras activas en texto y sprite
    private System.Collections.Generic.List<string> mejorasActivasTexto = new System.Collections.Generic.List<string>();
    private System.Collections.Generic.List<Sprite> mejorasActivasSprites = new System.Collections.Generic.List<Sprite>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MostrarMejorasActivas();
    }

    public void MostrarMejorasActivas()
    {
        
        mejorasActivasSprites.Clear();

        // Se comprueba cada mejora y se agrega texto y sprite correspondiente si está activada
        if (UpgradeData.masClientes)
        {
            ;
            mejorasActivasSprites.Add(spriteMasClientes);
        }
        if (UpgradeData.dineroTriple)
        {
            
            mejorasActivasSprites.Add(spriteDineroTriple);
        }
        if (UpgradeData.coctelesDobles)
        {
            
            mejorasActivasSprites.Add(spriteCoctelesDobles);
        }
        if (UpgradeData.mezcladoRapido)
        {
            
            mejorasActivasSprites.Add(spriteMezcladoRapido);
        }
        if (UpgradeData.inventorySlotExtraActivado)
        {
            
            mejorasActivasSprites.Add(spriteInventorySlotExtra);
        }

        

        // Reemplaza las casillas por las mejoras correspondientes
        ReemplazarCasilla(CasillaMejora1, mejorasActivasSprites.Count > 0 ? mejorasActivasSprites[0] : null);
        ReemplazarCasilla(CasillaMejora2, mejorasActivasSprites.Count > 1 ? mejorasActivasSprites[1] : null);
    }

    // Método que reemplaza un GameObject (casilla) por un sprite que se instancia dentro de ella
    void ReemplazarCasilla(GameObject casilla, Sprite sprite)
    {
        foreach (Transform child in casilla.transform)
        {
            Destroy(child.gameObject);
        }

        if (sprite == null) return;

        GameObject nuevaMejora = new GameObject("MejoraSprite");
        nuevaMejora.transform.SetParent(casilla.transform);
        nuevaMejora.transform.localPosition = Vector3.zero;
        nuevaMejora.transform.localScale = Vector3.one;

        Image img = nuevaMejora.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = true;

        RectTransform rtCasilla = casilla.GetComponent<RectTransform>();
        RectTransform rtNueva = nuevaMejora.GetComponent<RectTransform>();
        if (rtCasilla != null && rtNueva != null)
        {
            rtNueva.anchorMin = Vector2.zero;
            rtNueva.anchorMax = Vector2.one;
            rtNueva.offsetMin = Vector2.zero;
            rtNueva.offsetMax = Vector2.zero;
        }
    }

    System.Collections.IEnumerator FadeOutTexto(TextMeshProUGUI texto)
    {
        yield return new WaitForSeconds(textoVisibleTime);

        float elapsed = 0f;
        Color originalColor = texto.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            texto.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        texto.text = " ";
        texto.color = originalColor; 
    }
}
