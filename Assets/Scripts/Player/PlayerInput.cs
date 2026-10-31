using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Incision.PLayer;
using UnityEngine;
using VContainer;

public class PlayerInput : MonoBehaviour
{
    private PlayerCharacter _playerCharacter;
    private PlayerCamera _playerCamera;
    private CardHand _cardHand;

    private PlayerMovementData _playerMovementData;
    private PlayerStats _playerStatsData;

    private PlayerInputActions _inputActions;

    private float _jumpTimeBuffer = 0;

    public void Intialize(PlayerCharacter character, PlayerCamera camera, CardHand hand, PlayerMovementData moveData, PlayerStats stats)
    {
        _playerCharacter = character;
        _playerCamera = camera;
        _cardHand = hand;

        _playerMovementData = moveData;
        _playerStatsData = stats;

        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    private void OnDestroy()
    {
        _inputActions.Dispose();
    }

    void Update()
    {
        if (_jumpTimeBuffer > 0)
        {
            _jumpTimeBuffer -= Time.deltaTime;
        }

        var playerInput = _inputActions.Player;
        var cardSlotInput = _inputActions.SlotActions;

        var cameraInput = new CameraInput 
        {
            Look = playerInput.Look.ReadValue<Vector2>(),
            Move = playerInput.Move.ReadValue<Vector2>(),
        };
        _playerCamera.UpdateRotation(cameraInput);

        if (playerInput.Jump.WasPressedThisFrame())
        {
            _jumpTimeBuffer = _playerMovementData.JumpBufferTime;
        }

        var characterInput = new CharacterInput
        {
            Rotation = _playerCamera.transform.rotation,
            Move = playerInput.Move.ReadValue<Vector2>(),
            Jump = _jumpTimeBuffer > 0,
            //JumpSustain = playerInput.Jump.IsPressed(),
            Dash = playerInput.Dash.WasPressedThisFrame(),
            Slam = playerInput.Slam.WasPressedThisFrame(),
        };
        _playerCharacter.UpdateInput(characterInput);

        var cardHandInput = new PlayerCardInput
        {
            Shoot = playerInput.Shoot.IsPressed(),
            Ability = playerInput.Ability.WasPressedThisFrame(),
            SwitchScroll = playerInput.SwitchCard.ReadValue<Vector2>(),
            FirstCardSlot = cardSlotInput.ActiveCardSlot1.WasPressedThisFrame(),
            SecondCardSlot = cardSlotInput.ActiveCardSlot2.WasPressedThisFrame(),
        };
        _cardHand.UpdateInput(cardHandInput);
    }

    private void LateUpdate()
    {
        _playerCamera.UpdatePosition(_playerCharacter.GetCameraTarget());
    }
    
    public void ConsumeJumpBuffer()
    {
        _jumpTimeBuffer = 0f;
    }
}
