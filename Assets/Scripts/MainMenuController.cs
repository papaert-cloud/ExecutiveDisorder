using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public TextMeshProUGUI titleText;
    public GameObject characterSelectPanel;
    public PoliticalCharacter[] availableCharacters;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartGame);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitGame);
            
        if (titleText != null)
            titleText.text = "EXECUTIVE DISORDER";
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Crisis");
    }

    public void OnQuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < availableCharacters.Length)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentCharacter = availableCharacters[index];
            }
            OnStartGame();
        }
    }
}
