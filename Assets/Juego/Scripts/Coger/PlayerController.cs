using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //[Tooltip("Velocidad de desplazamiento en unidades por segundo")]
    public float velocidad = 5f;
    //[Tooltip("Distancia mínima al destino para detenerse")]
    public float distanciaDetencion = 0.1f;

    private Vector3 destino;
    private bool moviendo = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            Debug.LogError("El jugador no tiene un CharacterController asignado.");
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
}
