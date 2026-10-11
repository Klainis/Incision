using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }

    [SerializeField] private Animator _playerAnimator;
    //[SerializeField] private Animator _cameraAnimator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void ThrowCardTriggerAnimation()
    {
        //_playerAnimator?.SetBool("IsGrounded", true);
        _playerAnimator?.SetTrigger("IsThrow");
    }

    public void SwapCardAnimation()
    {
        _playerAnimator?.SetTrigger("SwapCard");
    }

    public void RunAnimation(float speed)
    {
        _playerAnimator?.SetFloat("Speed", speed);
    }

    public void FallAnimation(bool isFalling)
    {
        _playerAnimator?.SetBool("IsGrounded", !isFalling);
    }

    public void DashAnimation()
    {
        Debug.Log("Dash");
        _playerAnimator?.SetTrigger("IsDashing");
    }
}
