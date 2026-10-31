using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsMethods : MonoBehaviour
{
    private PlayerAttack _playerAttack;

    public void Initialize(PlayerAttack attack)
    {
        _playerAttack = attack;
    }

    public void Attack()
    {
        if (_playerAttack == null)
        {
            _playerAttack = FindAnyObjectByType<PlayerAttack>();
        }

        _playerAttack?.CardAttack();
    }
}
