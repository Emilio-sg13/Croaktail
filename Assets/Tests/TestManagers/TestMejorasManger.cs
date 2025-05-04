using UnityEngine;

public class TestMejorasManager : MonoBehaviour
{
    //[Tooltip("Tipo de mejora que activa este bot�n")]
    public MejoraType mejoraType;
    //[Tooltip("Sprite que se mostrar� cuando la mejora est� activa")]
    public Sprite spriteAsignado;

    /// <summary>
    /// Con�ctalo al OnClick() del bot�n de tag "comprar"
    /// </summary>
    public void OnComprarButtonClick()
    {
        // 1. Activa la mejora en el manager
        MejorasManager.Instance.ActivarMejora(mejoraType);

        // 2. Busca el objeto con tag "MostrarMejora" y actualiza su sprite
        GameObject target = GameObject.FindWithTag("MostrarMejora");
        if (target != null)
        {
            SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = spriteAsignado;
                sr.enabled = true;
            }
            else
            {
                Debug.LogWarning("MostrarMejora no tiene SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontr� ning�n objeto con tag 'MostrarMejora'.");
        }
    }
}
