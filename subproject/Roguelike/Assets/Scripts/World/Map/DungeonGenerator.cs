using System.Collections.Generic;
using Roguelike.Core.Enums;
using Roguelike.Data;
using UnityEngine;

namespace Roguelike.World.Map
{
    public class DungeonGenerator : MonoBehaviour
    {
        public DungeonGeneratorData data;

        [Tooltip("Размер комнаты в Unity Units для правильного расположения в пространстве")]
        public float roomSpacing = 18f;

        // Храним ссылки на логику сгенерированных комнат
        private Dictionary<Vector2Int, DungeonRoom> instantiatedRooms = new Dictionary<Vector2Int, DungeonRoom>();

        void Awake()
        {
            GenerateDungeon();
        }
        public void GenerateDungeon()
        {
            // Очистка старого подземелья
            foreach (var room in instantiatedRooms)
            {
                if (room.Value != null)
                    Destroy(room.Value.gameObject);
            }
            instantiatedRooms.Clear();

            // Формируем пул всех комнат, которые нужно создать
            List<DungeonRoomData> roomPool = new List<DungeonRoomData>();
            DungeonRoomData startRoomData = null;
            DungeonRoomData endRoomData = null;

            foreach (var group in data.roomGroups)
            {
                for (int i = 0; i < group.count; i++)
                {
                    // Выбираем случайную вариацию комнаты из коллекции
                    var randomVariant = group.roomVariants[Random.Range(0, group.roomVariants.Count)];

                    if (group.roomType == RoomType.Start)
                        startRoomData = randomVariant;
                    else if (group.roomType == RoomType.End)
                        endRoomData = randomVariant;
                    else
                        roomPool.Add(randomVariant);
                }
            }

            if (startRoomData == null || endRoomData == null)
            {
                Debug.LogError("В настройках не задана стартовая или конечная комната!");
                return;
            }

            // Виртуальная генерация, пока не ставим комнаты
            Dictionary<Vector2Int, DungeonRoomData> gridMap = new Dictionary<Vector2Int, DungeonRoomData>();
            List<Vector2Int> availablePositions = new List<Vector2Int>();

            // Вспомогательная функция добавления соседних клеток
            void AddAvailableNeighbors(Vector2Int pos)
            {
                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                foreach (var dir in directions)
                {
                    Vector2Int neighbor = pos + dir;
                    // если у нас нет комнаты на карте и у нас нет этого места как доступного соседа
                    if (!gridMap.ContainsKey(neighbor) && !availablePositions.Contains(neighbor))
                    {
                        availablePositions.Add(neighbor);
                    }
                }
            }

            // ставим стартовую комнату
            gridMap.Add(Vector2Int.zero, startRoomData);
            AddAvailableNeighbors(Vector2Int.zero);

            // Ставим остальные комнаты (кроме конца)
            while (roomPool.Count > 0)
            {
                // Берем случайную доступную позицию
                int randPosIndex = Random.Range(0, availablePositions.Count);
                Vector2Int currentPos = availablePositions[randPosIndex];
                availablePositions.RemoveAt(randPosIndex);

                // Берем случайную комнату из пула
                int randRoomIndex = Random.Range(0, roomPool.Count);
                DungeonRoomData currentRoom = roomPool[randRoomIndex];
                roomPool.RemoveAt(randRoomIndex);

                gridMap.Add(currentPos, currentRoom);
                AddAvailableNeighbors(currentPos);
            }

            // Позиционируем конечную комнату (ищем самую дальнюю доступную клетку от старта)
            Vector2Int endPosition = Vector2Int.zero;
            float maxDistance = -1f;

            foreach (var pos in availablePositions)
            {
                float distance = Vector2Int.Distance(Vector2Int.zero, pos);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    endPosition = pos;
                }
            }
            gridMap.Add(endPosition, endRoomData);


            // Инстанцирование префабов комнат
            foreach (var kvp in gridMap)
            {
                Vector2Int gridPos = kvp.Key;
                DungeonRoomData roomData = kvp.Value;

                // Переводим координаты сетки в мировые координаты с учетом размера комнаты
                Vector3 worldPos = new Vector3(gridPos.x * roomSpacing, gridPos.y * roomSpacing, 0);

                GameObject roomGo = Instantiate(roomData.roomPrefab, worldPos, Quaternion.identity, transform);
                roomGo.name = $"Room_{roomData.roomType}_{gridPos.x}_{gridPos.y}";

                DungeonRoom roomComponent = roomGo.GetComponent<DungeonRoom>();
                if (roomComponent != null)
                {
                    instantiatedRooms.Add(gridPos, roomComponent);
                }
                else
                {
                    Debug.LogWarning($"На префабе {roomData.roomPrefab.name} нет компонента DungeonRoom!");
                }
            }

            // Обновление дверей / проходов
            UpdateRoomConnections(gridMap);
        }

        private void UpdateRoomConnections(Dictionary<Vector2Int, DungeonRoomData> gridMap)
        {
            foreach (var kvp in instantiatedRooms)
            {
                Vector2Int pos = kvp.Key;
                DungeonRoom room = kvp.Value;

                // Проверяем, есть ли комнаты в соседних клетках сетки
                bool hasTop = gridMap.ContainsKey(pos + Vector2Int.up);
                bool hasBottom = gridMap.ContainsKey(pos + Vector2Int.down);
                bool hasLeft = gridMap.ContainsKey(pos + Vector2Int.left);
                bool hasRight = gridMap.ContainsKey(pos + Vector2Int.right);

                // Настраиваем стены и двери
                room.SetupDoors(hasTop, hasBottom, hasLeft, hasRight);
            }
        }
    }
}