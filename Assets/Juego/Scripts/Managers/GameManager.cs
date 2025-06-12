using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Singleton ---
    public static GameManager Instance;

    // --- Variables de configuración ---
    // Contador de noches (nivel). Se incrementa cada vez que se pasa de noche.
    public int nightCounter = 1;
    // Cantidad de dinero que se requiere para la primera noche.
    // La secuencia de dinero requerido será: X, 2X, 3X, ...
    public int initialTargetMoney = 100;

    // Propiedad que devuelve el dinero requerido en la noche actual.
    public int CurrentTargetMoney
    {
        get { return initialTargetMoney * nightCounter; }
    }

    void Awake()
    {
        // Implementación del Singleton: si ya existe uno, se destruye el duplicado.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Llamado cuando se pulsa el botón de tag "inicio" en el MainMenu.
    /// Reinicia el contador de noches y carga la escena "Noche".
    /// </summary>
    /// <summary>
    /// Llamado cuando se pulsa el botón de tag "inicio" en el MainMenu.
    /// Reinicia el contador de noches y carga la escena "Tutorial".
    /// </summary>
    public void StartGame()
    {
        nightCounter = 1;
        // Cargar escena de tutorial antes de la primera noche
        SceneManager.LoadScene("Tutorial");
    }

    /// <summary>
    /// Llamar al finalizar la escena de Tutorial para iniciar la primera noche.
    /// </summary>
    public void CompleteTutorial()
    {
        // Carga la primera noche tras el tutorial
        SceneManager.LoadScene("Noche");
    }

    /// <summary>
    /// Reinicia el juego completamente: resetea noches y vuelve a Noche sin destruir el GameManager.
    /// </summary>
    public void RestartGame()
    {
        nightCounter = 1;                  // Volver a nivel 1
        SceneManager.LoadScene("Noche"); // Recargar la escena Noche
    }

    /// <summary>
    /// Llamado cuando se pulsa el botón de tag "finalizar".
    /// Carga el menú principal y destruye el GameManager para permitir un inicio limpio.
    /// </summary>
    public void FinalizeGame()
    {
        SceneManager.LoadScene("MenuPrincipal");
        Destroy(gameObject);
    }

    /// <summary>
    /// Llamado desde la escena "Tienda" al pulsar el botón de tag "siguiente".
    /// Incrementa el contador de noches y carga la escena "Noche".
    /// </summary>
    public void LoadNextNight()
    {
        nightCounter++;
        SceneManager.LoadScene("Noche");
    }
}
