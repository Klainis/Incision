using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effecter : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedLinesParticle;

    public void PlaySpeedLines()
    {
        speedLinesParticle.Play();
    }

    public void StopSpeedLines()
    {
        speedLinesParticle.Stop();
    }
}
