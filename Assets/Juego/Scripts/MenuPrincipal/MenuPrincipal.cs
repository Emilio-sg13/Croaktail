using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Poner escena menu principal en primer lugar de build settings

    }

    public void QuitGame()
    {
       // Application.Quit();
        EditorApplication.isPlaying = false;
    }

    public void reStart()
    {
        GameManager.Instance.StartGame();
    }
}
