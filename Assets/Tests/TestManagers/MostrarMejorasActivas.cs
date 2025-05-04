using UnityEngine;
using System.Collections.Generic;

public class MostrarMejorasActivas : MonoBehaviour
{
    //[Tooltip("Order must match MejoraType enum order")]
    public Sprite[] mejoraSprites;       // sprites para cada MejoraType
    public GameObject iconoMejoraPrefab; // prefab con un SpriteRenderer

    void Start()
    {
        // Limpia hijos previos (por si vuelve a entrar a escena)
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        var mgr = MejorasManager.Instance;
        if (mgr == null) return;

        // Recolecta los tipos activos
        var activos = new List<MejoraType>();
        if (mgr.dineroTripleActivado) activos.Add(MejoraType.DineroTriple);
        if (mgr.coctelesDoblesActivado) activos.Add(MejoraType.CoctelesDobles);
        if (mgr.clientesExtraActivado) activos.Add(MejoraType.ClientesExtra);
        if (mgr.preparacionRapidaActivado) activos.Add(MejoraType.PreparacionRapida);

        // Para cada mejora activa, instancia un icono
        foreach (var tipo in activos)
        {
            int idx = (int)tipo;
            if (idx < 0 || idx >= mejoraSprites.Length) continue;

            GameObject go = Instantiate(iconoMejoraPrefab, transform);
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = mejoraSprites[idx];
            else
                Debug.LogWarning("El prefab IconoMejora no tiene SpriteRenderer.");
        }
    }
}
