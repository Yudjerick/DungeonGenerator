using System;
using UnityEngine;

namespace DungeonGeneration
{
    [Serializable]
    public class Transition
    {
        [field: SerializeField] public RoomData NextRoom { get; set; }
        [field: SerializeField] public DoorData Door { get; set; }

        [field: SerializeField] public bool PathBuilt { get; set; } = false;
    }
}