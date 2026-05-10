using UnityEngine;
using Roguelike.Combat;

namespace Roguelike.Actors.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class EnemyBase : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected HealthSystem healthSystem;
        protected Transform playerTarget;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            healthSystem = GetComponent<HealthSystem>();

            // настройки физики для top-down игры
            rb.gravityScale = 0f;
            rb.freezeRotation = true; // Чтобы враги не крутились от столкновений

            // Подписываемся на событие смерти из нашей системы здоровья
            healthSystem.OnDeath += HandleDeath;
        }

        protected virtual void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
            }
        }

        protected virtual void Update()
        {
            UpdateBehavior();
        }

        protected virtual void FixedUpdate()
        {
            FixedUpdateBehavior();
        }

        // Эти методы обязаны реализовать наследники (овечка, гоблин, босс и т.д.)
        protected abstract void UpdateBehavior();
        protected virtual void FixedUpdateBehavior() { }
        protected virtual void HandleDeath()
        {
            // НЕ УДАЛЯТЬ ЗДЕСЬ ОБЪЕКТ - этим занимается HealthSystem.Die();
            // Отписываемся, чтобы избежать утечек памяти
            healthSystem.OnDeath -= HandleDeath;

            // Здесь в будущем можно будет добавить
            // Спавн партиклов крови/взрыва
            // Звук смерти
            // Выпадение лута (монетки, опыт, мана и т. д.)
            // ВАЖНО!!!! - (Сам объект удаляется внутри HealthSystem.Die(), так что тут Destroy не нужен)
        }
    }
}