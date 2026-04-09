using System;
using System.Collections.Generic;
using Roguelike.Core.Enums;
using UnityEngine;

namespace Roguelike.Data
{
    [Serializable]
    public class RoomSettingsGroup
    {
        [Tooltip("Список возможных префабов или данных для этого типа комнат")]
        public RoomType roomType;
        public List<DungeonRoomData> roomVariants;
        [Header("Количество")]
        [Range(0, 20)] public int count;

        // public int GetRandomCount() => UnityEngine.Random.Range(minCount, maxCount + 1);

        // public void OnValidate()
        // {
        //     if (minCount > maxCount) maxCount = minCount;
        // }
    }

    [CreateAssetMenu(fileName = "NewDungeon", menuName = "Roguelike/Data/Dungeon")]
    public class DungeonGeneratorData : ScriptableObject
    {
        public string dungeonName;
        [TextArea] public string description;

        [Header("Настройки подземелья")]
        public float roomAtStepChance;

        [Header("Настройки комнат")]
        public List<RoomSettingsGroup> roomGroups = new List<RoomSettingsGroup>();


        // private void OnValidate()
        // {
        //     foreach (var group in roomGroups)
        //     {
        //         group.OnValidate();
        //     }
        // }
    }
}