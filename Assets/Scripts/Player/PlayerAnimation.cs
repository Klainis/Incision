using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [Inject] private IEventBus _eventBus;

    public void ThrowCardTriggerAnimation()
    {
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
        _eventBus.Publish(new Log("[PlayerAnim]".AsTag(Color.blue),"Dash"));
        _playerAnimator?.SetTrigger("IsDashing");
    }
}
