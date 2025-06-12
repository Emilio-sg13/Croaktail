using UnityEngine;

public class Cliente : MonoBehaviour
{
    public Sprite pedidoSprite; // Se compara con el sprite del inventario al servir

    // 🔥 NUEVO: efectos visuales
    public GameObject sparkEffectPrefab; // 火花 prefab
    public Transform sparkPoint;         // 特效生成位置（拖一个空物体如 "Mouth"）

    public void RecibirCoctel()
    {
        if (sparkEffectPrefab != null && sparkPoint != null)
        {
            GameObject chispa = Instantiate(sparkEffectPrefab, sparkPoint.position, Quaternion.identity);
            Destroy(chispa, 2f); // 避免特效堆积
        }

        Debug.Log("¡Cliente ha recibido el cóctel con chispas!");
    }
}
