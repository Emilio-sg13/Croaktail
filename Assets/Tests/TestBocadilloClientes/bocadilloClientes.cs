using UnityEngine;
using TMPro;

public class bocadilloClientes : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fondoBocadillo;
    [SerializeField] private TextMeshPro textoBocadillo;

    private void Awake()
    {
        // Asegurarse de que los componentes est�n asignados correctamente
        if (fondoBocadillo == null)
            fondoBocadillo = transform.Find("Fondo").GetComponent<SpriteRenderer>();  // Asignaci�n manual si no est� asignado
        if (textoBocadillo == null)
            textoBocadillo = transform.Find("Texto").GetComponent<TextMeshPro>();  // Asignaci�n manual si no est� asignado
    }

    public void Setup(string mensaje)
    {
        textoBocadillo.text = mensaje;

        textoBocadillo.ForceMeshUpdate();
        Vector2 textSize = textoBocadillo.GetRenderedValues(false);
        fondoBocadillo.size = textSize + new Vector2(1f, 0.5f);  // Ajustar el fondo al tama�o del texto
        fondoBocadillo.transform.localPosition = new Vector3(textSize.x / 2, 0, 0);  // Posiciona el fondo correctamente
    }

    public static bocadilloClientes create(Transform parent, Vector3 localPosition, string mensaje)
    {
        GameObject bocadilloPrefab = GameAssetsBocadillo.Instance.BocadilloPrefab;
        GameObject bocadilloObjeto = Instantiate(bocadilloPrefab, parent);

        bocadilloObjeto.transform.localPosition = localPosition;

        bocadilloClientes bocadillo = bocadilloObjeto.GetComponent<bocadilloClientes>();
        bocadillo.Setup(mensaje);

        return bocadillo;
    }
}
