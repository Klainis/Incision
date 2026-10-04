using Incision.Enemies.Structs;

namespace Incision.Enemies.Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(DamageInfo info);
    }
}
