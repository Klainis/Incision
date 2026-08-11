using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public struct CharacterInput
{
    public Quaternion Rotation;
    public Vector2 Move;
    public bool Jump;
    public bool JumpSustain;
    public bool Dash;
    public bool Shoot;
}

public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    public static PlayerCharacter Instance {  get; private set; }

    [SerializeField] private KinematicCharacterMotor motor;

    [Header("Camera Transform")]
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private Effecter effecter;

    private Quaternion _requestedRotation;

    private Vector3 _requestedMovement;
    private Vector3 _groundedMovementDirection;
    private Vector3 _forwardDirection;

    private Vector3 _dashDirection;

    private float _groundMovementSpeed;
    private float _airMovementSpeed;
    private float _groundMovementSmooth;
    private float _airMovementSmooth;

    private float _jumpSpeed;
    private float _doubleJumpSpeed;
    private float _smallJumpSpeed;

    private float _gravity;
    private float _jumpSustainGravity;
    private float _jumpSmooth;

    private float _dashSpeed;
    private float _dashTime;
    private float _gravityInDash;
    private float _dashCoolDown;

    private bool _requestedJump;
    private bool _requestedSustainJump;
    private bool _canJump = true;
    private bool _canDoubleJump = false;

    private bool _requestedDash;
    private bool _isDashing;
    private bool _canDash = true;

    private bool _oldGroundingStatus = false;
    private bool _isJumpingUp;

    public bool IsDashing {  get { return _isDashing; } }

    private Coroutine _dashCoroutine;
    private Coroutine _dashCoolDownCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void Initialize(PlayerMovementData data)
    {
        motor.CharacterController = this;

        _groundMovementSpeed = data.GroundMovementSpeed;
        _airMovementSpeed = data.AirMovementSpeed;
        _groundMovementSmooth = data.GroundMovementSmooth;
        _airMovementSmooth = data.AirMovementSmooth;
        _jumpSpeed = data.JumpSpeed;
        _doubleJumpSpeed = data.DoubleJumpSpeed;
        _smallJumpSpeed = data.SmallJumpSpeed;
        _gravity = data.Gravity;
        _jumpSustainGravity = data.JumpSustainGravity;
        _jumpSmooth = data.JumpSmooth;
        _dashSpeed = data.DashSpeed;
        _dashTime = data.DashTime;
        _gravityInDash = data.GravityInDash;
        _dashCoolDown = data.DashCoolDown;

        _canJump = true;
        _canDash = true;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.Rotation;

        _requestedMovement = new Vector3(input.Move.x, 0f, input.Move.y);
        _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
        _requestedMovement = input.Rotation * _requestedMovement;

        _requestedJump = _requestedJump || input.Jump;
        _requestedSustainJump = input.JumpSustain;

        _requestedDash = _requestedDash || input.Dash;
    }

    #region Interface
    public void AfterCharacterUpdate(float deltaTime)
    {

    }

    public void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void PostGroundingUpdate(float deltaTime)
    {

    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        _forwardDirection = Vector3.ProjectOnPlane
        (
            _requestedRotation * Vector3.forward,
            motor.CharacterUp
        );

        if (_forwardDirection != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(_forwardDirection, motor.CharacterUp);
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (!_canDash)
        {
            _requestedDash = false;
        }

        if (_requestedJump && !motor.GroundingStatus.IsStableOnGround && !_canDoubleJump)
        {
            _requestedJump = false;
        }

        //DASH
        if (_requestedDash && _canDash)
        {
            _requestedDash = false;
            _canDash = false;
            _isDashing = true;

            effecter.PlaySpeedLines();

            Vector3 rawDir = _requestedMovement.magnitude > 0.01f ? _requestedMovement : _forwardDirection;
            _dashDirection = Vector3.ProjectOnPlane(rawDir, motor.CharacterUp).normalized;

            StartDashRoutine();
        }
        if (_isDashing)
        {
            _requestedDash = false;
            _requestedJump = false;

            currentVelocity = _dashDirection * _dashSpeed;
            return;
        }

        if (motor.GroundingStatus.IsStableOnGround)
        {
            //Момент столкновения с землей
            if (!_oldGroundingStatus)
            {
                CallEndFallAnimation();

                _oldGroundingStatus = true;
                _isJumpingUp = false;

                _canJump = true;
                _canDoubleJump = false;
            }

            //MOVE
            _groundedMovementDirection = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;

            if (!_isDashing)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, _groundedMovementDirection * _groundMovementSpeed, _groundMovementSmooth);
            }

            if (currentVelocity.x != 0 || currentVelocity.z != 0)
            {
                CallStartRunAnimation();
            }
            else
            {
                CallEndRunAnimation();
            }
        }
        else
        {
            CallStartFallAnimation();

            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);
            Vector3 targetHorizontalVelocity = _requestedMovement * _airMovementSpeed;

            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetHorizontalVelocity, _airMovementSmooth);

            if (!_isDashing)
            {
                currentVelocity = Vector3.Project(currentVelocity, motor.CharacterUp) + horizontalVelocity;
            }

            //GRAVITY
            var effectivedGravity = _gravity;
            var verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            if (_requestedSustainJump && verticalSpeed > 0)
            {
                effectivedGravity *= _jumpSustainGravity;
            }
            if (_isDashing)
            {
                effectivedGravity = 0f;
            }

            currentVelocity += motor.CharacterUp * effectivedGravity * deltaTime;

            _oldGroundingStatus = false;
        }

        //JUMP
        if (motor.GroundingStatus.IsStableOnGround && _requestedJump && _canJump)
        {
            _requestedJump = false;
            _isJumpingUp = true;

            _canJump = false;
            _canDoubleJump = true;

            motor.ForceUnground(time: 0.1f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _jumpSpeed);

            currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
        }

        //DOUBLE JUMP
        if (_requestedJump && _canDoubleJump)
        {
            _requestedJump = false;
            _isJumpingUp = true;

            _canJump = false;
            _canDoubleJump = false;

            motor.ForceUnground(time: 0.1f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _doubleJumpSpeed);

            currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
        }

        //JUMP STOP
        if(_isJumpingUp && !_requestedSustainJump)
        {
            _isJumpingUp = false;
            float verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            verticalSpeed = Mathf.Lerp(verticalSpeed, 0f, _jumpSmooth);

            currentVelocity = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp) + (motor.CharacterUp * verticalSpeed);
        }
    }

    #endregion

    #region Coroutine

    private void StartDashRoutine()
    {
        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
        }

        _dashCoroutine = StartCoroutine(DashRoutine());
    }

    private void StartDashCoolDownRoutine()
    {
        if (_dashCoolDownCoroutine != null)
        {
            StopCoroutine(_dashCoolDownCoroutine);
        }

        _dashCoolDownCoroutine = StartCoroutine(DashCoolDownRoutine());
    }

    private IEnumerator DashRoutine()
    {
        float dashTime = _dashTime;
        float requestedTime = 0;

        while (requestedTime <= dashTime)
        {
            requestedTime += Time.deltaTime;
            yield return null;
        }

        effecter.StopSpeedLines();
        _isDashing = false;

        motor.BaseVelocity = Vector3.zero;

        StartDashCoolDownRoutine();

        _dashCoroutine = null;
    }

    private IEnumerator DashCoolDownRoutine()
    {
        float dashTime = _dashCoolDown;
        float requestedTime = 0;

        while (requestedTime <= dashTime)
        {
            requestedTime += Time.deltaTime;
            yield return null;
        }

        _canDash = true;

        _dashCoolDownCoroutine = null;
    }

    #endregion

    public Transform GetCameraTarget() => cameraTarget;

    private void CallStartRunAnimation()
    {
        PlayerAnimation.Instance.StartRun();
    }

    private void CallEndRunAnimation()
    {
        PlayerAnimation.Instance.EndRun();
    }

    private void CallStartFallAnimation()
    {
        PlayerAnimation.Instance.StartFall();
    }

    private void CallEndFallAnimation()
    {
        PlayerAnimation.Instance.EndFall();
    }
}
