
using Incision.Enemies.Templates;

namespace Incision.Enemies.Runtime
{
    public class EnemyStats
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }
        public float MoveSpeed { get; }
        public float TurnSpeed { get; }
        public float Armor { get; }
        public float DetectionRadius { get; }
        public float LoseTargetRadius { get; }

        public EnemyStats(EnemyStatsTemplate template)
        {
            MaxHealth = template.MaxHealth;
            CurrentHealth = template.MaxHealth;
            MoveSpeed = template.MoveSpeed;
            TurnSpeed = template.TurnSpeed;
            Armor = template.Armor;
            DetectionRadius = template.DetectionRadius;
            LoseTargetRadius = template.DetectionRadius;
            
            if (template.LoseTargetRadius == 0)
                LoseTargetRadius = int.MaxValue;
            else if (template.LoseTargetRadius < DetectionRadius)
                LoseTargetRadius += template.LoseTargetRadius;
        }
    }
}
