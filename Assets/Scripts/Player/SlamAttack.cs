using Incision.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class SlamAttack : MonoBehaviour
{
    [SerializeField] private Transform _slamOverlapPoint;

    private PlayerCharacter _playerCharacter;
    private PlayerStats _playerStatsData;
    private List<StatusEffectTemplate> _statusEffects;

    public float hitHeight = 6;
    public float heightSpeed = 1;
    public float slamDamage = 10;

    private float _slamRadius;

    public void Initialize(PlayerCharacter character, PlayerStats stats)
    {
        _playerCharacter = character;
        _playerStatsData = stats;
        _statusEffects = stats?.StatusEffects;

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
            //TakeDamage(hit.gameObject);
            ApplyStun(hit.gameObject);
        }
    }

    private void TakeDamage(GameObject hit)
    {
        if (hit.gameObject.TryGetComponent(out IDamageable damageable) &&
                hit.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            damageable.TakeDamage(new(slamDamage, gameObject, hit.gameObject));
        }
    }

    private void ApplyStun(GameObject hit)
    {
        if (hit.gameObject.TryGetComponent(out IStatusReceiver statusReceiver) &&
                hit.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            foreach (StatusEffectTemplate statusTemplate in _statusEffects)
                statusReceiver.TryApplyStatus(statusTemplate, gameObject);
        }
    }
}
