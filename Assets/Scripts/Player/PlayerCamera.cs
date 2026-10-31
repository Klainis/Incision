using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraInput
{
    public Vector2 Look;
    public Vector2 Move;
}

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float maxCameraAngleX = 85f;
    [SerializeField] private float minCameraAngleX = -85f;
    [SerializeField] private float moveAngleZ = 2f;
    [SerializeField] private float moveFieldOfView = 95f;
    [SerializeField] private float dashFieldOfView = 100f;

    private Camera _camera;
    private PlayerCharacter _playerCharacter;

    private Vector3 _eulerAngles;

    private float _sensitivity;
    private float _offsetSmooth;
    private float _initialCameraFieldOfView;
    private float _currentField;

    public void Initialize(Transform target, PlayerMovementData data, Camera camera, PlayerCharacter character)
    {
        _camera = camera;
        _playerCharacter = character;

        transform.position = target.position;
        transform.eulerAngles = _eulerAngles = target.eulerAngles;

        _initialCameraFieldOfView = camera.fieldOfView;
        _currentField = _initialCameraFieldOfView;

        _sensitivity = data.CameraSensitivity;
        _offsetSmooth = data.CameraMoveSmooth;
    }

    public void UpdateRotation(CameraInput input)
    {
        _eulerAngles += new Vector3(-input.Look.y, input.Look.x) * _sensitivity;

        if (_eulerAngles.x > maxCameraAngleX)
        {
            _eulerAngles.x = maxCameraAngleX;
        }
        else if (_eulerAngles.x < minCameraAngleX)
        {
            _eulerAngles.x = minCameraAngleX;
        }

        //Angle offset
        if (input.Move.x == 0)
        {
            _eulerAngles.z = Mathf.Lerp(_eulerAngles.z, 0, _offsetSmooth);
        }
        else if (input.Move.x > 0)
        {
            _eulerAngles.z = Mathf.Lerp(_eulerAngles.z, moveAngleZ, _offsetSmooth);
        }
        else
        {
            _eulerAngles.z = Mathf.Lerp(_eulerAngles.z, -moveAngleZ, _offsetSmooth);
        }

        //Field
        if (_playerCharacter.IsDashing)
        {
            _currentField = Mathf.Lerp(_currentField, dashFieldOfView, _offsetSmooth);
        }
        else if (input.Move.y > 0)
        {
            _currentField = Mathf.Lerp(_currentField, moveFieldOfView, _offsetSmooth);
        }
        else
        {
            _currentField = Mathf.Lerp(_currentField, _initialCameraFieldOfView, _offsetSmooth);
        }

        _camera.fieldOfView = _currentField;
        transform.eulerAngles = _eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }
}
