using UnityEngine;
using Roguelike.Core.Interfaces;
using Roguelike.Combat;
using Roguelike.Data;
using Roguelike.World.Map;

namespace Roguelike.Abilities
{
    // базовый класс для всех игровых сущностей, попадающих под название
    // <снаряд>, то есть различные пули и им подобные, в будущем переписать все это добро в абстрактный класс!
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Атрибуты")]
        public float speed = 15f;
        public float lifeTime = 3f;

        private AbilityData abilityData;
        private GameObject source;

        public void Initialize(AbilityData data, GameObject caster)
        {
            abilityData = data;
            source = caster;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            // Летим туда, куда повернут префаб (задано в AbilityController, мы там ставим его сразу повернутым в соотвтетствии с логикой определения ближайшей цели)
            rb.linearVelocity = transform.right * speed;

            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Игнорируем того, кто запустил снаряд, потому что кто хочет получить сам от себя?
            if (collision.gameObject == source) return;
            // мы можем сталкиваться с комнатой(так как она вся имеет коллайдер)
            if (collision.gameObject.tag == "Room") return;

            // Если у объекта есть система здоровья - наносим урон, очевидно это враг
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

            Destroy(gameObject);
        }
    }
}