using Incision.Enemies.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Enemies.Components
{
    public class EnemyHealth : Health
    {
        [Header("Heal Orb")]
        [SerializeField] private HealOrbTemplate _healOrbData;
        [SerializeField] private HealOrb _healOrbPrefab;
        [SerializeField] private Vector3 _instantiateOffset = new Vector3(0, 1, 0);

        [Inject] ObjectFactory _objectFactory;

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
            for (int count = 1; count <= _healOrbData.OrbAmount; count++)
            {
                _objectFactory.CreateHealOrb(_healOrbData, _healOrbPrefab, transform.position + _instantiateOffset, Quaternion.identity);
            }
        }
    }
}
