using UnityEngine;

namespace Roguelike.Actors.Enemies
{
    public class SheepEnemy : EnemyBase
    {
        // реализуем простую стейт-машину, меняем состояния,
        // переодически идем, отдыхаем
        private enum SheepState { Idle, Wander }

        [Header("Настройки поведения Овечки")]
        public float minIdleTime = 1f;
        public float maxIdleTime = 3f;
        public float minWanderTime = 1.5f;
        public float maxWanderTime = 4f;

        private SheepState currentState;
        private float stateTimer;
        private Vector2 moveDirection;

        protected override void Start()
        {
            base.Start();
            SwitchState(SheepState.Idle); // Начинаем с отдыха
        }

        protected override void UpdateBehavior()
        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0)
            {
                // Меняем состояние на противоположное
                if (currentState == SheepState.Idle)
                    SwitchState(SheepState.Wander);
                else
                    SwitchState(SheepState.Idle);
            }
        }

        protected override void FixedUpdateBehavior()
        {
            if (currentState == SheepState.Wander)
            {
                // Берем скорость из CharacterData (которая лежит в HealthSystem)
                float speed = healthSystem.baseData.baseMoveSpeed;
                rb.linearVelocity = moveDirection * speed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        private void SwitchState(SheepState newState)
        {
            currentState = newState;

            if (newState == SheepState.Idle)
            {
                stateTimer = Random.Range(minIdleTime, maxIdleTime);
                rb.linearVelocity = Vector2.zero;
            }
            else if (newState == SheepState.Wander)
            {
                stateTimer = Random.Range(minWanderTime, maxWanderTime);

                // Выбираем случайное направление (угол от 0 до 360 градусов)
                float randomAngle = Random.Range(0f, Mathf.PI * 2f);
                moveDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
            }
        }
    }
}