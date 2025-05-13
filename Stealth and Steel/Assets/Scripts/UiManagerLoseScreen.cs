using UnityEngine;
using UnityEngine.UI;

public class UiManagerLoseScreen : MonoBehaviour
{
    [SerializeField]
    private Button _retryButton;
    [SerializeField]
    private Button _exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnRetryButtonClicked()
    {
        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
