using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;

    private EnemyAttack _enemyAttack;
    private NavMeshAgent _agent;

    private GameObject _player;

    private float _currentCoolDown = 0f;

    private bool _oldAttack = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    void Start()
    {
        _player = FindAnyObjectByType<PlayerCharacter>().gameObject;

        _agent.SetDestination(_player.transform.position);
    }

    void Update()
    {
        if (_oldAttack)
        {
            _currentCoolDown -= Time.deltaTime;
        }

        if (_currentCoolDown <= 0)
        {
            _oldAttack = false;
        }

        float playerDest = Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position));

        if (playerDest <= _enemyAttack.AttackRange && _currentCoolDown <= 0)
        {
            _currentCoolDown = _enemyAttack.AttackCoolDown;
            _oldAttack = true;

            _agent.isStopped = _oldAttack;
            transform.transform.LookAt(_player.transform.position);
;
            _enemyAnimator.SetBool("IsRunning", false);
            _enemyAttack.Attack();
        }

        if (!_oldAttack)
        {
            _agent.isStopped = _oldAttack;

            _enemyAnimator.SetBool("IsRunning", true);
            _agent.SetDestination(_player.transform.position);
        }
    }
}
