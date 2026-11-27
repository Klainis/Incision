using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "StunEffectTemplate", menuName = "Scriptable Objects/Status/Stun")]
    public class StunTemplate : StatusEffectTemplate
    {
        public override StatusEffect CreateRuntime(GameObject source) => new StunEffect(this, source);
    }

    public class StunEffect : StatusEffect
    {
        public override StatusFlags Flags => StatusFlags.Stunned;
        public StunEffect(StatusEffectTemplate template, GameObject source) : base(template, source) { }
    }
}
