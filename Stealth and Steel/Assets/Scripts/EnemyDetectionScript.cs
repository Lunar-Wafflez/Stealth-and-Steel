using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    private GameObject _player;
    private DuelScript _duelScript;

    public bool _isInDuel = false;
    public EnemyControlLogic _enemyControlLogic;

    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.Log(_player);
        }
        _duelScript = GameObject.Find("DuelSystem").GetComponent<DuelScript>();
        if (_duelScript == null)
        {
            Debug.Log(_duelScript);
        }
        _enemyControlLogic = GetComponent<EnemyControlLogic>();
    }

    void Update()
    {
        DetectionUpdate();
    }

    private void DetectionUpdate()
    {
        if (Vector3.Dot((_player.transform.position - transform.position).normalized, transform.forward) > 0.8f)
        {
            Physics.Raycast(transform.position, _player.transform.position - transform.position, out RaycastHit hit, 10);

            if (hit.collider == _player.GetComponent<CapsuleCollider>())
            {
                if (!_isInDuel & hit.distance < 10f)
                {
                    _duelScript.BeginDuel(_player, this.gameObject, this, _enemyControlLogic);
                    _isInDuel = true;
                    Debug.Log("detected " + hit.collider);
                }
            }
        }
    }
}
