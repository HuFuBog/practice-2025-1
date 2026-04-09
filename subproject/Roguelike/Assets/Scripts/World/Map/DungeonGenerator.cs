using System;
using System.Collections.Generic;
using Roguelike.Core.Enums;
using Roguelike.Data;
using Unity.VisualScripting;
using UnityEngine;
// using Roguelike.Data;

namespace Roguelike.World.Map
{
    public class DungeonGenerator : MonoBehaviour
    {
        public DungeonGeneratorData data;
        // public List<Data.DungeonRoomData> rooms;
        private Dictionary<Vector2Int, GameObject> instantiatedRooms;
        void Awake()
        {
            instantiatedRooms = new Dictionary<Vector2Int, GameObject>();
        }
        private void InstantiateRoom(DungeonRoomData data)
        {
            if (data == null) return;

            instantiatedRooms[data.roomPosition] = Instantiate(data.roomPrefab, (Vector2)data.roomPosition, Quaternion.identity, transform);
        }
        public void GenerateDungeon()
        {
            foreach (var room in instantiatedRooms)
            {
                Destroy(room.Value);
            }
            instantiatedRooms = new Dictionary<Vector2Int, GameObject>();



            Vector2Int pointer = new Vector2Int();
            Dictionary<RoomType, int> typeRoomCounts = new Dictionary<RoomType, int>();
            foreach (RoomSettingsGroup roomSettingsGroup in data.roomGroups)
            {
                typeRoomCounts[roomSettingsGroup.roomType] += roomSettingsGroup.count;
            }

            List<Vector2Int> pointerTrace = new List<Vector2Int>();

            bool done = false;
            List<Vector2Int> m = new List<Vector2Int>()
            {
                new Vector2Int(0,1),
                new Vector2Int(1,0),
                new Vector2Int(0,-1),
                new Vector2Int(-1,0),
            };
            while (!done)
            {
                if (((Func<int>)(() => { int sum = 0; foreach (KeyValuePair<RoomType, int> keyValuePair in typeRoomCounts) { sum += keyValuePair.Value; } return sum; }))() == 0)
                {
                    break;
                }

            }


            //first room always is start and at V2(0;0)




        }
    }
    public struct DungeonRoomUnit
    {
        public Vector3 position;
        public DungeonRoomData data;
    }


}


