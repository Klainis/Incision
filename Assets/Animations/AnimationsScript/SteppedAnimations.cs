using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SteppedAnimations : MonoBehaviour
{
    [SerializeField] private float _animationFPS = 12f;

    Animator _animator;
    float _accumulator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

    private void Update()
    {
        float step = 1f / _animationFPS;
        _accumulator += Time.deltaTime;

        while (_accumulator >= step)
        {
            _animator.Update(step);
            _accumulator -= step;
        }
    }
}
