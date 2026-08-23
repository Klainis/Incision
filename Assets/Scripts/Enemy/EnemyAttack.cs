using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Касание игрока");
    //    if (other.gameObject.TryGetComponent(out IDamageable damageable) 
    //        && other.CompareTag("Player")
    //        && _isAttacking)
    //    {
    //        damageable.ApplyDamage(_damage);
    //    }
    //}

    private void Update()
    {
        
    }

    public void Attack()
    {
        _animator.SetTrigger("IsAttacking");

        StartCoroutine(AttackDash(_attackDistance, _dashDuration));

        //Collider[]_hits = Physics.OverlapSphere(transform.position, 10, _playerLayer);

        //foreach (var hit in _hits)
        //{
        //    if (hit.gameObject.TryGetComponent(out IDamageable damageable)
        //        && hit.CompareTag("Player")
        //        && _isAttacking)
        //    {
        //        damageable.ApplyDamage(_damage);
        //    }
        //}
    }

    private IEnumerator AttackDash(float distance, float duration)
    {
        Vector3 start = transform.position;
        Vector3 target = start + transform.forward * distance;

        float time = 0f;

        _isAttacking = true;

        //Collider[] _hits = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z + 0.8f), 2, _playerLayer);

        //foreach (var hit in _hits)
        //{
        //    if (hit.gameObject.TryGetComponent(out IDamageable damageable)
        //        && hit.CompareTag("Player")
        //        && _isAttacking)
        //    {
        //        damageable.ApplyDamage(_damage);
        //    }
        //}

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
