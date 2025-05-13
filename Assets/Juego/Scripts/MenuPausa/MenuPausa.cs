using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    //public GameObject menuPausa;
    [SerializeField] private GameObject botonPausa;
    public bool pausado;

    private string escenaOriginal;

    void Start()
    {
        //menuPausa.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        escenaOriginal = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MenuPausa", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        pausado = true;
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("MenuPausa");
        pausado = false;
    }


    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.FinalizeGame();
        MoneyManager.Instance.FinalizeMoney();
        pausado = false;
    }
}
