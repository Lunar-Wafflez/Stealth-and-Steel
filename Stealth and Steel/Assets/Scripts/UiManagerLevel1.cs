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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _kunaiNumber.text = _playerMovementScript.Kunais.ToString();
        _smokeBombNumber.text = _playerMovementScript.SmokeBombs.ToString();
    }
}
