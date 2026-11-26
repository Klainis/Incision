using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Incision.Interfaces
{
    public interface IStatusReceiver
    {
        public bool TryApply(StatusEffectTemplate tpl, GameObject source, float? duration = null);
    }
}
