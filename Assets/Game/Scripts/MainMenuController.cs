
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Settings Panel")]
    public GameObject settingsPanel;

    [Header("Game Scene")]
    public string gameSceneName = "GamePlay";

    private void Start()
    {
        // Settings tertutup saat Main Menu pertama kali dibuka
        settingsPanel.SetActive(false);
    }

    // Membuka Scene permainan
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Membuka panel Settings
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Menutup panel Settings
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Keluar dari aplikasi
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Keluar dari Eco-Bot");
    }
}
