using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // Implementación como Singleton
    public static MoneyManager Instance { get; private set; }

    // Variable que acumula las ganancias a lo largo de las noches
    public int Ganancias { get; private set; } = 0;

    void Awake()
    {
        // Si no hay un MoneyManager, se asigna este y se marca para que no se destruya al cargar otras escenas.
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
    /// Se llama al pulsar el botón de tag "irTienda". 
    /// Calcula la diferencia entre el dinero ganado en la noche y el dinero pedido, 
    /// y la suma a las ganancias acumuladas.
    /// </summary>
    /// <param name="nightMoney">Dinero ganado en la noche actual.</param>
    /// <param name="targetMoney">Dinero pedido para la noche (objetivo).</param>
    public void IrTienda(int nightMoney, int targetMoney)
    {
        int leftover = nightMoney - targetMoney;
        // Se acumula el sobrante (puede ser cero o mayor; asumiendo que se cumplió el objetivo)
        Ganancias += leftover;
        Debug.Log("IrTienda: NightMoney=" + nightMoney +
                  ", TargetMoney=" + targetMoney +
                  ", Leftover=" + leftover +
                  ", Total Ganancias=" + Ganancias);
    }

    /// <summary>
    /// Se llama al pulsar el botón de tag "comprar". Resta el coste de la compra a las Ganancias.
    /// </summary>
    /// <param name="cost">El coste de la compra, asignado en el Inspector en el botón.</param>
    public void Comprar(int cost)
    {
        Ganancias = Mathf.Max(0, Ganancias - cost);
        Debug.Log("Comprar: Cost=" + cost + ", Ganancias ahora=" + Ganancias);
    }
}
