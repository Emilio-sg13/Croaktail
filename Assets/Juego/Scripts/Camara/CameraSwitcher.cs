using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    
    public Camera mainCamera1;
    public Camera mainCamera2;

    // Interna para saber cuál está activa
    private bool isUsingCamera1 = true;

    void Start()
    {
        // Asegúrate de que al inicio solo 1 esté activo
        if (mainCamera1 != null) mainCamera1.enabled = true;
        if (mainCamera2 != null) mainCamera2.enabled = false;
    }

    void Update()
    {
        // Al pulsar 'C' cambiamos
        if (Input.GetKeyDown(KeyCode.C))
        {
            isUsingCamera1 = !isUsingCamera1;

            if (mainCamera1 != null) mainCamera1.enabled = isUsingCamera1;
            if (mainCamera2 != null) mainCamera2.enabled = !isUsingCamera1;
        }
    }
}
