using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System;

public struct CharacterInput
{
    public Quaternion Rotation;
    public Vector2 Move;
    public bool Jump;
    public bool JumpSustain;
    public bool Dash;
    public bool Shoot;
    public bool Slam;
}

public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    public static PlayerCharacter Instance {  get; private set; }

    [SerializeField] private KinematicCharacterMotor _motor;
    [SerializeField] private Player _player;

    [Header("Camera Transform")]
    [SerializeField] private Transform _cameraTarget;

    [Header("Movement")]
    [SerializeField] private DashStamina _dashStamina;

    [Header("Collisions")]
    [SerializeField] private LayerMask _enemyLayers;

    [Header("Effects")]
    [SerializeField] private Effecter _effecter;

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
    private float _dashDelay;

    private float _slamSpeed;
    private float _slamMinHeight;
    private float _slamStartY = 0;

    private bool _requestedJump;
    private bool _requestedSustainJump;
    private bool _canJump = true;
    private bool _canDoubleJump = false;

    private bool _requestedDash;
    private bool _isDashing;
    private bool _canDash = true;

    private bool _requestedSlam;
    private bool _isSlamming;
    private bool _canSlam = true;

    private bool _ignoreEnemies;

    //private bool _oldGroundingStatus = false;
    private bool _isJumpingUp;

    public bool IsDashing {  get { return _isDashing; } }
    public Transform GetCameraTarget() => _cameraTarget;

    private Coroutine _dashCoroutine;
    private Coroutine _dashCoolDownCoroutine;

    private readonly Collider[] _overlaps = new Collider[4];

    public event Action<float> SlamLamded;

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
        _motor.CharacterController = this;

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
        _dashDelay = data.DashDelay;
        _slamSpeed = data.SlamSpeed;
        _slamMinHeight = data.SlamMinHeight;

        _canJump = true;
        _canDash = true;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.Rotation;

        Vector3 planarForward = Vector3.ProjectOnPlane(input.Rotation * Vector3.forward, _motor.CharacterUp);
        if (planarForward.sqrMagnitude < 0.0001f)
        {
            planarForward = _forwardDirection.sqrMagnitude > 0f ? _forwardDirection : Vector3.forward;
        }
        Quaternion yawRotation = Quaternion.LookRotation(planarForward.normalized, _motor.CharacterUp);

        Vector3 move = new Vector3(input.Move.x, 0f, input.Move.y);
        _requestedMovement = yawRotation * Vector3.ClampMagnitude(move, 1f);

        _requestedJump = _requestedJump || input.Jump;
        _requestedSustainJump = input.JumpSustain;

        _requestedDash = _requestedDash || input.Dash;

        _requestedSlam = _requestedSlam || input.Slam;
    }

    #region Interface
    public void AfterCharacterUpdate(float deltaTime)
    {

    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        if (_isDashing || _isSlamming)
        {
            _ignoreEnemies = true;
        }
        else if (_ignoreEnemies)
        {
            _ignoreEnemies = false;

            int count = _motor.CharacterOverlap(
                _motor.TransientPosition,
                _motor.TransientRotation,
                _overlaps,
                _enemyLayers,
                QueryTriggerInteraction.Ignore);

            _ignoreEnemies = count > 0;
        }
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        bool isEnemy = (_enemyLayers.value & (1 << coll.gameObject.layer)) != 0;

        if (isEnemy && (_isDashing || _isSlamming || _ignoreEnemies))
            return false;

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
        bool isGrounded = _motor.GroundingStatus.IsStableOnGround;
        bool wasGrounded = _motor.LastGroundingStatus.IsStableOnGround;

        if (isGrounded && !wasGrounded)
        {
            _isJumpingUp = false;

            _canJump = true;
            _canDoubleJump = false;

            CallFallAnimation(false);
        }

        if (isGrounded)
        {
            EndSlam();
        }

        if (!isGrounded && wasGrounded)
        {
            CallFallAnimation(true);
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        _forwardDirection = Vector3.ProjectOnPlane
        (
            _requestedRotation * Vector3.forward,
            _motor.CharacterUp
        );

        if (_forwardDirection != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(_forwardDirection, _motor.CharacterUp);
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (!_canDash)
        {
            _requestedDash = false;
        }

        if (_requestedJump && !_motor.GroundingStatus.IsStableOnGround && !_canDoubleJump)
        {
            _requestedJump = false;
        }

        //DASH
        if (_requestedDash && _canDash && !_isSlamming)
        {
            _requestedDash = false;

            if (_dashStamina.TryConsumeStamina())
            {
                _canDash = false;
                _isDashing = true;

                CallDashAnimation();
                _effecter.PlaySpeedLines();

                Vector3 rawDir = _requestedMovement.magnitude > 0.01f ? _requestedMovement : _forwardDirection;
                _dashDirection = Vector3.ProjectOnPlane(rawDir, _motor.CharacterUp).normalized;

                StartDashRoutine();
            }
        }
        if (_isDashing)
        {
            _requestedDash = false;
            _requestedJump = false;
            _requestedSlam = false;

            currentVelocity = _dashDirection * _dashSpeed;
            return;
        }

        //SLAM
        if (_requestedSlam && _canSlam)
        {
            _requestedSlam = false;

            bool inAir = !_motor.GroundingStatus.IsStableOnGround;
            bool highEnough = !Physics.Raycast(_motor.TransientPosition, -_motor.CharacterUp, _slamMinHeight, _motor.StableGroundLayers);

            if (inAir && highEnough && !_isSlamming)
            {
                _canSlam = false;
                _canJump = false;
                _canDoubleJump = false;

                _isSlamming = true;
                _isDashing = false;
                _isJumpingUp = false;

                _slamStartY = _motor.TransientPosition.y;

                _effecter.PlaySlamLines();
            }
        }
        if (_isSlamming)
        {
            currentVelocity = -_motor.CharacterUp * _slamSpeed;
            return;
        }

        if (_motor.GroundingStatus.IsStableOnGround)
        {
            //MOVE
            _groundedMovementDirection = _motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: _motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;

            if (!_isDashing)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, _groundedMovementDirection * _groundMovementSpeed, _groundMovementSmooth);

                var normVel = currentVelocity.normalized.magnitude;
                CallRunAnimation(normVel);
            }
        }
        else
        {
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(currentVelocity, _motor.CharacterUp);
            Vector3 targetHorizontalVelocity = _requestedMovement * _airMovementSpeed;

            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetHorizontalVelocity, _airMovementSmooth);

            if (!_isDashing)
            {
                currentVelocity = Vector3.Project(currentVelocity, _motor.CharacterUp) + horizontalVelocity;
            }

            //GRAVITY
            var effectivedGravity = _gravity;
            var verticalSpeed = Vector3.Dot(currentVelocity, _motor.CharacterUp);
            if (_requestedSustainJump && verticalSpeed > 0)
            {
                effectivedGravity *= _jumpSustainGravity;
            }
            if (_isDashing)
            {
                effectivedGravity = 0f;
            }

            currentVelocity += _motor.CharacterUp * effectivedGravity * deltaTime;

            //_oldGroundingStatus = false;
        }

        //JUMP
        if (_motor.GroundingStatus.IsStableOnGround && _requestedJump && _canJump)
        {
            _player?.ConsumeJumpBuffer();

            _requestedJump = false;
            _isJumpingUp = true;

            _canJump = false;
            _canDoubleJump = true;

            _motor.ForceUnground(time: 0.1f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, _motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _jumpSpeed);

            currentVelocity += _motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
        }

        //DOUBLE JUMP
        if (_requestedJump && _canDoubleJump)
        {
            _player?.ConsumeJumpBuffer();

            _requestedJump = false;
            _isJumpingUp = true;

            _canJump = false;
            _canDoubleJump = false;

            _motor.ForceUnground(time: 0.1f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, _motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _doubleJumpSpeed);

            currentVelocity += _motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
        }

        ////JUMP STOP
        //if(_isJumpingUp && !_requestedSustainJump)
        //{
        //    _isJumpingUp = false;
        //    float verticalSpeed = Vector3.Dot(currentVelocity, _motor.CharacterUp);
        //    verticalSpeed = Mathf.Lerp(verticalSpeed, 0f, _jumpSmooth);

        //    currentVelocity = Vector3.ProjectOnPlane(currentVelocity, _motor.CharacterUp) + (_motor.CharacterUp * verticalSpeed);
        //}
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

        _effecter.StopSpeedLines();
        _isDashing = false;

        //_motor.BaseVelocity = Vector3.zero;

        StartDashCoolDownRoutine();

        _dashCoroutine = null;
    }

    private IEnumerator DashCoolDownRoutine()
    {
        float dashTime = _dashDelay;
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

    private void EndSlam()
    {
        if (_isSlamming)
        {
            _isSlamming = false;
            _canSlam = true;

            _effecter.StopSlamLines();

            float slamHeight = _slamStartY - _motor.TransientPosition.y;
            SlamLamded?.Invoke(slamHeight);
            _slamStartY = 0;
        }
    }

    private void CallRunAnimation(float speed)
    {
        PlayerAnimation.Instance.RunAnimation(speed);
    }

    private void CallFallAnimation(bool isFalling)
    {
        PlayerAnimation.Instance.FallAnimation(isFalling);
    }

    private void CallDashAnimation()
    {
        PlayerAnimation.Instance.DashAnimation();
    }
}
