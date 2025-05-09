using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMechanics : MonoBehaviour
{
       
    public int SmokeBombs = 0;
    public int Kunais = 0;
    public bool IsHidden = false;

    [SerializeField]
    private float _velocity = 5;

    [SerializeField]
    private float _kunaiDistance = 15;

    [SerializeField]
    private LayerMask _enemyLayerMask;

    [SerializeField]
    private float _smokeBombDuration = 5f;

    [SerializeField]
    private Material _normal, hidden, _offTarget, _onTarget;
    

    private NavMeshAgent _playerNavMesh;

    private Renderer _renderer;

    private float _smokeBombTimer = 0f;

    private bool _smokeBombActive = false, _kunaiActive = false;  

    private LineRenderer _lineRenderer;

    private Ray _ray;







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerNavMesh = GetComponent<NavMeshAgent>();
        _renderer = GetComponent<Renderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }
     
    // Update is called once per frame
    void Update()
    {
        // Kunai Throw
        Plane groundPlane = new Plane(Vector3.up, 2f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mousePosOnPlane = ray.GetPoint(enter);
            Vector3 rayDirection = (mousePosOnPlane - transform.position).normalized;
            _ray = new Ray(transform.position, rayDirection);

            if (Input.GetKey(KeyCode.F) && Kunais > 0f)
            {
                Debug.Log("HOLDING F");
                _lineRenderer.enabled = true;
                if (Physics.Raycast(_ray, out RaycastHit hit, _kunaiDistance, _enemyLayerMask))
                {
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, hit.point);
                    _lineRenderer.material = _onTarget;
                }
                else
                {
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, _ray.GetPoint(_kunaiDistance));
                    _lineRenderer.material = _offTarget;    
                }
                _kunaiActive = true;

            }
            if (Input.GetKeyUp(KeyCode.F) && _kunaiActive)
            {
                Kunais = Mathf.Max(0, Kunais - 1);
                if (Physics.Raycast(_ray, out RaycastHit hit, _kunaiDistance))
                {
                    if (((1 << hit.collider.gameObject.layer) & _enemyLayerMask) != 0)
                    {
                        Destroy(hit.collider.gameObject);

                    }
                }
                _lineRenderer.enabled = false;
                _kunaiActive = false;

            }
        }
        
        // Temporary Movement
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _playerNavMesh.SetDestination(hit.point);
                _playerNavMesh.speed = _velocity;
            }
        }
        // Sneak Attack
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (Physics.SphereCast(transform.position, _sneakAttackRadius, )
        //    {
        //    }
        //}

        // SmokeBomb Logic
        if (Input.GetKeyDown(KeyCode.Space) && SmokeBombs > 0f)
        {
            _renderer.material = hidden;
            SmokeBombs = Mathf.Max(0, SmokeBombs - 1);
            _smokeBombTimer = 0f;
            IsHidden = true;
            _smokeBombActive = true;
            Debug.Log("Smoke Bomb Activated");
        }
        if (_smokeBombActive)
        {
            if (_smokeBombTimer < _smokeBombDuration)
            {
                _smokeBombTimer += Time.deltaTime;
                Debug.Log("Smoke Bomb Timer: " + _smokeBombTimer);
            }
            else
            {
                IsHidden = false;
                _renderer.material = _normal;
                Debug.Log("Smoke Bomb Duration Ended");
                _smokeBombActive = false;
            }
        }

    }
}
