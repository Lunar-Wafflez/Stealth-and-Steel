using UnityEngine;

public class BushHidingLogic : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementScript _playerMovementScript;
    [SerializeField]
    private LayerMask _layerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovementScript.IsHidden = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_playerMovementScript.IsHidden);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (1 << (other.gameObject.layer & _layerMask) != 0)
        {
            // Hide the player
            _playerMovementScript.IsHidden = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (1 << (other.gameObject.layer & _layerMask) != 0)
        {
            // Show the player
            _playerMovementScript.IsHidden = false;
        }
    }
}
