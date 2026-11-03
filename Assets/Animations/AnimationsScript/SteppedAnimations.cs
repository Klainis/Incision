using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppedAnimations : MonoBehaviour
{
    [Serializable]
    public class AnimatorSet
    {
        public Animator animator;
        public float animationFPS;

        [NonSerialized] public float accumulator;
    }

    [SerializeField] private List<AnimatorSet> _animatorsSets = new List<AnimatorSet>();

    public void Initialize()
    {
        foreach (AnimatorSet animatorSet in _animatorsSets)
        {
            if (animatorSet.animator == null)
            {
                Debug.LogError($"[SteppedAnimation.cs] : Absent Animator");
                continue;
            }

            animatorSet.animator.enabled = true;
            animatorSet.accumulator = 0;
        }
    }

    private void Update()
    {
        StepAnim();
    }

    private void StepAnim()
    {
        foreach (AnimatorSet animatorSet in _animatorsSets)
        {
            if (animatorSet.animator == null ||
                animatorSet.animationFPS <= 0f)
                continue;

            float step = 1f / animatorSet.animationFPS;
            animatorSet.accumulator += Time.deltaTime;

            while (animatorSet.accumulator >= step)
            {
                animatorSet.animator.Update(step);
                animatorSet.accumulator -= step;
            }
        }
    }
}
