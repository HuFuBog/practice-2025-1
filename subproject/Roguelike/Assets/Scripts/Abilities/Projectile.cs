using UnityEngine;
using Roguelike.Core.Interfaces;
using Roguelike.Combat;
using Roguelike.Data;

namespace Roguelike.Abilities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        public float speed = 15f;
        public float lifeTime = 3f;

        private AbilityData abilityData;
        private GameObject source;

        public void Initialize(AbilityData data, GameObject caster)
        {
            abilityData = data;
            source = caster;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            // Летим туда, куда повернут префаб (задано в AbilityController)
            rb.velocity = transform.right * speed;

            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Игнорируем того, кто запустил снаряд
            if (collision.gameObject == source) return;

            // Если у объекта есть система здоровья - наносим урон
            if (collision.TryGetComponent(out IDamageable target))
            {
                DamageInfo damage = new DamageInfo(
                    abilityData.damageAmount,
                    abilityData.damageType,
                    source,
                    false // there crit logic 
                );

                target.TakeDamage(damage);
            }

            // Взрываемся при любом столкновении
            Destroy(gameObject);
        }
    }
}