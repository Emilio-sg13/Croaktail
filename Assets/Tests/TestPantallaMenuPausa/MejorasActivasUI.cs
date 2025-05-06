using UnityEngine;
using TMPro;

public class MejorasActivasUI : MonoBehaviour
{
    public static MejorasActivasUI Instance;

    public TextMeshProUGUI slot1Texto;
    public TextMeshProUGUI slot2Texto;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MostrarMejorasActivas();
    }

    public void MostrarMejorasActivas()
    {
        var mejorasActivas = new System.Collections.Generic.List<string>();

        if (UpgradeData.masClientes) mejorasActivas.Add("Más Clientes activado");
        if (UpgradeData.dineroTriple) mejorasActivas.Add("Dinero Triple activado");
        if (UpgradeData.coctelesDobles) mejorasActivas.Add("Cócteles Dobles activado");
        if (UpgradeData.mezcladoRapido) mejorasActivas.Add("mezcladoRapido activada");
        if (UpgradeData.inventorySlotExtraActivado) mejorasActivas.Add("Mejora 5 activada");

        slot1Texto.text = mejorasActivas.Count > 0 ? mejorasActivas[0] : " ";
        slot2Texto.text = mejorasActivas.Count > 1 ? mejorasActivas[1] : " ";
    }
}
