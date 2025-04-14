using UnityEngine;

public class TestMejorasManger : MonoBehaviour
{
    // Singleton para acceder a la instancia en cualquier escena
    public static TestMejorasManger Instance { get; private set; }

    // Bandera para indicar si la mejora "dinero triple" está activa
    public bool dineroTripleActivado = false;

    void Awake()
    {
        // Implementación del Singleton y persiste entre escenas
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
    /// Si la mejora dineroTriple está activada, multiplica el valor original por 3.
    /// De lo contrario, devuelve el valor original.
    /// </summary>
    /// <param name="valorOriginal">El valor original del item.</param>
    /// <returns>El valor final, multiplicado por 3 si la mejora está activa, o el valor original.</returns>
    public int AplicarDineroTriple(int valorOriginal)
    {
        return UpgradeData.dineroTriple ? valorOriginal * 3 : valorOriginal;
    }

}
