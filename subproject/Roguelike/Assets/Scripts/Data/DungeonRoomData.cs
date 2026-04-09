using System;
using Roguelike.Core.Enums;
using UnityEngine;

namespace Roguelike.Data
{
    [CreateAssetMenu(fileName = "NewDungeonRoom", menuName = "Roguelike/Data/DungeonRoom")]
    public class DungeonRoomData : ScriptableObject
    {
        public RoomType roomType;
        public string roomName;
        public Vector2Int roomPosition;
        [TextArea] public string description;

        public GameObject roomPrefab;
    }
}