using UnityEngine;

public class DebugMenuToggle : MonoBehaviour
{
    [Tooltip("Arrastra aquí tu Panel de depuración")]
    public GameObject debugPanel;

    void Start()
    {
        // Asegurarnos de que empiece oculto
        if (debugPanel != null)
            debugPanel.SetActive(false);
    }

    void Update()
    {
        // Al pulsar 'D' alternamos visibilidad
        if (Input.GetKeyDown(KeyCode.D) && debugPanel != null)
        {
            debugPanel.SetActive(!debugPanel.activeSelf);
        }
    }
}
