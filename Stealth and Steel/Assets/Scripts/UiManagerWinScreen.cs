using UnityEngine;
using UnityEngine.UI;

public class UiManagerWinScreen : MonoBehaviour
{
    [SerializeField]
    private Button _levelsButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _level1Button;
    [SerializeField] 
    private Button _level2Button;
    [SerializeField]
    private Button _level3Button;
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject _menuLevels;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _levelsButton.onClick.AddListener(OnLevelsButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _level1Button.onClick.AddListener(OnLevel1ButtonClicked);
        _level2Button.onClick.AddListener(OnLevel2ButtonClicked);
        _level3Button.onClick.AddListener(OnLevel3ButtonClicked);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnLevelsButtonClicked()
    {
        // Reload the current scene
        _menu.SetActive(false);
        _menuLevels.SetActive(true);
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
    private void OnLevel1ButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    private void OnLevel2ButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
    private void OnLevel3ButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
}
