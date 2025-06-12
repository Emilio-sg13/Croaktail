using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //[Tooltip("Velocidad de desplazamiento en unidades por segundo")]
    public float velocidad = 5f;
    //[Tooltip("Distancia mínima al destino para detenerse")]
    public float distanciaDetencion = 0.1f;
    
    [Header("Audio")]
    [Tooltip("AudioSource para reproducir sonidos del personaje")]
    public AudioSource audioSource;
    
    [Tooltip("Sonido de victoria del personaje")]
    public AudioClip victorySoundClip;
    
    [Tooltip("Sonidos adicionales del personaje")]
    public AudioClip[] otherSounds;
    
    private Vector3 destino;
    private bool moviendo = false;
    private CharacterController controller;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("El jugador no tiene un CharacterController asignado.");
            
        // Si no se asignó un AudioSource, intentar encontrar uno en el mismo GameObject
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (!moviendo)
            return;
            
        // Calcula la dirección y la distancia al destino
        Vector3 offset = destino - transform.position;
        float distance = offset.magnitude;
        
        // Si estamos suficientemente cerca, dejamos de movernos
        if (distance < distanciaDetencion)
        {
            moviendo = false;
            return;
        }
        
        // Dirección normalizada hacia el destino
        Vector3 direction = offset.normalized;
        
        // SimpleMove aplica la gravedad internamente y mueve con velocidad (m/s)
        controller.SimpleMove(direction * velocidad);
    }
    
    /// <summary>
    /// Establece un nuevo destino y activa el movimiento.
    /// </summary>
    public void MoverHacia(Vector3 posicionDestino)
    {
        destino = posicionDestino;
        moviendo = true;
    }
    
    /// <summary>
    /// Reproduce un sonido usando el AudioSource del personaje.
    /// </summary>
    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de victoria del personaje.
    /// </summary>
    public void ReproducirSonidoVictoria()
    {
        if (audioSource != null && victorySoundClip != null)
        {
            audioSource.PlayOneShot(victorySoundClip);
        }
    }
}