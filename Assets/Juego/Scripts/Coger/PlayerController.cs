using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Velocidad de desplazamiento en unidades por segundo")]
    public float velocidad = 5f;
    [Tooltip("Distancia mínima al destino para detenerse")]
    public float distanciaDetencion = 0.1f;

    [Header("Rotación")]
    [Tooltip("Velocidad de rotación al girar hacia la dirección de movimiento")]
    public float rotationSpeed = 10f;

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

    // Animator para controlar las animaciones
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isWalking", false);

        if (controller == null)
            Debug.LogError("El jugador no tiene un CharacterController asignado.");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!moviendo)
            return;

        // Dirección y distancia al destino
        Vector3 offset = destino - transform.position;
        float distance = offset.magnitude;

        // Si estamos cerca, detener
        if (distance < distanciaDetencion)
        {
            moviendo = false;
            return;
        }

        // Normalizar dirección (solo en XZ)
        Vector3 direction = offset.normalized;
        Vector3 lookDir = new Vector3(direction.x, 0f, direction.z);

        // Rotar suavemente hacia la dirección de movimiento
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Mover con SimpleMove (aplica gravedad)
        controller.SimpleMove(direction * velocidad);
    }

    /// <summary>
    /// Establece un nuevo destino y activa el movimiento.
    /// </summary>
    public void MoverHacia(Vector3 posicionDestino)
    {
        destino = posicionDestino;
        // Disparar animacion Caminar mientras se mueve
        animator?.SetBool("isWalking", true);
        moviendo = true;

    }

    /// <summary>
    /// Reproduce un sonido usando el AudioSource del personaje.
    /// </summary>
    public void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Reproduce el sonido de victoria del personaje.
    /// </summary>
    public void ReproducirSonidoVictoria()
    {
        if (audioSource != null && victorySoundClip != null)
            audioSource.PlayOneShot(victorySoundClip);
    }

    public void AnimacionCoger()
    {
        animator?.SetTrigger("coger");
    }

    public void AnimacionMezclar()
    {
        animator?.SetTrigger("mezclar");
    }

    public void AnimacionVictoria()
    {
        
        animator?.SetTrigger("victoria");
        ReproducirSonidoVictoria();

    }



}
