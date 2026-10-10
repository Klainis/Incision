using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private CardHand _cardHand;

    [SerializeField] private PlayerMovementData _playerMovementData;
    [SerializeField] private PlayerStats _playerStatsData;

    private PlayerInputActions _inputActions;

    //private bool _isAttacking = false;

    private float _jumpTimeBuffer = 0;

    public PlayerMovementData PlayerMovementData { get { return _playerMovementData; } }
    public PlayerStats PlayerStatsData { get { return _playerStatsData; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if ( _playerCharacter != null )
        {
            _playerCharacter.Initialize(_playerMovementData);
        }
        else
        {
            Debug.LogError("[Player.cs] : отсутствует ссылка на PlayerCharacter");
        }

        if (_playerCamera != null)
        {
            _playerCamera.Initialize(_playerCharacter.GetCameraTarget(), _playerMovementData);
        }
        else
        {
            Debug.LogError("[Player.cs] : отсутствует ссылка на PlayerCamera");
        }
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
