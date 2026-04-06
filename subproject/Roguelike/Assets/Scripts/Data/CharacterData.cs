using UnityEngine;
using System.Collections.Generic;
using Roguelike.Core.Enums;

namespace Roguelike.Data
{
    [System.Serializable]
    public struct ElementalResistance
    {
        public DamageType type;
        [Range(-1f, 1f)] public float value; // 0.5 = 50% защиты, -0.5 = получает на 50% больше урона
    }

    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Roguelike/Data/Character")]
    public class CharacterData : ScriptableObject
    {
        [Header("Base Stats")]
        public float maxHealth = 100f;
        public float baseMoveSpeed = 5f;
        public float baseArmor = 10f;
        [Range(0f, 100f)] public float evasionChance = 5f;

        [Header("Resistances")]
        public List<ElementalResistance> resistances = new List<ElementalResistance>();
    }
}