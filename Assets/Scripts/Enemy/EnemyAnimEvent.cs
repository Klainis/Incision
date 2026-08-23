using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private Transform _point;
    [SerializeField] private float _overlapRadius = 2;

    public void DoDamage()
    {
        Collider[] _hits = Physics.OverlapSphere(_point.position, _overlapRadius, _enemyAttack.Playerlayer);

        foreach (var hit in _hits)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damageable)
                && hit.CompareTag("Player"))
            {
                damageable.ApplyDamage(_enemyAttack.Damage);
            }
        }
    }
}
