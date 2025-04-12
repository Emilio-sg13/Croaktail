using UnityEngine;

public class PedidosClientesPez : MonoBehaviour
{
    [SerializeField] private Transform chatParent;  // Asegúrate de asignarlo correctamente en el Inspector
    private bocadilloClientes mensajeActual = null;  // Para almacenar el objeto de mensaje actual
    private bool isMensajeVisible = false;  // Para verificar si el mensaje está visible
    private string mensajePedido = "";  // Para almacenar el mensaje del pedido
    private bool isWaitingForMessage = false;  // Para controlar si estamos esperando que el mensaje desaparezca

    private void Start()
    {
        // Asegurarse de que chatParent esté correctamente asignado
        if (chatParent == null)
            chatParent = this.transform;  // Asigna el transform si no se asigna desde el Inspector
    }

    private void OnMouseEnter()
    {
        // Si el mensaje no está visible y no estamos esperando, creamos un nuevo mensaje
        if (!isMensajeVisible && !isWaitingForMessage && mensajePedido != "")
        {
            CrearMensaje(mensajePedido);
        }
        // Si el mensaje aún no está asignado, lo asignamos aleatoriamente
        else if (!isMensajeVisible && mensajePedido == "")
        {
            mensajePedido = GetRandomMessage();  // Elegir un mensaje aleatorio
            CrearMensaje(mensajePedido);
        }
    }

    private void CrearMensaje(string mensaje)
    {
        // Creación del mensaje solo si no existe ya uno visible
        if (mensajeActual == null)
        {
            // Creamos el mensaje en la posición ajustada (por encima del cliente)
            mensajeActual = bocadilloClientes.create(chatParent, Vector3.up * 2f, mensaje);
            isMensajeVisible = true;  // Marca que ya hay un mensaje visible

            // Después de 4 segundos, el mensaje desaparece
            Destroy(mensajeActual.gameObject, 4f);

            // Empezamos la espera para permitir la re-creación del mensaje después de desaparecer
            StartCoroutine(RestablecerMensaje());
        }
    }

    private System.Collections.IEnumerator RestablecerMensaje()
    {
        isWaitingForMessage = true;  // Indicamos que estamos esperando
        yield return new WaitForSeconds(4f);  // Esperamos 4 segundos antes de permitir que el mensaje se repita
        isMensajeVisible = false;  // Ahora podemos mostrar otro mensaje cuando el ratón pase por encima
        isWaitingForMessage = false;  // Finalizamos la espera
    }

    private string GetRandomMessage()
    {
        string[] mensajes = { "Agua de Albufera", "Moscow Bug's" };
        return mensajes[Random.Range(0, mensajes.Length)];
    }
}
