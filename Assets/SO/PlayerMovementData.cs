using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Camera")]
    [SerializeField] private float cameraSensitivity = 0.1f;
    [SerializeField][Range(0f, 1f)] private float cameraMoveOffsetSmooth = 0.2f;

    [Header("Movement")]
    [SerializeField] private float groundMovementSpeed = 20f;
    [SerializeField] private float airMovementSpeed = 20f;
    [SerializeField][Range(0f, 1f)] private float groundMovementSmooth = 0.8f;
    [SerializeField][Range(0f, 1f)] private float airMovementSmooth = 0.25f;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 20f;
    [SerializeField] private float doubleJumpSpeed = 20f;
    [SerializeField] private float smallJumpSpeed = 5f;
    [SerializeField] private float jumpBufferTime = 0.14f;

    [Space]
    [SerializeField][Range(0f, 1f)] private float _jumpSmoothStopper = 0.1f;


    [Header("Dash")]
    [SerializeField] private float dashSpeed = 40f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashDelay = 0.1f;
    [SerializeField] private float dashCoolDown = 0.8f;
    [SerializeField] private int dashMaxCharges = 3;
    [SerializeField][Range(0f, 1f)] private float gravityInDash = 0f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -90f;
    [SerializeField][Range(0f, 1f)] private float gravityInJumpUp = 0.4f;


    public float CameraSensitivity => cameraSensitivity;
    public float CameraMoveSmooth => cameraMoveOffsetSmooth;
    public float GroundMovementSpeed => groundMovementSpeed;
    public float AirMovementSpeed => airMovementSpeed;
    public float GroundMovementSmooth => groundMovementSmooth;
    public float AirMovementSmooth => airMovementSmooth;
    public float JumpSpeed => jumpSpeed;
    public float DoubleJumpSpeed => doubleJumpSpeed;
    public float SmallJumpSpeed => smallJumpSpeed;
    public float Gravity => gravity;
    public float JumpSustainGravity => gravityInJumpUp;
    public float JumpSmooth => _jumpSmoothStopper;
    public float DashSpeed => dashSpeed;
    public float DashTime => dashTime;
    public float GravityInDash => gravityInDash;
    public float DashDelay => dashDelay;
    public float DashCoolDown => dashCoolDown;
    public int DashMaxCharges => dashMaxCharges;
    public float JumpBufferTime => jumpBufferTime;
}
