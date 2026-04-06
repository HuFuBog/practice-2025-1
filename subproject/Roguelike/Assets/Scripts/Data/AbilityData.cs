using UnityEngine;
using Roguelike.Core.Enums;

namespace Roguelike.Data
{
    [CreateAssetMenu(fileName = "NewAbility", menuName = "Roguelike/Data/Ability")]
    public class AbilityData : ScriptableObject
    {
        public string abilityName;
        [TextArea] public string description;

        public float cooldown = 1f;
        public float damageAmount = 10f;
        public DamageType damageType = DamageType.Physical;

        // префаб, который появится при использовании (например, фаербол или зона взрыва)
        public GameObject executionPrefab;
    }
}