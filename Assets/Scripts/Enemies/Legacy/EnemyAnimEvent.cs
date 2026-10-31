using System;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using UnityEngine;

[Obsolete("Use EnemyAnimEvents instead")]
public class EnemyAnimEvent : MonoBehaviour
{
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private Transform _point;
    [SerializeField] private float _overlapRadius = 2;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_point.position, _overlapRadius);
    }

    public void DoDamage()
    {
        Collider[] _hits = Physics.OverlapSphere(_point.position, _overlapRadius, _enemyAttack.Playerlayer);

        foreach (var hit in _hits)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damageable)
                && hit.CompareTag("Player"))
            {
                damageable.TakeDamage(new DamageInfo(_enemyAttack.Damage, gameObject, _point.gameObject));
            }
        }
    }
}
