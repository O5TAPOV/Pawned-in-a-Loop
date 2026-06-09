using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;

    public void StartNewGame()
    {
        Debug.Log("Запускаємо гру...");
        SceneManager.LoadScene("SampleScene");
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

    public void ExitGame()
    {
        Debug.Log("Виходимо з гри...");
        Application.Quit();
    }

    public void ClosePanel(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
    }
}