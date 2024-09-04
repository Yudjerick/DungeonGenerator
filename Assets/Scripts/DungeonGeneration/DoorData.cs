using NaughtyAttributes;
using System;
using UnityEngine;

namespace DungeonGeneration
{
    [Serializable]
    public class DoorData
    {
        [field: SerializeField] public Transform Transform { get; set; }

        //[AllowNesting]
        //[ShowIf("ShowMaxUses")]
        [SerializeField] private int maxUses;
        public int MaxUses { get => maxUses; set => maxUses = value; }
        [field: SerializeField]
        public int Uses { get; set; } = 0;

        public bool ShowMaxUses { get; set; }
        
    }
}
