using UnityEngine;

public enum MejoraType
{
    DineroTriple,
    CoctelesDobles,
    ClientesExtra,
    PreparacionRapida
}

public class MejorasManager : MonoBehaviour
{
    public static MejorasManager Instance { get; private set; }


    public bool dineroTripleActivado = false;
    public bool coctelesDoblesActivado = false;
    public bool clientesExtraActivado = false;
    public bool preparacionRapidaActivado = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activa la mejora indicada por tipo.
    /// </summary>
    public void ActivarMejora(MejoraType type)
    {
        switch (type)
        {
            case MejoraType.DineroTriple:
                dineroTripleActivado = true;
                break;
            case MejoraType.CoctelesDobles:
                coctelesDoblesActivado = true;
                break;
            case MejoraType.ClientesExtra:
                clientesExtraActivado = true;
                break;
            case MejoraType.PreparacionRapida:
                preparacionRapidaActivado = true;
                break;
        }
        Debug.Log($"Mejora activada: {type}");
    }

    // Métodos de ejemplo para consumir esas flags en tu lógica:
    public int AplicarDineroTriple(int valor) =>
        dineroTripleActivado ? valor * 3 : valor;

    public int AplicarCoctelesDobles(int cantidad) =>
        coctelesDoblesActivado ? cantidad * 2 : cantidad;

    public float AplicarClientesExtra(float spawnInterval) =>
        clientesExtraActivado ? spawnInterval / 2f : spawnInterval;

    public int AplicarPreparacionRapida(int clicsNecesarios) =>
        preparacionRapidaActivado ? Mathf.CeilToInt(clicsNecesarios / 2f) : clicsNecesarios;
}

