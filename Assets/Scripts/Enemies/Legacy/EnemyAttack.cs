using System;
using System.Collections;
using UnityEngine;

[Obsolete("Use IEnemyAttack implementations instead")]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LayerMask _playerLayer;

    [Space(5)]
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackCoolDown = 2.5f;
    [SerializeField] private float _attackDistance = 7f;
    [SerializeField] private float _dashDuration = 0.5f;
    [SerializeField] private float _attackRange = 6f;

    private bool _isAttacking = false;

    //private Collider[] _hits;

    public float AttackCoolDown => _attackCoolDown;
    public float AttackRange => _attackRange;
    public float Damage => _damage;
    public LayerMask Playerlayer => _playerLayer;

    public void Attack()
    {
        _animator.SetTrigger("IsAttacking");

        StartCoroutine(AttackDash(_attackDistance, _dashDuration));
    }

    private IEnumerator AttackDash(float distance, float duration)
    {
        Vector3 start = transform.position;
        Vector3 target = start + transform.forward * distance;

        float time = 0f;

        _isAttacking = true;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        _isAttacking = false;
    }
}
