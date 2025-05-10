using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMechanics : MonoBehaviour
{
       
    public int SmokeBombs = 0;
    public int Kunais = 0;
    public bool IsHidden = false;

    [SerializeField]
    private float _kunaiDistance = 15;

    [SerializeField]
    private LayerMask _enemyLayerMask;

    [SerializeField]
    private float _smokeBombDuration = 5f;

    [SerializeField]
    private Material _normal, hidden, _offTarget, _onTarget;

    [SerializeField]
    private float _sneakAttackRadius;

    private Renderer _renderer;

    private float _smokeBombTimer = 0f;

    private bool _smokeBombActive = false, _kunaiActive = false;  

    private LineRenderer _lineRenderer;

    private Ray _ray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }
     
    // Update is called once per frame
    void Update()
    {
        // Kunai Throw
        Plane groundPlane = new Plane(Vector3.up, 0f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mousePosOnPlane = ray.GetPoint(enter);
            Vector3 rayDirection = (mousePosOnPlane - transform.position).normalized;
            _ray = new Ray(transform.position, rayDirection);
            Debug.DrawRay(transform.position, rayDirection);
            if (Input.GetKey(KeyCode.F) && Kunais > 0f)
            {
                _lineRenderer.enabled = true;
                _kunaiActive = true;

                if (Physics.Raycast(_ray, out RaycastHit hit, _kunaiDistance))
                {
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, mousePosOnPlane);

                    if (((1 << hit.collider.gameObject.layer) & _enemyLayerMask) != 0)
                    {
                        _lineRenderer.material = _onTarget; // Turn green if on target
                    }
                    else
                    {
                        _lineRenderer.material = _offTarget; // Turn red if not on target
                    }
                }
                else
                {
                    _lineRenderer.SetPosition(0, transform.position);
                    _lineRenderer.SetPosition(1, mousePosOnPlane);
                    _lineRenderer.material = _offTarget; // Turn red if not on target
                }


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
    
        //Sneak Attack
        if (Input.GetKeyDown(KeyCode.E))    
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
}
