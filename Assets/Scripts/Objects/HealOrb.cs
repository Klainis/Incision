using UnityEngine;

namespace Incision
{
    public class HealOrb : AttractedPickup
    {
        [SerializeField] private HealOrbTemplate _healOrbData;

        private PlayerHealth _playerHealth;

        protected override Transform ResolvePlayerTarget(Transform currentTarget, float attractDistance)
        {
            if (currentTarget != null && _playerHealth != null) return currentTarget;

            PlayerCharacter character = PlayerCharacter.Instance;
            if (character == null) return null;

            _playerHealth = character.GetComponentInParent<PlayerHealth>();

            return _playerHealth != null ? character.transform : null;
        }

        protected override void OnPickedUp()
        {
            if (_playerHealth != null)
                _playerHealth.Heal(_healOrbData != null ? _healOrbData.HealAmount : 10f);
        }
    }
}

