
using Incision.Templates;

namespace Incision.Data
{
    public class Stats
    {
        public float Mass { get; }
        public float MaxHealth { get; }
        public float CurrentHealth { get; set; }
        public float MoveSpeed { get; }
        public float Acceleration { get; }
        public float TurnSpeed { get; }
        public float Armor { get; }
        public float DetectionRadius { get; }
        public float LoseTargetRadius { get; }

        public Stats(StatsTemplate template)
        {
            Mass = template.Mass;
            MaxHealth = template.MaxHealth;
            CurrentHealth = template.MaxHealth;
            MoveSpeed = template.MoveSpeed;
            Acceleration = template.Acceleration;
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
