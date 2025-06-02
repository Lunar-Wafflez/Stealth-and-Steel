using System.Collections;
using UnityEngine;

public class EnemyDetectionScript : MonoBehaviour
{
    private GameObject _player;
    private DuelScript _duelScript;

    public bool _isInDuel = false;
    private bool _duelCooldown = false;
    public EnemyControlLogic _enemyControlLogic;
    public PlayerMovementScript PlayerMovementScript;
    public float _detectionDistance = 10f;

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
        IsTouching();
    }

    private void DetectionUpdate()
    {
        if (Vector3.Dot((_player.transform.position - transform.position).normalized, transform.forward) > 0.8f)
        {
            Physics.Raycast(transform.position, _player.transform.position - transform.position, out RaycastHit hit, 10);

            if (hit.collider == _player.GetComponent<CapsuleCollider>() && !PlayerMovementScript.IsHidden)
            {
                if (hit.distance > 5f & hit.distance < _detectionDistance)
                {
                    _enemyControlLogic.Alarmed = true;
                }

                if (!_isInDuel & !_duelCooldown & hit.distance < 5f)
                {
                    _duelScript.BeginDuel(_player, this.gameObject, this, _enemyControlLogic);
                    _isInDuel = true;
                    _duelCooldown = true;
                    StartCoroutine(DuelCooldown(7f));
                    //Debug.Log("detected " + hit.collider);
                }
            }
        }
    }

    //Timers/Coroutines

    IEnumerator DuelCooldown(float duration)
    {
        for (float x = 0f; x <= duration; x += Time.deltaTime)
        {
            if (x >= duration * 0.95)
            {
                _duelCooldown = false;
            }
            //Debug.Log(x);
            yield return new WaitForFixedUpdate();
        }
    }
    private void IsTouching()
    {
        Collider enemyCollider = GetComponent<Collider>();
        Collider playerCollider = _player.GetComponent<Collider>();
        if (enemyCollider != null && playerCollider != null)
        {
            if (enemyCollider.bounds.Intersects(playerCollider.bounds))
            {
                _enemyControlLogic.Alarmed = true;
            }
        }
    }
}
