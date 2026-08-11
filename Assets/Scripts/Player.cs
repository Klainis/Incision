using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;

    [SerializeField] private PlayerMovementData playerMovementData;

    private PlayerInputActions _inputActions;

    void Start()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        Cursor.lockState = CursorLockMode.Locked;

        if ( playerCharacter != null )
        {
            playerCharacter.Initialize(playerMovementData);
        }
        else
        {
            Debug.LogError("[Player.cs] : отсутствует ссылка на PlayerCharacter");
        }

        if (playerCamera != null)
        {
            playerCamera.Initialize(playerCharacter.GetCameraTarget(), playerMovementData);
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
        var input = _inputActions.Player;

        var cameraInput = new CameraInput 
        {
            Look = input.Look.ReadValue<Vector2>(),
            Move = input.Move.ReadValue<Vector2>(),
        };
        playerCamera.UpdateRotation(cameraInput);

        var characterInput = new CharacterInput
        {
            Rotation = playerCamera.transform.rotation,
            Move = input.Move.ReadValue<Vector2>(),
            Jump = input.Jump.WasPressedThisFrame(),
            JumpSustain = input.Jump.IsPressed(),
            Dash = input.Dash.WasPressedThisFrame(),
            Shoot = input.Shoot.WasPressedThisFrame() 
        };
        playerCharacter.UpdateInput(characterInput);

        if (characterInput.Shoot)
        {
            //var gun = FindAnyObjectByType<GunAttack>();
            //gun?.Attack();
            //StartCoroutine(ThrowAttack());
            PlayerAnimation.Instance.StartThrowCard();
        }
    }

    //private IEnumerator ThrowAttack()
    //{
    //    PlayerAnimation.Instance.StartThrowCard();

    //    yield return new WaitForSeconds(0.3f);

    //    var gun = FindAnyObjectByType<GunAttack>();
    //    gun.Attack();

    //    PlayerAnimation.Instance.EndThrowCard();
    //}

    private void LateUpdate()
    {
        playerCamera.UpdatePosition(playerCharacter.GetCameraTarget());
    }
}
