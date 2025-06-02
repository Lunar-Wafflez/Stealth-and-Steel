using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControlLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private NavMeshAgent _aiNavMesh;
    private bool _isMoving = false;
    private int _nextIndex = 0;
    [SerializeField]
    private Vector3[] _positions;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private PlayerMovementScript _playerMovementScript;
    [SerializeField]
    private float _pauseDuration = 2f;
    private bool _isPaused = false;
    [SerializeField]
    private DuelScript _duelScript;
    [SerializeField]
    private float _runSpeed = 5.5f;


    public bool Alarmed = false;
    public bool IsInDuel = false;
    void Start()
    {
        _aiNavMesh = GetComponent<NavMeshAgent>();

        
              
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovementScript.IsHidden)
        {
            Alarmed = false;
        }
        if (!IsInDuel)
        {
            _aiNavMesh.isStopped = false;
            if (Alarmed && !_playerMovementScript.IsHidden)
            {
                _aiNavMesh.SetDestination(_target.position);
                _aiNavMesh.speed = _runSpeed;
            }
            else
            {
                _aiNavMesh.speed = 2;

                if (!_isMoving)
                {
                    _aiNavMesh.destination = _positions[_nextIndex];
                    _isMoving = true;
                }
                if (_isMoving && !_aiNavMesh.pathPending && _aiNavMesh.remainingDistance <= 1f)
                {
                    _isMoving = false;
                    _nextIndex++;
                    _nextIndex = _nextIndex % _positions.Length;
                    //Debug.Log(_nextIndex);
                }
            }
        }
        else
        {
            _aiNavMesh.isStopped = true;
        }



    }
}
