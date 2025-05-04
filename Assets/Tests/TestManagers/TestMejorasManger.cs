using UnityEngine;

public class TestMejorasManager : MonoBehaviour
{
    //[Tooltip("Tipo de mejora que activa este botón")]
    public MejoraType mejoraType;
    //[Tooltip("Sprite que se mostrará cuando la mejora esté activa")]
    public Sprite spriteAsignado;

    /// <summary>
    /// Conéctalo al OnClick() del botón de tag "comprar"
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
            Debug.LogWarning("No se encontró ningún objeto con tag 'MostrarMejora'.");
        }
    }
}
