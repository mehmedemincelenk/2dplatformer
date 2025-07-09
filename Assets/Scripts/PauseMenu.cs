using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        
   if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    public void GoToMainMenu()
        {
        Time.timeScale = 1f; // Resume the game before loading a new scene
        SceneManager.LoadScene("Main_Menu"); // Replace with your main menu scene name
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Oyundan Çýkýlýyor..."); // Log for debugging purposes
    }

}