using System;
using System.Collections.Generic;
using UnityEngine;
using Roguelike.Core.Enums;
using Roguelike.Combat;
using Roguelike.Actors.Player;

namespace Roguelike.World.Map
{
    [Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemiesToSpawn;
    }

    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(DungeonRoom))]
    public class RoomEncounter : MonoBehaviour
    {
        [Header("Настройки спавна (Волны)")]
        public List<EnemyWave> waves;

        [Header("Точки спавна")]
        [Tooltip("Точки, расставленные вручную в комнате")]
        // ###############################
        // Спавн объектов происходит по
        // фиксированным позициям с целью
        // невозможности спавна врагов в
        // стенах
        // ###############################
        public List<Transform> spawnPoints;
        public Transform rewardSpawnPoint;

        [Header("Префабы наград")]
        public GameObject chestPrefab;
        public GameObject questPrefab;

        private DungeonRoom room;
        private bool isCleared = false;
        private bool isActive = false;
        private int currentWaveIndex = 0;
        private int activeEnemiesCount = 0;

        private void Awake()
        {
            room = GetComponent<DungeonRoom>();

            // Убеждаемся, что коллайдер является триггером
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Если комната уже пройдена или бой идет - игнорируем
            if (isCleared || isActive) return;

            if (collision.GetComponent<PlayerController>() != null)
            {
                StartEncounter();
            }
        }

        private void StartEncounter()
        {
            RoomType type = room.RoomType;

            // Проверяем, подразумевает ли комната врагов
            bool hasEnemies = type == RoomType.SmallWithEnemy ||
                              type == RoomType.SmallWithEnemyAndChest ||
                              type == RoomType.SmallWithEnemyAndQuest;

            if (hasEnemies && waves.Count > 0)
            {
                isActive = true;
                room.LockDoors();
                currentWaveIndex = 0;
                SpawnNextWave();
            }
            else
            {
                // Если врагов нет, сразу завершаем комнату (спавним сундук, если нужен)
                CompleteEncounter();
            }
        }

        private void SpawnNextWave()
        {
            if (currentWaveIndex >= waves.Count)
            {
                CompleteEncounter();
                return;
            }

            EnemyWave currentWave = waves[currentWaveIndex];
            activeEnemiesCount = currentWave.enemiesToSpawn.Count;

            if (activeEnemiesCount == 0)
            {
                // Защита от пустых волн
                currentWaveIndex++;
                SpawnNextWave();
                return;
            }

            for (int i = 0; i < currentWave.enemiesToSpawn.Count; i++)
            {
                // остаток от деления - индекс, это защита от ситуации, если врагов больше, чем мест спавна 
                Transform spawnPoint = spawnPoints[i % spawnPoints.Count];

                GameObject enemyObj = Instantiate(currentWave.enemiesToSpawn[i], spawnPoint.position, Quaternion.identity, transform);

                // Подписываемся на смерть врага
                if (enemyObj.TryGetComponent(out HealthSystem health))
                {
                    health.OnDeath += OnEnemyDefeated;
                }
                else
                {
                    Debug.LogWarning($"У префаба врага {enemyObj.name} нет HealthSystem! Комната может сломаться.");
                }
            }

            currentWaveIndex++;
        }

        private void OnEnemyDefeated()
        {
            activeEnemiesCount--;

            if (activeEnemiesCount <= 0)
            {
                SpawnNextWave();
            }
        }

        private void CompleteEncounter()
        {
            isActive = false;
            isCleared = true;
            room.UnlockDoors();

            RoomType type = room.RoomType;

            // Спавн сундука
            if ((type == RoomType.SmallWithChest || type == RoomType.SmallWithEnemyAndChest) && chestPrefab != null)
            {
                Instantiate(chestPrefab, rewardSpawnPoint.position, Quaternion.identity, transform);
            }

            // Спавн квеста
            if ((type == RoomType.SmallWithQuest || type == RoomType.SmallWithEnemyAndQuest) && questPrefab != null)
            {
                Instantiate(questPrefab, rewardSpawnPoint.position, Quaternion.identity, transform);
            }
        }
    }
}