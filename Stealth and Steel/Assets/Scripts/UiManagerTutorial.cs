using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerTutorial : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _kunaiNumber;
    [SerializeField]
    private TextMeshProUGUI _smokeBombNumber;
    [SerializeField]
    private PlayerMovementScript _playerMovementScript;
    [SerializeField]
    private Image Heart1;
    [SerializeField]
    private Image Heart2;
    [SerializeField]
    private Image Heart3;
    [SerializeField]
    private Button _controls;
    [SerializeField]
    private GameObject _controlsMenu;

    [SerializeField]
    private TextMeshProUGUI _welcome;
    [SerializeField]
    private TextMeshProUGUI _goal;
    [SerializeField]
    private TextMeshProUGUI _carefull;
    [SerializeField]
    private TextMeshProUGUI _sneakKill;
    [SerializeField]
    private TextMeshProUGUI _spotted;
    [SerializeField]
    private TextMeshProUGUI _duel;
    [SerializeField]
    private TextMeshProUGUI _winAndLose;
    [SerializeField]
    private TextMeshProUGUI _alarmed;
    [SerializeField]
    private TextMeshProUGUI _winCondition;
    [SerializeField]
    private TextMeshProUGUI _tips;
    [SerializeField]
    private TextMeshProUGUI _ending;

    [SerializeField] private float _messageDuration = 4f;

    private TextMeshProUGUI[] _tutorialMessages;
    private int _currentMessageIndex = 0;
    private float _timer = 0f;
    private bool _showingTutorial = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controls.onClick.AddListener(OnControlsButtonClicked);

        _tutorialMessages = new TextMeshProUGUI[]
        {
            _welcome, _goal, _carefull, _sneakKill, _spotted, _duel, _winAndLose, _alarmed, _winCondition, _tips, _ending
        };

        for (int i = 0; i < _tutorialMessages.Length; i++)
        {
            _tutorialMessages[i].gameObject.SetActive(i == 0);
        }

        _currentMessageIndex = 0;
        _timer = 0f;
        _showingTutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        _kunaiNumber.text = _playerMovementScript.Kunais.ToString();
        _smokeBombNumber.text = _playerMovementScript.SmokeBombs.ToString();
        if (_playerMovementScript.Health == 3)
        {
            Heart1.gameObject.SetActive(true);
            Heart2.gameObject.SetActive(true);
            Heart3.gameObject.SetActive(true);
        }
        else if (_playerMovementScript.Health == 2)
        {
            Heart1.gameObject.SetActive(true);
            Heart2.gameObject.SetActive(true);
            Heart3.gameObject.SetActive(false);
        }
        else if (_playerMovementScript.Health == 1)
        {
            Heart1.gameObject.SetActive(true);
            Heart2.gameObject.SetActive(false);
            Heart3.gameObject.SetActive(false);
        }

        if (_showingTutorial && _tutorialMessages.Length > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= _messageDuration)
            {
                if (_currentMessageIndex < _tutorialMessages.Length - 1)
                {
                    // Hide current message and show next
                    _tutorialMessages[_currentMessageIndex].gameObject.SetActive(false);
                    _currentMessageIndex++;
                    _tutorialMessages[_currentMessageIndex].gameObject.SetActive(true);
                    _timer = 0f;
                }
                else
                {
                    // Last message: keep it visible and stop the tutorial
                    _showingTutorial = false;
                }
            }

            // Optional: Skip to next message with Enter
            if (Input.GetKeyDown(KeyCode.K))
            {
                _timer = _messageDuration;
            }
        }
    }
    private void OnControlsButtonClicked()
    {
        _controlsMenu.SetActive(!_controlsMenu.activeSelf);
    }
}