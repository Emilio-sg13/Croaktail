using UnityEngine;
using System.Collections.Generic;

public class TiendaManager : MonoBehaviour
{
    // Referencias a los SpriteRenderer donde se mostrar�n las dos mejoras
    public SpriteRenderer mejoraSlot1;
    public SpriteRenderer mejoraSlot2;

    // Arreglo de sprites para las 5 mejoras
    // El orden del array debe corresponder con las mejoras 1 a 5
    public Sprite[] mejorasSprites;

    // Variable para recordar qu� mejora se est� mostrando en cada slot
    private int mejoraMostradaSlot1 = -1;
    private int mejoraMostradaSlot2 = -1;

    void Start()
    {
        MostrarMejorasAleatorias();
    }

    // M�todo para escoger y mostrar dos mejoras aleatorias
    public void MostrarMejorasAleatorias()
    {
        // 1. Crear una lista de �ndices de mejoras no compradas
        List<int> mejorasNoCompradas = new List<int>();

        // (a) Checar cada mejora en UpgradeData:
        //     si est� en "false", significa que no se ha comprado
        if (!UpgradeData.mejora1) mejorasNoCompradas.Add(0); // �ndice 0 corresponder� a mejora1
        if (!UpgradeData.mejora2) mejorasNoCompradas.Add(1); // �ndice 1 corresponder� a mejora2
        if (!UpgradeData.mejora3) mejorasNoCompradas.Add(2);
        if (!UpgradeData.mejora4) mejorasNoCompradas.Add(3);
        if (!UpgradeData.mejora5) mejorasNoCompradas.Add(4);

        // 2. Si la lista est� vac�a, significa que ya no hay mejoras disponibles
        if (mejorasNoCompradas.Count == 0)
        {
            Debug.Log("No hay mejoras disponibles para comprar.");
            // Puedes ocultar los slots o indicar que no hay m�s mejoras
            mejoraSlot1.enabled = false;
            mejoraSlot2.enabled = false;
            return;
        }

        // 3. Escoger la primera mejora aleatoria
        int randomIndex = Random.Range(0, mejorasNoCompradas.Count);
        mejoraMostradaSlot1 = mejorasNoCompradas[randomIndex];
        mejorasNoCompradas.RemoveAt(randomIndex); // para no volver a escoger la misma

        // 4. Asignar sprite al Slot1
        mejoraSlot1.sprite = mejorasSprites[mejoraMostradaSlot1];
        mejoraSlot1.enabled = true;

        // 5. Si a�n quedan mejoras sin comprar, escoger la segunda
        if (mejorasNoCompradas.Count > 0)
        {
            randomIndex = Random.Range(0, mejorasNoCompradas.Count);
            mejoraMostradaSlot2 = mejorasNoCompradas[randomIndex];
            mejorasNoCompradas.RemoveAt(randomIndex);

            // Asignar sprite al Slot2
            mejoraSlot2.sprite = mejorasSprites[mejoraMostradaSlot2];
            mejoraSlot2.enabled = true;
        }
        else
        {
            // No hay segunda mejora disponible
            mejoraMostradaSlot2 = -1;
            mejoraSlot2.enabled = false;
        }
    }

    // Ejemplo: cuando el jugador compre la mejora del primer slot
    public void ComprarMejoraSlot1()
    {
        if (mejoraMostradaSlot1 == -1)
            return; // no hay mejora asignada

        // Actualizamos en UpgradeData seg�n el �ndice de la mejora
        switch (mejoraMostradaSlot1)
        {
            case 0: UpgradeData.mejora1 = true; break;
            case 1: UpgradeData.mejora2 = true; break;
            case 2: UpgradeData.mejora3 = true; break;
            case 3: UpgradeData.mejora4 = true; break;
            case 4: UpgradeData.mejora5 = true; break;
        }

        Debug.Log("Compraste la mejora del slot 1.");
        // Recarga las mejoras disponibles
        MostrarMejorasAleatorias();
    }

    // Ejemplo: cuando el jugador compre la mejora del segundo slot
    public void ComprarMejoraSlot2()
    {
        if (mejoraMostradaSlot2 == -1)
            return; // no hay mejora asignada

        switch (mejoraMostradaSlot2)
        {
            case 0: UpgradeData.mejora1 = true; break;
            case 1: UpgradeData.mejora2 = true; break;
            case 2: UpgradeData.mejora3 = true; break;
            case 3: UpgradeData.mejora4 = true; break;
            case 4: UpgradeData.mejora5 = true; break;
        }

        Debug.Log("Compraste la mejora del slot 2.");
        // Recarga las mejoras disponibles
        MostrarMejorasAleatorias();
    }
}

