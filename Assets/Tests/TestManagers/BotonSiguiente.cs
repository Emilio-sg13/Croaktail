using UnityEngine;


public class BotonSiguiente : MonoBehaviour
{

    public void OnSiguienteButtonClick()
    {
        GameManager.Instance.LoadNextNight();
        
    }
}
