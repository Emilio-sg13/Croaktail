using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    
    public Camera mainCamera1;
    public Camera mainCamera2;

    // Interna para saber cu�l est� activa
    private bool isUsingCamera1 = true;

    void Start()
    {
        // Aseg�rate de que al inicio solo 1 est� activo
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
