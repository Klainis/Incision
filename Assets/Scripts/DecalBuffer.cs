using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Incision.Effects
{
    public class DecalBuffer
    {
        private readonly DecalProjector _decalProjector;
        private readonly int _maxDecals;

        private readonly IObjectResolver _resolver;

        private readonly Queue<DecalProjector> _decals = new Queue<DecalProjector>();

        public DecalBuffer(IObjectResolver resolver, DecalBufferTemplate decalTemplate)
        {
            _decalProjector = decalTemplate.Projector;
            _maxDecals = decalTemplate.MaxDecals;
            _resolver = resolver;
        }

        public void SpawnDecal(Vector3 position, Vector3 normal, float size)
        {
            Quaternion rotation = Quaternion.LookRotation(-normal);
            rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

            Vector3 spawnPosition = position - normal * 0.1f;

            DecalProjector decal = SpawnDecalLocal(spawnPosition, rotation, size);
            if (decal == null) return;


            _decals.Enqueue(decal);
        }

        private DecalProjector SpawnDecalLocal(Vector3 position, Quaternion rotation, float size)
        {
            while (_decals.Count >= _maxDecals)
            {
                var oldest = _decals.Dequeue();

                if (oldest != null)
                    return oldest;
            }

            if (_decalProjector == null)
            {
                return null;
            }

            var decal = _resolver.Instantiate(_decalProjector, position, rotation);
            
            decal.size = new Vector3(size, size, decal.size.z);
            return decal;
        }
    }
}