using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Libro : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> paginas;
    int index = -1;
    bool rotar = false;
    [SerializeField] GameObject botonAtras;
    [SerializeField] GameObject botonAdelante;
    [SerializeField] GameObject botonCerrar;
    public GameObject CanvasLibro;
    public GameObject LibroFondo;


    private void Start()
    {
        InitialState();
        botonCerrar.SetActive(false);
        CanvasLibro.SetActive(false);
        LibroFondo.SetActive(false);
        botonAdelante.SetActive(false);
    }

    public void InitialState()
    {
        index = -1;
        for (int i = 0; i < paginas.Count; i++)
        {
            paginas[i].transform.rotation=Quaternion.identity;
        }
        paginas[0].SetAsLastSibling();
        botonAtras.SetActive(false);
    }

    public void RotarSiguiente()
    {
        if (rotar == true) { return; }
        index++;
        float angulo = 180;
        BotonAdelanteActions();
        paginas[index].SetAsLastSibling();
        StartCoroutine(Rotar(angulo, true));
    }

    public void BotonAdelanteActions()
    {
        if(botonAtras.activeInHierarchy == false)
        {
            botonAtras.SetActive(true);
        }
        if (index==paginas.Count - 1)
        {
            botonAdelante.SetActive(false);
        }
    }

    public void RotarAtras()
    {
        if (rotar == true ) { return; }
        paginas[index].SetAsLastSibling();
        BotonAtrasActions();
        float angulo = 0;
        StartCoroutine(Rotar(angulo, false));
    }

    public void BotonAtrasActions()
    {
        if (botonAdelante.activeInHierarchy == false)
        {
            botonAdelante.SetActive(true);
        }
        if (index -1 < 0)
        {
            botonAtras.SetActive(false);
        }
    }

    IEnumerator Rotar(float angulo, bool adelante)
    {
        float value = 0f;
        while(true)
        {
            rotar = true;
            Quaternion targetRotation = Quaternion.Euler(0, angulo, 0);
            value += Time.deltaTime * pageSpeed;
            paginas[index].rotation = Quaternion.Slerp(paginas[index].rotation, targetRotation, value);
            float angulo1 = Quaternion.Angle(paginas[index].rotation, targetRotation);
            if(angulo1 < 0.1f)
            {
                if (adelante == false)
                {
                    index--;
                }
                rotar = false;
                break;
            }
            yield return null;
        }
    }

    public void CerrarLibro()
    {
        CanvasLibro.SetActive(false);
        LibroFondo.SetActive(false);
        botonAdelante.SetActive(false);
        botonCerrar.SetActive(false);
        botonAtras.SetActive(false);
        InitialState();
    }

}
