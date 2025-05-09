using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
       
    public int SmokeBombs = 0;
    public int Kunais = 0;
    public bool IsHidden = false;

    [SerializeField]
    private float _velocity = 5;

    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private float _kunaiDistance = 25;

    [SerializeField]
    private LayerMask _enemyLayerMask;

    [SerializeField]
    private float _smokeBombDuration = 5f;

    [SerializeField]
    private Material _normal, hidden;
    

    private NavMeshAgent _playerNavMesh;

    private Renderer _renderer;

    private float _smokeBombTimer = 0f;

    private bool _smokeBombActive = false;  







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerNavMesh = GetComponent<NavMeshAgent>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary movement
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _playerNavMesh.SetDestination(hit.point);
                _playerNavMesh.speed = _velocity;
            }
        }
        // Temporary Kunai Throw
        if (Input.GetKeyDown(KeyCode.F) && Kunais > 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (((1 << hit.collider.gameObject.layer) & _enemyLayerMask) != 0)
                {
                    Debug.Log("You killed the enemy");
                    Destroy(hit.collider.gameObject);
                    Kunais -= 1;
                }
            }


        }
        // SmokeBomb Logic
        if (Input.GetKeyDown(KeyCode.Space) && SmokeBombs > 0f)
        {
            _renderer.material = hidden;
            SmokeBombs -= 1;
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
