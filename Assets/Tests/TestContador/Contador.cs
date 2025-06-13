using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Contador : MonoBehaviour
{
    /*--------------- CONFIG ----------------*/
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI contadorTexto;
    [SerializeField] private float tiempoRestante = 120f;

    [Header("Lógica externa")]
    public BarraCobroUI barraCobroUI;
    public GameManager gameManager;
    public PlayerController jugador;

    [Header("GameObjects de transición")]
    public GameObject transicionVictoriaGO;
    public float duracionVictoria = 0.9f;

    public GameObject transicionDerrotaGO;
    public float duracionDerrota = 0.9f;

    [Header("Animación jugador")]
    public float duracionAnimacionVictoriaJugador = 1.1f;

    /*--------------- ESTADO ----------------*/
    private bool finDisparado = false;

    /*--------------- INICIO ----------------*/
    void Start()
    {
        if (transicionVictoriaGO) transicionVictoriaGO.SetActive(false);
        if (transicionDerrotaGO) transicionDerrotaGO.SetActive(false);
    }

    /*--------------- UPDATE ----------------*/
    void Update()
    {
        if (finDisparado) return;

        int min = Mathf.FloorToInt(tiempoRestante / 60);
        int seg = Mathf.FloorToInt(tiempoRestante % 60);
        contadorTexto.text = $"{min:00}:{seg:00}";

        if (tiempoRestante <= 0f)
        {
            finDisparado = true;
            contadorTexto.text = "00:00";

            int dineroConseguido = barraCobroUI.GetTotalActual();
            int dineroObjetivo = GameManager.Instance.CurrentTargetMoney;

            if (dineroConseguido < dineroObjetivo)
            {
                StartCoroutine(TransicionYCarga(
                    transicionDerrotaGO,
                    duracionDerrota,
                    "PantallaDerrota",
                    null
                ));
            }
            else
            {
                StartCoroutine(EsperarAnimacionJugadorYTransicion(
                    transicionVictoriaGO,
                    duracionVictoria,
                    "PantallaVictoria",
                    dineroConseguido,
                    dineroObjetivo
                ));
            }
        }
        else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }

    /*-- NUEVA COROUTINA: esperar animación jugador y luego transicionar --*/
    private IEnumerator EsperarAnimacionJugadorYTransicion(GameObject go, float dur, string escena, int dinero, int objetivo)
    {
        // 1) Lanza animación del jugador
        if (jugador != null)
        {
            jugador.AnimacionVictoria();
        }

        // 2) Esperar duración exacta de la animación
        yield return new WaitForSeconds(duracionAnimacionVictoriaJugador);

        // 3) Iniciar transición de pantalla
        yield return StartCoroutine(TransicionYCarga(
            go,
            dur,
            escena,
            () => MoneyManager.Instance.IrTienda(dinero, objetivo)
        ));
    }

    /*--------------- COROUTINE TRANSICIÓN -------------*/
    private IEnumerator TransicionYCarga(GameObject go, float dur, string escena, System.Action postAnim)
    {
        Time.timeScale = 0f;

        if (go != null)
        {
            Animator anim = go.GetComponent<Animator>();
            go.SetActive(true);

            if (anim)
            {
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                anim.Play(0, 0, 0f);
            }

            yield return new WaitForSecondsRealtime(dur);
        }

        postAnim?.Invoke();

        Time.timeScale = 1f;
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }

    /*--------------- EXTRA -----------------*/
    public void ResetearTiempo()
    {
        tiempoRestante = 20f;
        FindFirstObjectByType<BGMController>()?.SaltarUltimos20Segundos();
    }
}
