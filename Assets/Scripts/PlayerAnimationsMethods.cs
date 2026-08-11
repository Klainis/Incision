using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsMethods : MonoBehaviour
{
    public static PlayerAnimationsMethods Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void Attack()
    {
        var gun = FindAnyObjectByType<GunAttack>();
        gun?.Attack();
    }
}
