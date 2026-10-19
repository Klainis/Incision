using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "DecalBufferTemplate", menuName = "Global/DecalBuffer")]
public class DecalBufferTemplate : ScriptableObject
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private int maxDecals = 80;
    
    public DecalProjector Projector => projector;
    public int MaxDecals => maxDecals;
}
