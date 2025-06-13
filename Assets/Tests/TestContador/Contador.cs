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

    [Header("GameObjects de transición")]
    public GameObject transicionVictoriaGO;   
    public float duracionVictoria = 0.9f;

    public GameObject transicionDerrotaGO;    
    public float duracionDerrota =0.9f;

    /*--------------- ESTADO ----------------*/
    private bool finDisparado = false;

    /*--------------- INICIO ----------------*/
    void Start()
    {
        // Desactiva las transiciones al arrancar
        if (transicionVictoriaGO) transicionVictoriaGO.SetActive(false);
        if (transicionDerrotaGO) transicionDerrotaGO.SetActive(false);
    }

    /*--------------- UPDATE ----------------*/
    void Update()
    {
        if (finDisparado) return;

        // Actualiza cronómetro
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
                    null));                    // sin acción extra
            }
            else
            {
                StartCoroutine(TransicionYCarga(
                    transicionVictoriaGO,
                    duracionVictoria,
                    "PantallaVictoria",
                    () =>                      // acción que se lanza justo antes del cambio
                        MoneyManager.Instance.IrTienda(dineroConseguido, dineroObjetivo)
                ));
            }
        }
        else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }

    /*--------------- COROUTINE -------------*/
    private IEnumerator TransicionYCarga(GameObject go, float dur, string escena, System.Action postAnim)
    {
        // Pausa lógicamente el juego pero permite que la UI/Animator se actualice
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

        // Reactiva el flujo normal del juego
        Time.timeScale = 1f;

        // Carga de escena
        SceneManager.LoadScene(escena, LoadSceneMode.Single);
    }

    /*--------------- EXTRA -----------------*/
    public void ResetearTiempo()
    {
        tiempoRestante = 20f;
        FindFirstObjectByType<BGMController>()?.SaltarUltimos20Segundos();
    }
}
