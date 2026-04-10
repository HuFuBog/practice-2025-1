using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core.Stats
{
    public enum ModifierType { Flat, PercentAdd, PercentMult }

    [System.Serializable]
    public class StatModifier
    {
        public float Value;
        public ModifierType Type;
        public object Source; // кто наложил бафф 

        public StatModifier(float value, ModifierType type, object source = null)
        {
            Value = value;
            Type = type;
            Source = source;
        }
    }

    [System.Serializable]
    public class Stat
    {
        [SerializeField] private float baseValue;
        private readonly List<StatModifier> modifiers = new List<StatModifier>();

        public Stat(float baseValue) => this.baseValue = baseValue;

        public float GetValue()
        {
            // ВАЖНО - считаем отдельно плоскую добавку и процентную, чтобы не получить ошибки связанные с порядком проверки модификатора.
            float finalValue = baseValue;
            float percentAdd = 0f;

            foreach (var mod in modifiers)
            {
                if (mod.Type == ModifierType.Flat) finalValue += mod.Value;
                else if (mod.Type == ModifierType.PercentAdd) percentAdd += mod.Value;
                else if (mod.Type == ModifierType.PercentMult) finalValue *= (1 + mod.Value);
            }

            finalValue *= (1 + percentAdd);
            return (float)System.Math.Round(finalValue, 4); // Защита от ошибок float
        }

        public void AddModifier(StatModifier mod) => modifiers.Add(mod);

        public void RemoveAllModifiersFromSource(object source)
        {
            modifiers.RemoveAll(mod => mod.Source == source);
        }
    }
}