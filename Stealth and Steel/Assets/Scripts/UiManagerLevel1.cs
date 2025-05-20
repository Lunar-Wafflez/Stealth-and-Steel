using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerLevel1 : MonoBehaviour
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }
}
