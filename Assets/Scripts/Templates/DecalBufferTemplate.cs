using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "General/DecalBuffer", fileName = "DecalBufferTemplate")]
public class DecalBufferTemplate : ScriptableObject
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private int maxDecals = 80;
    
    public DecalProjector Projector => projector;
    public int MaxDecals => maxDecals;
}
