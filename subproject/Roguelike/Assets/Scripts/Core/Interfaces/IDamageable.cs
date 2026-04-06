using Roguelike.Combat;

namespace Roguelike.Core.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damage);
        void Heal(float amount);
    }
}