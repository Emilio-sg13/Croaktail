using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    private bool pausado = false;
    private string escenaOriginal;

    /// <summary>
    /// Llamar desde el bot髇 de pausa (Scene principal).  
    /// </summary>
    public void PausarJuego()
    {

        FindFirstObjectByType<BGMController>()?.PausarMusica(); // 🎵 暂停音乐

        // 1) Deshabilita el bot髇 de pausa usando su Tag
        GameObject btn = GameObject.FindWithTag("PauseButton");
        if (btn != null)
        {
            Button b = btn.GetComponent<Button>();
            if (b != null) b.interactable = false;
        }

        // 2) Guarda la escena actual y carga el men?de pausa de forma aditiva
        escenaOriginal = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);

        // 3) Detiene el tiempo
        Time.timeScale = 0f;
        pausado = true;
    }

    /// <summary>
    /// Llamar desde el bot髇 揜eanudar?en la escena MenuPausa.  
    /// </summary>
    public void ReanudarJuego()
    {

        FindFirstObjectByType<BGMController>()?.ReanudarMusica(); // 🎵 继续音乐

        // 1) Reactiva el bot髇 de pausa en la escena principal usando su Tag
        GameObject btn = GameObject.FindWithTag("PauseButton");
        if (btn != null)
        {
            Button b = btn.GetComponent<Button>();
            if (b != null) b.interactable = true;
        }

        // 2) Reactiva el tiempo y cierra la escena de pausa
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        pausado = false;
    }

    /// <summary>
    /// Llamar desde el bot髇 揑r al men鷶 en la escena MenuPausa.  
    /// </summary>
    public void IrAlMenu()
    {
        // Asegura que el juego est?reanudado antes de cambiar de escena
        Time.timeScale = 1f;
        GameManager.Instance.FinalizeGame();
        MoneyManager.Instance.FinalizeMoney();
        pausado = false;
    }
}
