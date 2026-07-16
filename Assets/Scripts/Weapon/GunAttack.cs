using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
    [SerializeField] private Transform _weaponMuzzle;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    [SerializeField] private float _force = 20f;

    public void Attack()
    {
        var projectile = Instantiate(_projectilePrefab, _weaponMuzzle.position, _weaponMuzzle.rotation);

        projectile.Rigidbody.AddForce(_weaponMuzzle.forward * _force, _forceMode);
    }
}
