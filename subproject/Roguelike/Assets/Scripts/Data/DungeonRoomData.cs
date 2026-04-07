using UnityEngine;

namespace Roguelike.Data
{
    [CreateAssetMenu(fileName = "NewDungeonRoom", menuName = "Roguelike/Data/DungeonRoom")]
    public class DungeonRoomData : ScriptableObject
    {
        public string roomName;
        public Vector3 roomPosition;
        [TextArea] public string description;

        public GameObject roomPrefab;
    }
}