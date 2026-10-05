using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private PlayerCamera _playerCamera;

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
    }

    void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

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

        var input = _inputActions.Player;

        var cameraInput = new CameraInput 
        {
            Look = input.Look.ReadValue<Vector2>(),
            Move = input.Move.ReadValue<Vector2>(),
        };
        _playerCamera.UpdateRotation(cameraInput);

        if (input.Jump.WasPressedThisFrame())
        {
            _jumpTimeBuffer = _playerMovementData.JumpBufferTime;
        }

        var characterInput = new CharacterInput
        {
            Rotation = _playerCamera.transform.rotation,
            Move = input.Move.ReadValue<Vector2>(),
            Jump = _jumpTimeBuffer > 0,
            JumpSustain = input.Jump.IsPressed(),
            Dash = input.Dash.WasPressedThisFrame(),
            //Shoot = input.Shoot.WasPressedThisFrame(),
            Slam = input.Slam.WasPressedThisFrame(),
        };
        _playerCharacter.UpdateInput(characterInput);

        //bool shoot = input.Shoot.WasPressedThisFrame() || input.Shoot.IsPressed();

        //if (shoot && !_isAttacking)
        //{
        //    PlayerAnimation.Instance.ThrowCardTrigger();
        //}
    }

    //public void OnAttackEnded()
    //{
    //    _isAttacking = false;
    //}

    private void LateUpdate()
    {
        _playerCamera.UpdatePosition(_playerCharacter.GetCameraTarget());
    }
    
    public void ConsumeJumpBuffer()
    {
        _jumpTimeBuffer = 0f;
    }
}
