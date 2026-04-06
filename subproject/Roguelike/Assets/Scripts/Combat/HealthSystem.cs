using UnityEngine;
using System;
using System.Collections.Generic;
using Roguelike.Core.Interfaces;
using Roguelike.Core.Stats;
using Roguelike.Core.Enums;
using Roguelike.Data;

namespace Roguelike.Combat
{
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        public CharacterData baseData;

        public float CurrentHealth { get; private set; }

        public Stat Armor;
        public Stat Evasion;
        private Dictionary<DamageType, float> resistanceDict = new Dictionary<DamageType, float>();

        public event Action<float, float> OnHealthChanged; // Current, Max
        public event Action OnDeath;
        public event Action<DamageInfo> OnDamageTaken; // Для UI всплывающих цифр урона и возможно на логику работы всяких шипов

        private void Awake()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            CurrentHealth = baseData.maxHealth;
            Armor = new Stat(baseData.baseArmor);
            Evasion = new Stat(baseData.evasionChance);

            // Переносим настройки из List в Dictionary для быстрого поиска в бою
            foreach (var res in baseData.resistances)
            {
                resistanceDict[res.type] = res.value;
            }
        }

        public void TakeDamage(DamageInfo damage)
        {
            if (CurrentHealth <= 0) return;

            // 1. Уклонение
            if (UnityEngine.Random.Range(0f, 100f) <= Evasion.GetValue())
            {
                Debug.Log($"{gameObject.name} уклонился!");
                return;
            }

            float finalDamage = damage.Amount;

            // 2. Резисты стихиям (кроме TrueDamage)
            if (damage.Type != DamageType.TrueDamage && resistanceDict.TryGetValue(damage.Type, out float resist))
            {
                finalDamage *= 1f - resist;
            }

            // 3. Броня (только для физ урона)
            if (damage.Type == DamageType.Physical)
            {
                float armorVal = Armor.GetValue();
                // Формула: чем больше брони, тем меньше входящий урон, но иммунитета не достичь
                finalDamage *= (100f / (100f + Mathf.Max(0, armorVal)));
            }

            // Минимальный урон 1, чтобы не было нулевого урона
            finalDamage = Mathf.Max(1f, finalDamage);

            CurrentHealth -= finalDamage;

            // Вызываем ивенты
            OnDamageTaken?.Invoke(new DamageInfo(finalDamage, damage.Type, damage.Source, damage.IsCritical));
            OnHealthChanged?.Invoke(CurrentHealth, baseData.maxHealth);

            if (CurrentHealth <= 0) Die();
        }

        public void Heal(float amount)
        {
            if (CurrentHealth <= 0) return;
            CurrentHealth = Mathf.Min(CurrentHealth + amount, baseData.maxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, baseData.maxHealth);
        }

        private void Die()
        {
            OnDeath?.Invoke();
            // В идеале возвращать в Пул, а не Destroy, но пока не так то много врагов
            Destroy(gameObject);
        }
    }
}