using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "SharperCardData", menuName = "Scriptable Objects/CardData/SharperCardData")]
    public class SharperCardData : CardData
    {
        [Header("Ricochet")]
        [SerializeField] private int _baseBounces = 1;
        [SerializeField] private float _searchRadius = 25f;
        [SerializeField] private LayerMask _enemyLayers;

        public int BaseBounces => _baseBounces;
        public float SearchRadius => _searchRadius;
        public LayerMask EnemyLayers => _enemyLayers;
    }
}
