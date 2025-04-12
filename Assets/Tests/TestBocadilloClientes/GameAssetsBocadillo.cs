using UnityEngine;

public class GameAssetsBocadillo : MonoBehaviour
{
    public static GameAssetsBocadillo Instance { get; private set; }

    public GameObject BocadilloPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
