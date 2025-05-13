using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManagerBeginScreen : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _controlsButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _controlsMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controlsButton.onClick.AddListener(OnControlsButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _startButton.onClick.AddListener(OnStartButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnControlsButtonClicked()
    {
        _mainMenu.SetActive(false);
        _controlsMenu.SetActive(true);
    }
    private void OnBackButtonClicked()
    {
        _mainMenu.SetActive(true);
        _controlsMenu.SetActive(false);
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Level1");
    }
}
