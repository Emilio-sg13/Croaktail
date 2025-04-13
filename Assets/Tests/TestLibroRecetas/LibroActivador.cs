using UnityEngine;

public class LibroActivador : MonoBehaviour
{
    [SerializeField] private GameObject CanvasLibro;
    [SerializeField] private GameObject LibroFondo;
    [SerializeField] private GameObject BotonAdelante;
    [SerializeField] private GameObject BotonCerrar;




    private void OnMouseDown()
    {
        if(CanvasLibro != null)
        {
            CanvasLibro.SetActive(true);
            LibroFondo.SetActive(true);
            BotonAdelante.SetActive(true);
            BotonCerrar.SetActive(true);
        }
    }
}
