using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{

    public void StartGame(string sceneName)
    {
        Time.timeScale = 1f; // Asegura que el tiempo esté normal al iniciar el juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
