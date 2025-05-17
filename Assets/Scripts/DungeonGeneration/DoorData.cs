using NaughtyAttributes;
using System;
using UnityEngine;

namespace DungeonGeneration
{
    /// <summary>
    /// Congiguration for one room doorway
    /// </summary>
    [Serializable]
    public class DoorData
    {
        /// <summary>
        /// Should be placed in position of corridor segment object connected to this doorway.
        /// Forward vector should face room
        /// </summary>
        [field: SerializeField] public Transform Transform { get; set; }
        [SerializeField] private int maxUses;
        public int MaxUses { get => maxUses; set => maxUses = value; }
        [field: SerializeField]
        public int Uses { get; set; } = 0;
        [field: SerializeField] public DoorType doorType { get; set; } = DoorType.Default;
    }
}
