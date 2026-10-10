using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimit : MonoBehaviour
{
    public enum Limits
    {
        Nolimit = -1,
        Limit30 = 30,
        Limit45 = 45,
        Limit60 = 60,
        Limit120 = 120,
        Limit240 = 240,
    }

    [SerializeField] private Limits limit;
    //[SerializeField] private Limits limit30;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)limit;
        Debug.Log($"[FrameRateLimit.cs] : Выствлен frame rate - {Application.targetFrameRate}");
    }
}
