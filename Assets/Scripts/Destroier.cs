using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroier : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 10f;
    [SerializeField] private TrailRenderer _trailRenderer;

    private float _currentTime;

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > _destroyTime)
        {
            if (_trailRenderer != null && _trailRenderer.enabled)
            {
                _trailRenderer.enabled = false;
            }

            Destroy(gameObject);
        }
    }
}
