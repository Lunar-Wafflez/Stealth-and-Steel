using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
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
    public GameObject _mesh;
    private Vector3 _velocity = new Vector3();
    private CharacterController _characterController;


    // Duel Script Ref
    [SerializeField]
    private DuelScript _duelScript;
    public bool _isInDuel = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _body = GetComponent<CapsuleCollider>();
        if (_body != null)
        {
            Debug.Log(_body);
        }
    }


    void Update()
    {
        UpdateInput();
        UpdateMovement();
    }

    // Movement functions

    void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Crouch(false);
        }
    }

    void UpdateMovement()
    {
        if (!_isInDuel)
        {
            Vector3 MoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            Vector3 MoveDirection = Quaternion.Euler(0, _cameraRoot.transform.rotation.eulerAngles.y, 0) * MoveInput;

            _velocity = MoveDirection;


            _characterController.Move(_velocity * (_speed * _speedMultiplier) * Time.deltaTime);
            RotateToMovementDirection(MoveInput);
        }
    }

    void Crouch(bool crouched)
    {
        if (crouched)
        {
            _characterController.height = 1;
            _characterController.transform.localPosition -= Vector3.up * 0.5f; //for some reason this does absolutely nothing
        }
        else
        {
            _characterController.height = 2;
            _characterController.transform.localPosition += Vector3.up * 0.5f; //this too, even if set to this.transform instead of _characterController.transform, and even if changed to position instead of localPosition
        }
    }

    void RotateToMovementDirection(Vector3 input)
    {
        if (input.sqrMagnitude == 0) return;

        float CurrentVelocityMagnitude = _characterController.velocity.magnitude;

        float TargetDirection = Mathf.Atan2(_characterController.velocity.x, _characterController.velocity.z) * Mathf.Rad2Deg;

        float direction = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetDirection, ref CurrentVelocityMagnitude, 0.2f, Time.deltaTime);

        _mesh.transform.rotation = Quaternion.Euler(0, TargetDirection, 0);
    }
}
