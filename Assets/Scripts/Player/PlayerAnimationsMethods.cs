using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsMethods : MonoBehaviour
{
    public static PlayerAnimationsMethods Instance { get; private set; }

    [SerializeField] private GunAttack _gun;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_gun == null)
        {
            _gun = FindAnyObjectByType<GunAttack>();
        }
    }

    public void Attack()
    {
        _gun?.Attack();
    }
}
