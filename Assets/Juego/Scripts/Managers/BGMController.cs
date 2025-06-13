using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioClip bgmNormal;
    public AudioClip bgmRapido;
    public AudioClip campanaClip;

    private AudioSource audioSource;
    private float tiempoTotal = 420f; // 7 minutos = 420 segundos
    private float tiempoRestante;
    private bool rapidoActivado = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tiempoRestante = tiempoTotal;
        audioSource.clip = bgmNormal;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;

        if (!rapidoActivado && tiempoRestante <= 20f)
        {
            CambiarABGMrapido();
        }
    }

    public void SaltarUltimos20Segundos()
    {
        tiempoRestante = 20f;
        CambiarABGMrapido();
    }

    void CambiarABGMrapido()
    {
        rapidoActivado = true;
        audioSource.Stop();
        audioSource.PlayOneShot(campanaClip);
        audioSource.clip = bgmRapido;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PausarMusica()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ReanudarMusica()
    {
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.UnPause();
    }

}
