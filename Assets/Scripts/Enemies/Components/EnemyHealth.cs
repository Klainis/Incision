using Incision.Enemies.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyHealth : Health
    {
        private void OnEnable()
        {
            OnDied += CreateHealOrb;
        }

        private void OnDisable()
        {
            OnDied -= CreateHealOrb;
        }

        public void CreateHealOrb(Health health)
        {

        }
    }
}
