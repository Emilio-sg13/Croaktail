using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuPausa : MonoBehaviour
{
    private string escenaOriginal;

    // Asignar desde el Inspector
    public Animator menuPausaImagenAnimacion;

    // Nombre de la escena del menú de pausa
    public string escenaParaCargar = "MenuPausa";

    /// Llama desde el botón de pausa en la escena principal
    public void PausarJuego()
    {
        FindFirstObjectByType<BGMController>()?.PausarMusica();

        GameObject pauseButton = GameObject.FindWithTag("PauseButton");
        if (pauseButton != null && pauseButton.TryGetComponent(out Button b))
        {
            b.interactable = false;
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
        }
    }

    private IEnumerator EsperarAnimacionYLoad(Animator animator, string escena)
    {
        float duracionAnimacion = 0.9f;

        yield return new WaitForSecondsRealtime(duracionAnimacion);

        SceneManager.LoadScene(escena, LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        FindFirstObjectByType<BGMController>()?.ReanudarMusica();

        GameObject pauseButton = GameObject.FindWithTag("PauseButton");
        if (pauseButton != null && pauseButton.TryGetComponent(out Button b))
        {
            b.interactable = true;
        }

        if (menuPausaImagenAnimacion != null)
        {
            menuPausaImagenAnimacion.updateMode = AnimatorUpdateMode.UnscaledTime;

            Scene escenaOriginalObj = SceneManager.GetSceneByName(escenaOriginal);
            if (escenaOriginalObj.IsValid() && escenaOriginalObj.isLoaded)
            {
                SceneManager.SetActiveScene(escenaOriginalObj);
            }

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
        if (menuPausaImagenAnimacion != null)
        {
            menuPausaImagenAnimacion.SetTrigger("Cerrar");
            yield return new WaitForSecondsRealtime(1.0f);
        }

        Time.timeScale = 1f;

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("MenuPausa");
        yield return new WaitUntil(() => unloadOp.isDone);
    }

    private IEnumerator CerrarSinAnimacion()
    {
        Time.timeScale = 1f;

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
    }
}
