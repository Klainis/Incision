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
    [SerializeField] private Camera camera;

    [Space]
    [SerializeField] private float maxCameraAngleX = 85f;
    [SerializeField] private float minCameraAngleX = -85f;
    [SerializeField] private float moveAngleZ = 2f;
    [SerializeField] private float moveFieldOfView = 95f;
    [SerializeField] private float dashFieldOfView = 100f;

    private Vector3 _eulerAngles;

    private float _sensitivity;
    private float _offsetSmooth;
    private float _initialCameraFieldOfView;

    public void Initialize(Transform target, PlayerMovementData data)
    {
        transform.position = target.position;
        transform.eulerAngles = _eulerAngles = target.eulerAngles;

        _initialCameraFieldOfView = camera.fieldOfView;

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

        //Наклон при движении вбок
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

        //Приближение при движении вперед
        if (PlayerCharacter.Instance.IsDashing)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, dashFieldOfView, _offsetSmooth);
        }
        else if (input.Move.y > 0)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, moveFieldOfView, _offsetSmooth);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _initialCameraFieldOfView, _offsetSmooth);
        }

        transform.eulerAngles = _eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }
}
