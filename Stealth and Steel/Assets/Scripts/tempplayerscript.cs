using UnityEngine;
using System.Collections;

public class playerscript : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    public GameObject _cameraRoot;
    public float _cameraSensitivity = 0.7f;
    [SerializeField]
    private Vector3 _cameraRotation = new Vector3();

    [SerializeField]
    private float _speed = 10f;
    private float _speedMultiplier = 1f;
    public int _health = 3;

    private CapsuleCollider _body;
    private Vector3 _velocity = new Vector3();


    // Duel Script Ref
    [SerializeField]
    private DuelScript _duelScript;
    public bool _isInDuel = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _body = GetComponent<CapsuleCollider>();
        if (_body != null)
        {
            Debug.Log(_body);
        }
    }


    void Update()
    {
        //UpdateCamera();
        UpdateMovement();
    }

    // Movement functions
    void UpdateMovement()
    {
        if (!_isInDuel)
        {
            Vector3 MoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 MoveDirection = Quaternion.Euler(0, _cameraRoot.transform.rotation.eulerAngles.y, 0) * MoveInput;

            _velocity = MoveDirection;

            transform.position += _velocity * (_speed * _speedMultiplier) * Time.deltaTime;
        }
    }

    //Camera functions
    void UpdateCamera()
    {
        _cameraRoot.transform.rotation = Quaternion.Euler(_cameraRotation);
    }
}
