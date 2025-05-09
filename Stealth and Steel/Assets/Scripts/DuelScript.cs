using UnityEngine;
using System.Collections;
using System.Threading;


public class DuelScript : MonoBehaviour
{
    public GameObject _player;
    public GameObject _enemy;
    public playerscript _playerScript;                //<----------------- add reference to the player movement script here and include a isInDuel bool in said script
    public EnemyScript _enemyScript;

    private Vector3 _camPos;
    private Quaternion _camInitialRot;

    private bool _duel = false;
    private bool _strike = false;
    private bool _win = false;
    private float _duelTiming = 1f;
    [SerializeField]
    private float _duelMinWaitTime = 1.5f;
    [SerializeField]
    private float _duelMaxWaitTime = 3f;
    [SerializeField]
    private float _duelTimingWindow = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetVars();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) & _strike)
        {
            _win = true;
        }
    }

    // Duel Methods/Functions
    public void BeginDuel(GameObject player, GameObject enemy, EnemyScript enemyScript)
    {
        _player = player;
        _enemy = enemy;
        _enemyScript = enemyScript;

        if (_player == null | _enemy == null)
        {
            Debug.Log("Duel failed to begin, Player is " + _player + " and Enemy is " + _enemy);
        }
        else
        {
            _duel = true;
            _enemyScript._isInDuel = true;
            _playerScript = _player.GetComponent<playerscript>();
            _playerScript._isInDuel = true;
            Debug.Log("The duel has begun between " + _player + " and " + _enemy);

            DuelPosition();
            CameraToDuel();
        }
    }

    private void DuelTypeSelector()
    {
        DuelType1();
    }

    private void DuelType1()
    {
        _duelTiming = Random.Range(_duelMinWaitTime, _duelMaxWaitTime);
        Debug.Log(_duelTiming);

        StartCoroutine(DuelTimer());
    }

    private void ResetVars()
    {
        _duel = false;
        _duelTiming = 1f;
        _win = false;
    }

    private void DuelPosition()
    {
        _camPos = _playerScript._cameraRoot.transform.position;
        _player.transform.position = (_enemy.transform.position + (_enemy.transform.forward * 5f));
        Quaternion temprot = _playerScript._cameraRoot.transform.rotation;
        _player.transform.rotation = _enemy.transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
        _playerScript._cameraRoot.transform.rotation = temprot;
        _playerScript._cameraRoot.transform.position = _camPos;
    }

    private void CameraToDuel()
    {
        Vector3 PlayerPos = _player.transform.position;
        Vector3 EnemyPos = _enemy.transform.position;

        Vector3 CamPosition = Vector3.Lerp(PlayerPos, EnemyPos, 0.5f);
        _camInitialRot = _playerScript._cameraRoot.transform.rotation;

        StartCoroutine(CamLerp(_camPos, CamPosition, _camInitialRot, Quaternion.identity, 0.5f));
    }

    private void EndDuel()
    {
        Vector3 CamPosition = _player.transform.position;
        _duel = false;

        StartCoroutine(CamLerp(_camPos, CamPosition, _playerScript._cameraRoot.transform.rotation, _camInitialRot, 0.5f));

        _playerScript._isInDuel = false;
        ResetVars();
    }

    // Timers

    IEnumerator DuelTimer()
    {
        Debug.Log("ready...");
        for (float x = 0f; x <= _duelTiming + _duelTimingWindow; x += Time.deltaTime)
        {
            if (x >= _duelTiming)
            {
                Debug.Log("strike!");
                _strike = true;
            }

            yield return new WaitForFixedUpdate();
        }

        if (_win)
        {
            Debug.Log("Player won the duel");
            EndDuel();
        }
        else
        {
            _strike = false;
            _playerScript._health -= 1;
            Debug.Log("Player lost the duel");

            // add alarm trigger logic here
        }
        StopCoroutine("DuelTimer");
    }

    IEnumerator CamLerp(Vector3 InitialCamPos, Vector3 TargetCamPos, Quaternion InitialCamRot, Quaternion TargetCamRot, float TimerDuration)
    {
        Vector3 LerpedPos = Vector3.zero;
        Quaternion LerpedRot = Quaternion.identity;

        for (float x = 0; x <= TimerDuration; x += Time.deltaTime)
        {
            LerpedPos = Vector3.Lerp(InitialCamPos, TargetCamPos, x / TimerDuration);
            LerpedRot = Quaternion.Lerp(InitialCamRot, TargetCamRot, x / TimerDuration);
            _playerScript._cameraRoot.transform.position = LerpedPos;
            _playerScript._cameraRoot.transform.rotation = LerpedRot;

            yield return new WaitForFixedUpdate();
        }
        if (_duel)
        {
            DuelTypeSelector();
        }
        StopCoroutine("CamLerp");
    }
}
