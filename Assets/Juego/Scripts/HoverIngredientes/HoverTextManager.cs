using TMPro;
using UnityEngine;

public class HoverTextManager : MonoBehaviour
{
    public TextMeshProUGUI tooltip;
    public Vector2 pixelOffset = new Vector2(-10, 15);

    [Header("Audio")]
    public AudioSource hoverAudio;          
    public AudioClip  hoverClip;            // opcional: clip específico

    public static HoverTextManager Instance;

    float nextPlayTime = 0f;
    public float minInterval = 0.05f;   // 50 ms de intervalo entre sonido y sonido

    void Awake()
    {
        Instance = this;
        tooltip.gameObject.SetActive(false);
    }

    public void Show(string msg)
    {
        tooltip.text = msg;
        tooltip.gameObject.SetActive(true);

        if (Time.time >= nextPlayTime && hoverAudio && hoverClip)
        {
            hoverAudio.PlayOneShot(hoverClip);
            nextPlayTime = Time.time + minInterval;
        }
    }

    public void Hide() => tooltip.gameObject.SetActive(false);

    void Update()
    {
        if (tooltip.gameObject.activeSelf)
            tooltip.transform.position = Input.mousePosition + (Vector3)pixelOffset;
    }
}
