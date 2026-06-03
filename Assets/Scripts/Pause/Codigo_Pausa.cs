using UnityEngine;
using UnityEngine.SceneManagement;

public class Codigo_Pausa : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool isPaused = false;
    public GameObject ExitMenu;
    public GameObject PauseMenu;
    public GameObject _TeleportMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                PauseMenu.SetActive(true);
                isPaused = true;

                Time.timeScale = 0f; // Detiene el tiempo en el juego
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (isPaused == true)
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        ExitMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f; // Reanuda el tiempo en el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu(string sceneName)
    {
        Time.timeScale = 1f; // Reanuda el tiempo en el juego
        SceneManager.LoadScene(sceneName);
    }

    public void TeleportMenu()
    {
        PauseMenu.SetActive(false);
        _TeleportMenu.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        _TeleportMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

}
