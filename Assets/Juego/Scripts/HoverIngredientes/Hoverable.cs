using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hoverable : MonoBehaviour
{
    [Tooltip("Texto que aparecerá al pasar el ratón. Si lo dejas vacío, se usará el nombre del GameObject.")]
    public string mensajePersonalizado;

    void OnMouseEnter()
    {
        string texto = string.IsNullOrEmpty(mensajePersonalizado) ? name : mensajePersonalizado;
        HoverTextManager.Instance?.Show(texto);
    }

    void OnMouseExit()
    {
        HoverTextManager.Instance?.Hide();
    }
}
