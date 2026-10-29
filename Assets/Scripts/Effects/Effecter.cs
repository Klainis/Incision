using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _speedLinesParticle;
    [SerializeField] private ParticleSystem _slamLinesParticle;

    public void PlaySpeedLines()
    {
        _speedLinesParticle.Play();
    }

    public void StopSpeedLines()
    {
        _speedLinesParticle.Stop();
    }

    public void PlaySlamLines()
    {
        _slamLinesParticle.Play();
    }

    public void StopSlamLines()
    {
        _slamLinesParticle.Stop();
    }
}
