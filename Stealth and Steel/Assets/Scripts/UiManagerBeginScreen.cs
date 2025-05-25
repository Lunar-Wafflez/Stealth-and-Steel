using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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
    [SerializeField]
    private GameObject _levelsMenu;
    [SerializeField]
    private Button _level1Button;
    [SerializeField]
    private Button _level2Button;
    [SerializeField]
    private Button _level3Button;
    [SerializeField]
    private GameObject _warning;

    private float _timer = 0f;
    private bool _showingWarning = false;
    private float _warningDuration = 3f;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controlsButton.onClick.AddListener(OnControlsButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _level1Button.onClick.AddListener(OnLevel1ButtonClicked);
        _level2Button.onClick.AddListener(OnLevel2ButtonClicked);
        _level3Button.onClick.AddListener(OnLevel3ButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
       /* if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _warning.SetActive(false);
                _showingWarning = false;
            }
        }*/

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
        _levelsMenu.SetActive(true);
        _mainMenu.SetActive(false);
    }
    private void OnLevel1ButtonClicked()
    {
        SceneManager.LoadScene("Level1");
    }
    private void OnLevel2ButtonClicked()
    {
        SceneManager.LoadScene("Level2");
        /*_warning.SetActive(true);
        _timer = _warningDuration;*/

    }
    private void OnLevel3ButtonClicked()
    {
        SceneManager.LoadScene("Level3");
    }
}
