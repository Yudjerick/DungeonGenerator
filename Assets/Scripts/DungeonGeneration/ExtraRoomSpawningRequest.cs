using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    [Serializable]
    public class ExtraRoomSpawningRequest
    {
        [field: SerializeField] public RoomData ExtraRoom { get; set; }
        [field: SerializeField] public int FloorOffset { get; set; } = 1;

    }
}