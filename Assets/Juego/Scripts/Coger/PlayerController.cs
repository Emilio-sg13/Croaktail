using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    private Vector3 destino;
    private bool moviendo = false;

    void Update()
    {
        if (moviendo)
        {
            // Moverse hacia el destino
            float paso = velocidad * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destino, paso);

            // Cuando se llega lo suficientemente cerca, se detiene el movimiento
            if (Vector3.Distance(transform.position, destino) < 0.1f)
            {
                moviendo = false;
            }
        }
    }

    // Método para establecer el destino y comenzar el movimiento
    public void MoverHacia(Vector3 posicionDestino)
    {
        destino = posicionDestino;
        moviendo = true;
    }
}

