using UnityEngine;
using Roguelike.Data;

namespace Roguelike.Actors.Player
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Equipped Abilities")]
        public AbilityData attackData;
        public AbilityData skillData;
        public AbilityData ultimateData;

        [Header("Spawn Points")]
        public Transform projectileSpawnPoint; // Откуда вылетают снаряды, верхушка посоха или дуло винтовки например

        private float attackTimer, skillTimer, ultimateTimer;

        private void Update()
        {
            if (attackTimer > 0) attackTimer -= Time.deltaTime;
            if (skillTimer > 0) skillTimer -= Time.deltaTime;
            if (ultimateTimer > 0) ultimateTimer -= Time.deltaTime;
        }

        public void UseAttack(Vector2 direction) => ExecuteAbility(attackData, ref attackTimer, direction);
        public void UseSkill(Vector2 direction) => ExecuteAbility(skillData, ref skillTimer, direction);
        public void UseUltimate(Vector2 direction) => ExecuteAbility(ultimateData, ref ultimateTimer, direction);

        private void ExecuteAbility(AbilityData data, ref float timer, Vector2 direction)
        {
            if (data == null || timer > 0) return;

            timer = data.cooldown;

            if (data.executionPrefab != null)
            {
                // Спавним префаб способности и поворачиваем в сторону direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                GameObject instance = Instantiate(data.executionPrefab, projectileSpawnPoint.position, rotation);

                // Передаем префабу информацию о его уроне
                if (instance.TryGetComponent(out Abilities.Projectile logic))
                {
                    logic.Initialize(data, gameObject);
                }
            }
        }
    }
}