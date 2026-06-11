using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel; // Сюди перетягнеш свою PausePanel
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;

    private bool isPaused = false; // Змінна, щоб знати, чи ми зараз на паузі

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(isPaused)
                ResumeGame();
            else
                PauseGame();
        }

    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Вихід з гри на робочий стіл");
        Application.Quit();
    }

    public void OpenSettings()
    {
        Debug.Log("Відкриваємо налаштування...");
        settingsPanel.SetActive(true); // Вмикаємо панель налаштувань
    }

    public void OpenTutorial()
    {
        Debug.Log("Відкриваємо туторіал...");
        tutorialPanel.SetActive(true); // Вмикаємо панель туторіалу
    }

    public void ClosePanel(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }
}