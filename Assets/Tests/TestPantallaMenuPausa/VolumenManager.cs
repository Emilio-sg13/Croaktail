using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class VolumenManager : MonoBehaviour
{

    [SerializeField] Slider sliderVolumen;
    void Start()   
    {
        if (!PlayerPrefs.HasKey("volumenMusica"))
        {
            PlayerPrefs.SetFloat("volumenMusica", 1);
            Load();
        }
    }

    public void CambiarVolumen()
    {
        AudioListener.volume = sliderVolumen.value;
        Debug.Log("Nivel volumen: " + sliderVolumen.value);
        Save();
    }

    private void Load()
    {
        sliderVolumen.value = PlayerPrefs.GetFloat("volumenMusica");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("volumenMusica", sliderVolumen.value);
    }
}
