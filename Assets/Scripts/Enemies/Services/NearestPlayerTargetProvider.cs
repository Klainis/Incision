using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Services
{
    public class NearestPlayerTargetProvider : ITargetProvider
    {
        private readonly Transform _player;

        public NearestPlayerTargetProvider(Transform player)
        {
            _player = player;
        }

        public Transform GetTarget(Enemy enemy)
        {
            if (_player == null) return null;

            float dist = Vector3.Distance(enemy.transform.position, _player.position);
            return dist <= enemy.Stats.DetectionRadius ? _player : null;
        }
    }
}