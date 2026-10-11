using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
   [SerializeField] private CardHand _hand;

    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;

    [Header("UI Crosshair (RectTransform)")]
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private Transform _weaponMuzzle;

    [Header("Force Settings")]
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
    [SerializeField] private float _force = 20f;

    [Header("Range")]
    [SerializeField] private float _maxRange = 500f;

    public void CardAttack()
    {
        if (!_hand.TryConsumeShot(out CardData card, out float damage, out bool isCrit))
            return;

        Vector3 direction = CalculateAttackDirection();

        Projectile projectile = Instantiate(
            card.CardPrefab,
            _weaponMuzzle.position,
            Quaternion.LookRotation(direction));

        projectile.Initialize(damage, isCrit);
        projectile.Rigidbody.AddForce(direction * _force, _forceMode);
    }

    //public void AbilityAttack()
    //{
    //    if (!_hand.TryConsumeShot(out CardData card, out float damage, out bool isCrit))
    //        return;
    //}

    private Vector3 CalculateAttackDirection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, _maxRange)
            ? hit.point
            : ray.GetPoint(_maxRange);

        return (targetPoint - _weaponMuzzle.position).normalized;
    }
}
