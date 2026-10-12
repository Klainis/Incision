using System.Collections;
using System.Collections.Generic;
using Incision.Enemies.Interfaces;
using UnityEngine;

public class SlamAttack : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private Transform _slamOverlapPoint;

    public float hitHeight = 6;
    public float heightSpeed = 1;
    public float slamDamage = 10;

    private PlayerStats _playerStatsData;

    private float _slamRadius;

    private void OnEnable() => _playerCharacter.SlamLamded += DoSlamAttack;

    private void Start()
    {
        if (PlayerInput.Instance.PlayerStatsData != null)
        {
            _playerStatsData = PlayerInput.Instance.PlayerStatsData;

            _slamRadius = _playerStatsData.SlamOverlapRadius;
        }
        else
        {
            Debug.LogError($"[SlamAttack.cs] : �� ������� ���������� PlayerStatsData");
        }

        if (_playerCharacter == null)
        {
            _playerCharacter = GetComponent<PlayerCharacter>();

            if (_playerCharacter == null )
            {
                Debug.LogError($"[SlamAttack.cs] : �� ������� ����� PlayerCharacter");
            }
        }
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
