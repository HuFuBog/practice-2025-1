using UnityEngine;
using Roguelike.Core.Enums;

namespace Roguelike.World.Map
{
    public class DungeonRoom : MonoBehaviour
    {
        public RoomType RoomType { get; private set; }

        [Header("Двери (открытые проходы)")]
        public GameObject topDoor;
        public GameObject bottomDoor;
        public GameObject leftDoor;
        public GameObject rightDoor;

        [Header("Стены (если соседа нет)")]
        public GameObject topWall;
        public GameObject bottomWall;
        public GameObject leftWall;
        public GameObject rightWall;

        [Header("Закрытые двери (появляются во время боя)")]
        public GameObject topDoorLocked;
        public GameObject bottomDoorLocked;
        public GameObject leftDoorLocked;
        public GameObject rightDoorLocked;

        private bool hasTop, hasBottom, hasLeft, hasRight;

        public void Initialize(RoomType type)
        {
            RoomType = type;
        }

        public void SetupDoors(bool top, bool bottom, bool left, bool right)
        {
            hasTop = top; hasBottom = bottom; hasLeft = left; hasRight = right;

            if (topDoor != null) topDoor.SetActive(hasTop);
            if (topWall != null) topWall.SetActive(!hasTop);
            if (topDoorLocked != null) topDoorLocked.SetActive(false);

            if (bottomDoor != null) bottomDoor.SetActive(hasBottom);
            if (bottomWall != null) bottomWall.SetActive(!hasBottom);
            if (bottomDoorLocked != null) bottomDoorLocked.SetActive(false);

            if (leftDoor != null) leftDoor.SetActive(hasLeft);
            if (leftWall != null) leftWall.SetActive(!hasLeft);
            if (leftDoorLocked != null) leftDoorLocked.SetActive(false);

            if (rightDoor != null) rightDoor.SetActive(hasRight);
            if (rightWall != null) rightWall.SetActive(!hasRight);
            if (rightDoorLocked != null) rightDoorLocked.SetActive(false);
        }

        public void LockDoors()
        {
            // Закрываем только те проходы, которые ведут в другие комнаты
            if (hasTop && topDoorLocked) { topDoor.SetActive(false); topDoorLocked.SetActive(true); }
            if (hasBottom && bottomDoorLocked) { bottomDoor.SetActive(false); bottomDoorLocked.SetActive(true); }
            if (hasLeft && leftDoorLocked) { leftDoor.SetActive(false); leftDoorLocked.SetActive(true); }
            if (hasRight && rightDoorLocked) { rightDoor.SetActive(false); rightDoorLocked.SetActive(true); }
        }

        public void UnlockDoors()
        {
            if (hasTop && topDoorLocked) { topDoor.SetActive(true); topDoorLocked.SetActive(false); }
            if (hasBottom && bottomDoorLocked) { bottomDoor.SetActive(true); bottomDoorLocked.SetActive(false); }
            if (hasLeft && leftDoorLocked) { leftDoor.SetActive(true); leftDoorLocked.SetActive(false); }
            if (hasRight && rightDoorLocked) { rightDoor.SetActive(true); rightDoorLocked.SetActive(false); }
        }
    }
}