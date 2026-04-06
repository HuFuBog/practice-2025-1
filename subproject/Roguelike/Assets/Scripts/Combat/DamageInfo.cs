using UnityEngine;
using Roguelike.Core.Enums;

namespace Roguelike.Combat
{
    public struct DamageInfo
    {
        public float Amount;
        public DamageType Type;
        public GameObject Source; // Кто нанес урон (чтобы триггернуть акссесуары, усиления и т. п.)
        public bool IsCritical;

        public DamageInfo(float amount, DamageType type, GameObject source = null, bool isCrit = false)
        {
            Amount = amount;
            Type = type;
            Source = source;
            IsCritical = isCrit;
        }
    }
}