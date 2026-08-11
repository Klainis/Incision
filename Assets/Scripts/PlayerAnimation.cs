using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _cameraAnimator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void StartThrowCard()
    {
        _playerAnimator?.SetTrigger("IsThrow");
    }

    //public void EndThrowCard()
    //{
    //    _playerAnimator?.SetBool("IsThrow", false);
    //}

    public void StartRun()
    {
        _cameraAnimator?.SetBool("IsRun", true);
    }

    public void EndRun()
    {
        _cameraAnimator?.SetBool("IsRun", false);
    }

    public void StartFall()
    {
        _cameraAnimator?.SetBool("IsRun", false);
        _playerAnimator?.SetBool("IsFall", true);
    }

    public void EndFall()
    {
        _playerAnimator?.SetBool("IsFall", false);
    }
}
