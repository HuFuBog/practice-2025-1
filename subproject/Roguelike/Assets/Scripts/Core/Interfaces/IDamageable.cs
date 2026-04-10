using Roguelike.Combat;

namespace Roguelike.Core.Interfaces
{
    // интерфейс любого объекта, который может получить урон
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damage);
        void Heal(float amount);
    }
}