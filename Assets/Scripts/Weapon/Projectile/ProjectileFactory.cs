using System;
using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision
{
    public class ProjectileFactory : ITaggable
    {
        public string LOG_TAG { get; } = "[Projectile Factory]".AsTag(Color.magenta);

        private static uint _cardProjectileCreated = 0;
        private static uint _enemyProjectileCreated = 0;

        [Inject] private IObjectResolver _resolver;
        [Inject] private IEventBus _eventBus;

        public CardProjectile CreateCardProjectile(CardData cardData, float damage, bool isCrit, Vector3 position, Quaternion rotation)
        {
            return Create(
                cardData.CardPrefab,
                cardData.ID,
                _cardProjectileCreated++,
                position, rotation,
                p => p.Initialize(damage, isCrit, cardData));
        }

        public EnemyProjectile CreateEnemyProjectile(RangedAttackTemplate rangedAttackData, float damage, Vector3 position, Quaternion rotation)
        {
            return Create(
                rangedAttackData.ProjectilePrefab,
                rangedAttackData.ID,
                _enemyProjectileCreated++,
                position, rotation,
                p => p.Initialize(damage));
        }

        //TODO: for another type of projectiles :)

        private T Create<T>(T prefab, string id, uint index, Vector3 position, Quaternion rotation, Action<T> initialize) where T : Projectile
        {
            T projectile = _resolver.Instantiate(prefab, position, rotation);
            initialize(projectile);
            projectile.name = $"{id}_{index}";

            _eventBus.Publish(
                new Log(
                    LOG_TAG,
                    $"Creating {projectile.name.AsTag(Color.yellow)} at {position.ToString().AsTag(Color.yellow)}"
                )
            );

            return projectile;
        }
    }
}