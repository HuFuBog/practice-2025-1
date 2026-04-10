using UnityEngine;

namespace Roguelike.World.Map
{
    public class DungeonRoom : MonoBehaviour
    {
        [Header("Двери / Проходы (включить, если есть сосед)")]
        public GameObject topDoor;
        public GameObject bottomDoor;
        public GameObject leftDoor;
        public GameObject rightDoor;

        [Header("Стены (включить, если соседа нет)")]
        public GameObject topWall;
        public GameObject bottomWall;
        public GameObject leftWall;
        public GameObject rightWall;

        // .тот метод вызовет DungeonGenerator после создания всей карты
        public void SetupDoors(bool hasTop, bool hasBottom, bool hasLeft, bool hasRight)
        {
            // если есть комната - открываем проход, скрываем стену
            if (topDoor != null) topDoor.SetActive(hasTop);
            if (topWall != null) topWall.SetActive(!hasTop);

            if (bottomDoor != null) bottomDoor.SetActive(hasBottom);
            if (bottomWall != null) bottomWall.SetActive(!hasBottom);

            if (leftDoor != null) leftDoor.SetActive(hasLeft);
            if (leftWall != null) leftWall.SetActive(!hasLeft);

            if (rightDoor != null) rightDoor.SetActive(hasRight);
            if (rightWall != null) rightWall.SetActive(!hasRight);
        }
    }
}