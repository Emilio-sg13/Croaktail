using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuPausa : MonoBehaviour
{
    private bool pausado = false;
    private string escenaOriginal;

    // Asignar desde el Inspector
    public Animator menuPausaImagenAnimacion;

    // Nombre de la escena del menú de pausa
    public string escenaParaCargar = "MenuPausa";

    /// Llama desde el botón de pausa en la escena principal
    public void PausarJuego()
    {
        FindFirstObjectByType<BGMController>()?.PausarMusica();

        // Desactiva el botón de pausa
        GameObject btn = GameObject.FindWithTag("PauseButton");
        if (btn != null)
        {
            Button b = btn.GetComponent<Button>();
            if (b != null) b.interactable = false;
        }

        escenaOriginal = SceneManager.GetActiveScene().name;

        if (menuPausaImagenAnimacion != null)
        {
            menuPausaImagenAnimacion.gameObject.SetActive(true);
            menuPausaImagenAnimacion.SetTrigger("Abrir"); // Usa Trigger "Abrir"
            StartCoroutine(EsperarAnimacionYLoad(menuPausaImagenAnimacion, escenaParaCargar));
        }
        else
        {
            SceneManager.LoadScene(escenaParaCargar, LoadSceneMode.Additive);
            Time.timeScale = 0f;
            pausado = true;
        }
    }

    private IEnumerator EsperarAnimacionYLoad(Animator animator, string escena)
    {
        float duracionAnimacion = 0.9f;

        yield return new WaitForSecondsRealtime(duracionAnimacion);

        SceneManager.LoadScene(escena, LoadSceneMode.Additive);
        Time.timeScale = 0f;
        pausado = true;
    }

    public void ReanudarJuego()
    {
        pausado = false;
        FindFirstObjectByType<BGMController>()?.ReanudarMusica();

        GameObject btn = GameObject.FindWithTag("PauseButton");
        if (btn != null)
        {
            Button b = btn.GetComponent<Button>();
            if (b != null) b.interactable = true;
        }

        if (menuPausaImagenAnimacion != null)
        {
            menuPausaImagenAnimacion.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Asegura que la escena principal esté activa primero
            Scene escenaOriginalObj = SceneManager.GetSceneByName(escenaOriginal);
            if (escenaOriginalObj.IsValid() && escenaOriginalObj.isLoaded)
            {
                SceneManager.SetActiveScene(escenaOriginalObj);
            }

            // Eeproduce la animación y descarga la escena de pausa
            StartCoroutine(AnimarCerrarYCerrarMenuPausa());
        }
        else
        {
            StartCoroutine(CerrarSinAnimacion());
        }
    }

    private IEnumerator CargarEscenaOriginalYAnimarCerrar()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(escenaOriginal, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadOp.isDone);

        Scene escenaOriginalObj = SceneManager.GetSceneByName(escenaOriginal);
        SceneManager.SetActiveScene(escenaOriginalObj);

        yield return AnimarCerrarYCerrarMenuPausa();
    }

    private IEnumerator AnimarCerrarYCerrarMenuPausa()
    {
        // Reproduce animación de cierre
        if (menuPausaImagenAnimacion != null)
        {
            menuPausaImagenAnimacion.SetTrigger("Cerrar");
            yield return new WaitForSecondsRealtime(1.0f);
        }

        // Reactiva el tiempo antes de descargar la escena de pausa
        Time.timeScale = 1f;
        pausado = false;

        // Descarga la escena de pausa
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("MenuPausa");
        yield return new WaitUntil(() => unloadOp.isDone);
    }

    private IEnumerator CerrarSinAnimacion()
    {
        Time.timeScale = 1f;
        pausado = false;

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("MenuPausa");
        yield return new WaitUntil(() => unloadOp.isDone);

        Scene escenaOriginalObj = SceneManager.GetSceneByName(escenaOriginal);
        if (escenaOriginalObj.IsValid() && escenaOriginalObj.isLoaded)
        {
            SceneManager.SetActiveScene(escenaOriginalObj);
        }
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.FinalizeGame();
        MoneyManager.Instance.FinalizeMoney();
        pausado = false;
    }
}
