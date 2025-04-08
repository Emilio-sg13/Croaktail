using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float reachThreshold = 0.1f; // Distancia para detenerse
    private Vector3 destino;
    private bool moviendo = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("El jugador no tiene un CharacterController asignado.");
        }
    }

    void Update()
    {
        if (moviendo)
        {
            // Calcula la dirección hacia el destino
            Vector3 direction = destino - transform.position;
            float distance = direction.magnitude;
            if (distance < reachThreshold)
            {
                moviendo = false;
                return;
            }

            direction.Normalize();
            // Movimiento a realizar este frame
            Vector3 movimiento = direction * velocidad * Time.deltaTime;

            // Evitar sobrepasar el destino
            if (movimiento.magnitude > distance)
            {
                movimiento = direction * distance;
            }

            // Mueve el personaje; CharacterController gestiona las colisiones
            controller.Move(movimiento);
        }
    }

    // Establece el destino y activa el movimiento
    public void MoverHacia(Vector3 posicionDestino)
    {
        destino = posicionDestino;
        moviendo = true;
    }
}
