using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Singleton ---
    public static GameManager Instance;

    // --- Variables de configuraci�n ---
    // Contador de noches (nivel). Se incrementa cada vez que se pasa de noche.
    public int nightCounter = 1;
    // Cantidad de dinero que se requiere para la primera noche.
    // La secuencia de dinero requerido ser�: X, 2X, 3X, ...
    public int initialTargetMoney = 100;

    // Propiedad que devuelve el dinero requerido en la noche actual.
    public int CurrentTargetMoney
    {
        get { return initialTargetMoney * nightCounter; }
    }

    void Awake()
    {
        // Implementaci�n del Singleton: si ya existe uno, se destruye el duplicado.
        if (Instance == null)
        {
            Instance = this;
            // Permite que el GameManager persista a trav�s de las escenas.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    /// <summary>
    /// Llamado cuando se pulsa el bot�n de tag "inicio" en el MainMenu.
    /// Reinicia el contador de noches y carga la escena "Noche".
    /// </summary>
    public void StartGame()
    {
        nightCounter = 1;
        SceneManager.LoadScene("Noche");
    }

    /// <summary>
    /// Llamado cuando se pulsa el bot�n de tag "finalizar" (disponible en varias escenas).
    /// Puede, por ejemplo, llevar al MainMenu o cerrar el juego.
    /// En este ejemplo se destruye el GameManager.
    /// </summary>
    public void FinalizeGame()
    {
        // Puedes realizar otras acciones aqu�, como guardar el progreso o mostrar una pantalla final.
        SceneManager.LoadScene("MenuPrincipal");
        Destroy(gameObject);
    }

    /// <summary>
    /// Llamado desde la escena "Tienda" al pulsar el bot�n de tag "siguiente".
    /// Incrementa el contador de noches y carga la escena "Noche".
    /// </summary>
    public void LoadNextNight()
    {
        nightCounter++;
        SceneManager.LoadScene("Noche");
    }
}
