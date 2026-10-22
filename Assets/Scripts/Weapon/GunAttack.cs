using Incision;
using Incision.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GunAttack : MonoBehaviour
{
    [SerializeField] private CardHand _hand;

    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;

    [Header("UI Crosshair")]
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private Transform _weaponMuzzle;

    [Header("Force Settings")]
    [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;

    [Header("Range")]
    [SerializeField] private float _maxRange = 500f;

    [Inject] private ProjectileFactory _projectileFactory;
    [Inject] private IEventBus _eventBus;

    public void CardAttack()
    {
        if (!_hand.TryConsumeShot(out CardData cardData, out float damage, out bool isCrit))
            return;

        Vector3 direction = CalculateAttackDirection();

        var projectile = SpawnProjectile(cardData, damage, isCrit, direction);
        projectile.Rigidbody.AddForce(direction * cardData.ProjectileSpeed, _forceMode);
    }

    //public void AbilityAttack()
    //{
    //    if (!_hand.TryConsumeShot(out CardData cardData, out float damage, out bool isCrit))
    //        return;
    //}

    public Projectile SpawnProjectile(CardData cardData, float damage, bool isCrit, Vector3 direction)
    {
        var position = _weaponMuzzle.position;
        var rotation = Quaternion.LookRotation(direction);

        CardProjectile projectile = _projectileFactory.CreateCardProjectile(cardData, damage, isCrit, position, rotation);

        return projectile;
    }

    private Vector3 CalculateAttackDirection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, _maxRange)
            ? hit.point
            : ray.GetPoint(_maxRange);

        return (targetPoint - _weaponMuzzle.position).normalized;
    }
}
