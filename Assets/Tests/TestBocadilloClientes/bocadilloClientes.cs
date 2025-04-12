using UnityEngine;
using TMPro;

public class bocadilloClientes : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fondoBocadillo;
    [SerializeField] private TextMeshPro textoBocadillo;

    private void Awake()
    {
        // Asegurarse de que los componentes estén asignados correctamente
        if (fondoBocadillo == null)
            fondoBocadillo = transform.Find("Fondo").GetComponent<SpriteRenderer>();  // Asignación manual si no está asignado
        if (textoBocadillo == null)
            textoBocadillo = transform.Find("Texto").GetComponent<TextMeshPro>();  // Asignación manual si no está asignado
    }

    public void Setup(string mensaje)
    {
        textoBocadillo.text = mensaje;

        textoBocadillo.ForceMeshUpdate();
        Vector2 textSize = textoBocadillo.GetRenderedValues(false);
        fondoBocadillo.size = textSize + new Vector2(1f, 0.5f);  // Ajustar el fondo al tamaño del texto
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
