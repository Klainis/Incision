using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;

    [Header("UI Crosshair (RectTransform)")]
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private Transform _weaponMuzzle;
    [SerializeField] private Projectile _projectilePrefab;

    [Header("Force Setiings")]
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    [SerializeField] private float _force = 20f;

    [Header("Range")]
    [SerializeField] private float _maxRange = 500f;

    public void Attack()
    {
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRange))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(_maxRange);
        }

        //Debug.Log(targetPoint);

        var projectile = Instantiate(_projectilePrefab, _weaponMuzzle.position, _weaponMuzzle.rotation);

        Vector3 direction = (targetPoint - _weaponMuzzle.position).normalized;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Rigidbody.AddForce(direction * _force, _forceMode);

        //Debug.DrawRay(_weaponMuzzle.position, direction * 50f, Color.green, 2f);
        //Debug.DrawLine(_weaponMuzzle.position, targetPoint, Color.red, 2f);
    }
}
