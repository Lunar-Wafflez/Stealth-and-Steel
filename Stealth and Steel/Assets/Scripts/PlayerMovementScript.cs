using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    public GameObject _cameraRoot;
    public float _cameraSensitivity = 0.7f;
    public Quaternion _cameraRotation;

    [SerializeField]
    private float _speed = 7f;
    private float _speedMultiplier = 1f;
    public int Health = 3;

    private CapsuleCollider _body;
    public GameObject _mesh;
    private Vector3 _velocity = new Vector3();
    private CharacterController _characterController;


    // Duel Script Ref
    [SerializeField]
    private DuelScript _duelScript;
    public bool _isInDuel = false;

    // Jozua's part 
    public int SmokeBombs = 0;
    public int Kunais = 0;
    public bool IsHidden = false;

    [SerializeField]
    private float _kunaiDistance = 15;
    [SerializeField]
    private float _kunaiForce = 15f;

    [SerializeField]
    private LayerMask _enemyLayerMask;

    [SerializeField]
    private LayerMask _groundLayerMask;

    [SerializeField]
    private float _smokeBombDuration = 5f;

    [SerializeField]
    private Material _normal, _hidden, _offTarget, _onTarget;

    [SerializeField]
    private float _sneakAttackRadius;

    [SerializeField]
    private GameObject _kunaiPrefab;

    private Renderer _renderer;

    private float _smokeBombTimer = 0f;

    private bool _smokeBombActive = false;

    private LineRenderer _lineRenderer;

    private Ray _ray;
    //Temporary untill actual model
    private GameObject _tempChildMesh;
    private Renderer _tempChildMeshRenderer;


    void Start()
    {
        IsHidden = false;
        _cameraRotation = _cameraRoot.transform.rotation;
        _characterController = GetComponent<CharacterController>();
        _body = GetComponent<CapsuleCollider>();
        _renderer = _mesh.GetComponent<Renderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _tempChildMesh = _mesh.transform.GetChild(0).gameObject;
        _tempChildMeshRenderer = _tempChildMesh.GetComponent<Renderer>();
    }


    void Update()
    {
        UpdateInput();
        UpdateMovement();
        Aim();
        //Debug.Log(IsHidden);

        if (_smokeBombActive)
        {
            Debug.Log("Smoke Bomb Active");
            if (_smokeBombTimer < _smokeBombDuration)
            {
                _smokeBombTimer += Time.deltaTime;
                Debug.Log("Smoke Bomb Timer: " + _smokeBombTimer);
            }
            else
            {
                IsHidden = false;
                _renderer.material = _normal;
                _tempChildMeshRenderer.material = _normal;
                _speedMultiplier = 1.0f;
                Debug.Log("Smoke Bomb Duration Ended");
                _smokeBombActive = false;
            }
        }
        if (Health <= 0)
        {
            SceneManager.LoadScene("EndScreenLoss");
        }
    }

    // Movement functions

    void UpdateInput()
    {
        // Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Crouch(false);
        }

        // Smoke Bomb
        if (Input.GetKeyDown(KeyCode.Space) && SmokeBombs > 0f && !_isInDuel)
        {
            Debug.Log("Smoke Bomb Activated");
            _renderer.material = _hidden;
            _tempChildMeshRenderer.material = _hidden;
            _speedMultiplier = 1.3f;
            SmokeBombs = Mathf.Max(0, SmokeBombs - 1);
            _smokeBombTimer = 0f;
            IsHidden = true;
            _smokeBombActive = true;
        }

        // Sneak Attack
        if (Input.GetKeyDown(KeyCode.E) && !_isInDuel)
        {
            Debug.Log("Sneak Attack Attempted");
            GameObject enemy = DetectClosest();
            if (enemy != null)
            {
                Vector3 enemyForward = enemy.transform.forward;
                Vector3 toPlayer = (transform.position - enemy.transform.position).normalized;
                float dotProduct = Vector3.Dot(enemyForward, toPlayer);

                if (dotProduct < 0.5f)
                {
                    Debug.Log("Sneak Attack Successful");
                    Destroy(enemy);
                }
                else
                {
                    Debug.Log("Sneak Attack Failed");
                }
            }
            else
            {
                Debug.Log("No enemy in range");
            }

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
            _speedMultiplier = 0.5f;
            _characterController.height = 1;
            _characterController.transform.localPosition -= Vector3.up * 0.5f; //for some reason this does absolutely nothing
            _mesh.transform.localPosition -= Vector3.up * 0.5f;
        }
        else
        {
            _speedMultiplier = 1f;
            _characterController.height = 2;
            _characterController.transform.localPosition += Vector3.up * 0.5f; //this too, even if set to this.transform instead of _characterController.transform, and even if changed to position instead of localPosition
            _mesh.transform.localPosition += Vector3.up * 0.5f;
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
    private GameObject DetectClosest()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _sneakAttackRadius, _enemyLayerMask);
        if (hitColliders.Length > 0)
        {

            GameObject target = hitColliders[0].gameObject;
            float closestDistanc = _sneakAttackRadius;
            foreach (Collider hitCollider in hitColliders)
            {
                Vector3 distance = hitCollider.transform.position - transform.position;
                if (distance.magnitude < closestDistanc)
                {
                    closestDistanc = distance.magnitude;
                    target = hitCollider.gameObject;
                }
            }
            return target;
        }
        else
        {
            return null;
        }


    }
    private (bool succes, Vector3 position) GetMousePosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayerMask))
        {
            return (true, hit.point);
        }
        else
        {
            return (false, Vector3.zero);
        }
    }
    private void Aim()
    { 
        var (succes, position) = GetMousePosition();    
        if(succes)
        {
            Vector3 direction = position - transform.position;
            direction.y = 0;
            direction.Normalize();
            Debug.DrawRay(transform.position, direction * _kunaiDistance, Color.green);
            if (Input.GetKeyDown(KeyCode.F) && Kunais > 0)
           {
                Kunais = Mathf.Max(0, Kunais - 1);
                Quaternion rotation = Quaternion.LookRotation(direction);
                GameObject kunai = Instantiate(_kunaiPrefab, transform.position, rotation);

                Rigidbody kunaiRb = kunai.GetComponent<Rigidbody>();
                kunaiRb.AddForce(direction * _kunaiForce, ForceMode.Impulse);

            }

        }
    }

}


