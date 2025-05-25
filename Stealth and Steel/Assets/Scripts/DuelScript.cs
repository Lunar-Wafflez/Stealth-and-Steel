using UnityEngine;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;


public class DuelScript : MonoBehaviour
{
    public GameObject _player;
    public GameObject _enemy;
    public PlayerMovementScript _playerScript;                //<----------------- add reference to the player movement script here and include a isInDuel bool in said script
    public EnemyDetectionScript _enemyScript;
    public EnemyControlLogic _enemyControlLogic;
    public GameObject _level1;
    [SerializeField]
    private float _alarmDistance = 10f;

    private Vector3 _camPos;
    private Quaternion _camInitialRot;

    private bool _duel = false;
    private bool _strike = false;
    private bool _win = false;
    private bool _fumble = false;
    private float _duelTiming = 1f;
    [SerializeField]
    private float _duelMinWaitTime = 1.5f;
    [SerializeField]
    private float _duelMaxWaitTime = 3f;
    [SerializeField]
    private float _duelTimingWindow = 0.5f;
    [SerializeField]
    private TMP_Text _duelText;

    // Audio
    [SerializeField] private AudioClip readySound;
    [SerializeField] private AudioClip strikeSound;
    private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSystem();

        // Initialising sound
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) & _strike)
        {
            if (_fumble) return;
            _win = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) & !_fumble)
        {
            _fumble = true;
        }
    }

    // Duel Methods/Functions
    public void BeginDuel(GameObject player, GameObject enemy, EnemyDetectionScript enemyScript, EnemyControlLogic enemyControl)
    {
        _player = player;
        _enemy = enemy;
        _enemyScript = enemyScript;
        _enemyControlLogic = enemyControl;

        if (_player == null | _enemy == null)
        {
            Debug.Log("Duel failed to begin, Player is " + _player + " and Enemy is " + _enemy);
        }
        else
        {
            _duel = true;
            _level1.SetActive(false);
            _enemyScript._isInDuel = true;
            _enemyControlLogic.IsInDuel = true;
            _playerScript = _player.GetComponent<PlayerMovementScript>();
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

    private void ResetSystem()
    {
        _duelText.enabled = false;
        _duel = false;
        _duelTiming = 1f;
        _win = false;
        _fumble = false;
    }

    private void DuelPosition()
    {
        _camPos = _playerScript._cameraRoot.transform.position;
        _player.transform.position = (_enemy.transform.position + (_enemy.transform.forward * 5f));
        Quaternion temprot = _playerScript._cameraRoot.transform.rotation;
        _playerScript._mesh.transform.rotation = _enemy.transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
        _playerScript._cameraRoot.transform.rotation = temprot;
        _playerScript._cameraRoot.transform.position = _camPos;
    }

    private void CameraToDuel()
    {
        Vector3 PlayerPos = _player.transform.position;
        Vector3 EnemyPos = _enemy.transform.position;

        Vector3 CamPosition = Vector3.Lerp(PlayerPos, EnemyPos, 0.5f);
        _camInitialRot = _playerScript._cameraRotation;

        StartCoroutine(CamLerp(_camPos, CamPosition, _camInitialRot, Quaternion.Euler(0, Mathf.Atan2(_enemy.transform.right.x, _enemy.transform.right.z) * Mathf.Rad2Deg,0), 0.5f));
    }

    private void EndDuel(bool isDuelWon)
    {
        Vector3 CamPosition = _player.transform.position;
        _duel = false;
        _level1.SetActive(true);

        if (!isDuelWon)
        {
            _enemyScript._isInDuel = false;
            _enemyControlLogic.IsInDuel = false;
        }
        else Destroy(_enemy);

        StartCoroutine(CamLerp(_camPos, CamPosition, _playerScript._cameraRoot.transform.rotation, _camInitialRot, 0.5f));

        _playerScript._isInDuel = false;
        ResetSystem();
    }

    private void AlertNearbyEnemies()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _alarmDistance);

        if (targets.Length > 0)
        {
            foreach (Collider target in targets)
            {
                if (target.GetComponent<EnemyControlLogic>() != null)
                {
                    target.GetComponent<EnemyControlLogic>().Alarmed = true;
                }
            }
        }
    }

    // Timers

    IEnumerator DuelTimer()
    {
        _duelText.enabled = true;
        Debug.Log("ready...");
        _duelText.text = "Ready...";

        // ▶️ Звук "Ready"
        if (audioSource != null && readySound != null)
            audioSource.PlayOneShot(readySound);

        //for (float x = 0f; x <= _duelTiming + _duelTimingWindow; x += Time.deltaTime)
        //{
        //    if (x >= _duelTiming)
        //    {
        //        Debug.Log("strike!");
        //        _duelText.text = "Strike!";
        //        _strike = true;
        //    }

        //    yield return new WaitForFixedUpdate();
        //}

        bool hasShownStrike = false;

        for (float x = 0f; x <= _duelTiming + _duelTimingWindow; x += Time.deltaTime)
        {
            if (!hasShownStrike && x >= _duelTiming)
            {
                Debug.Log("strike!");
                _duelText.text = "Strike!";
                _strike = true;

                if (audioSource != null && strikeSound != null)
                    audioSource.PlayOneShot(strikeSound);

                hasShownStrike = true;
            }

            yield return new WaitForFixedUpdate();
        }

        if (_win)
        {
            if (_enemy.gameObject.name == "Boss")
            {
                Application.Quit();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            Debug.Log("Player won the duel");
            EndDuel(true);

        }
        else
        {
            _strike = false;
            _playerScript._health -= 1;
            Debug.Log("Player lost the duel");

            AlertNearbyEnemies();
            EndDuel(false);
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
