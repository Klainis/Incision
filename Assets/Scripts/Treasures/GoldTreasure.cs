using System.Collections;
using System.Collections.Generic;
using Incision.Interfaces;
using Incision.Structs.Events;
using Incision.Treasures;
using UnityEngine;

namespace Incision
{
    public class GoldTreasure : Treasure
    {   
        private GoldTreasureTemplate goldTemplate => (GoldTreasureTemplate)_template;
        
        protected override void OnPickedUp()
        {
            _eventBus.Publish(new OnGoldCollected(goldTemplate, Random.Range(goldTemplate.MinGold, goldTemplate.MaxGold)));
        }
    }
}
