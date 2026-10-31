using System.Collections;
using System.Collections.Generic;
using Incision.Interfaces;
using UnityEngine;

public class SlamAttack : MonoBehaviour
{
    [SerializeField] private Transform _slamOverlapPoint;

    private PlayerCharacter _playerCharacter;
    private PlayerStats _playerStatsData;

    public float hitHeight = 6;
    public float heightSpeed = 1;
    public float slamDamage = 10;

    private float _slamRadius;

    public void Initialize(PlayerCharacter character, PlayerStats stats)
    {
        _playerCharacter = character;
        _playerStatsData = stats;

        _slamRadius = _playerStatsData.SlamOverlapRadius;

        _playerCharacter.SlamLamded += DoSlamAttack;
    }

    private void OnDisable()
    {
        _playerCharacter.SlamLamded -= DoSlamAttack;
    }

    private void DoSlamAttack(float slamHeight)
    {
        Collider[] _hits = Physics.OverlapSphere(_slamOverlapPoint.position, _slamRadius);

        foreach (var hit in _hits)
        {
            if (hit.gameObject.TryGetComponent(out IDamageable damageable) &&
                hit.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                damageable.TakeDamage(new (slamDamage, gameObject, hit.transform.position));

                Vector3 hitPos = hit.gameObject.transform.position;
                hitPos = Vector3.MoveTowards(hitPos, hitPos + Vector3.up * hitHeight, heightSpeed * Time.deltaTime);
            }
        }
    }
}
