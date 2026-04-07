using System.Collections.Generic;
using UnityEngine;
// using Roguelike.Data;

namespace Roguelike.World.Map
{
    public class DungeonGenerator : MonoBehaviour
    {
        public List<Data.DungeonRoomData> rooms;
        private List<GameObject> instantiatedRooms;
        void Awake()
        {
            instantiatedRooms = new List<GameObject>();
            InstantiateRooms();
        }
        private void InstantiateRoom(Data.DungeonRoomData data)
        {
            if (data == null) return;

            instantiatedRooms.Add(Instantiate(data.roomPrefab, data.roomPosition, Quaternion.identity, transform));
        }
        private void InstantiateRooms()
        {
            foreach (Data.DungeonRoomData dungeonRoomData in rooms)
            {
                InstantiateRoom(dungeonRoomData);
            }
        }
        public void GenerateDungeon()
        {
            foreach (GameObject gameObject in instantiatedRooms)
            {
                Destroy(gameObject);
            }
            instantiatedRooms = new List<GameObject>();

            InstantiateRooms();
        }
    }
    public struct DungeonRoomUnit
    {
        public Vector3 position;
        public Data.DungeonRoomData data;
    }


}


