using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGeneration
{
    [SelectionBase]
    public class RoomData : MonoBehaviour
    {
        [field: SerializeField] public int Width { get; set; }
        [field: SerializeField] public int Height { get; set; }
        [field: SerializeField] public int Depth { get; set; }
        [field: SerializeField] public List<DoorData> AvailableDoors { get; set; }
        public List<Transition> Transitions => transitions;
        [ReadOnly]
        [SerializeField] private List<Transition> transitions = new List<Transition>();
        [field: SerializeField] public int TransitionGroupId { get; set; } = 0;
        [field: SerializeField] public bool RandomRotation { get; set; } = true;
        [SerializeField] private bool limitedDoorUse = false; 
        [field: SerializeField] public List<ExtraRoomSpawningRequest> AssociatedRooms { get; set; }
        [field: SerializeField] public virtual DoorType DoorType { get; set; } = DoorType.Default;

        [SerializeField] private List<Transform> enemiesSpawnPoints = new List<Transform>();
        public void AddBidirectionalTransition(RoomData roomData, DoorData doorPos, DoorData otherDoorPos)
        {
            if (!transitions.Where(x => x.NextRoom == roomData).Any())
            {
                var transition = new Transition { NextRoom = roomData, Door = doorPos };
                transitions.Add(transition);
                if (limitedDoorUse)
                {
                    doorPos.Uses++;
                }
            }
            print(roomData + " " + transitions);
            if (!roomData.transitions.Where(x => x.NextRoom == this).Any()) 
            {
                var transition = new Transition { NextRoom = this, Door = otherDoorPos };
                roomData.transitions.Add(transition);
                if (roomData.limitedDoorUse)
                {
                    otherDoorPos.Uses++;
                }
            }
        }
        public float OptimalDoorDistance(RoomData other, out DoorData doorPos, out DoorData otherDoorPos)
        {
            float minDistance = float.MaxValue;
            DoorData selectedDoor = null;
            DoorData selectedOtherDoor = null;
            foreach (var door in AvailableDoors.Where(x => !limitedDoorUse || x.Uses < x.MaxUses))
            {
                foreach (var otherDoor in other.AvailableDoors.Where(x => !other.limitedDoorUse || x.Uses < x.MaxUses))
                {
                    if (Mathf.Abs(door.Transform.position.y - otherDoor.Transform.position.y) > 0.01f)
                    {
                        continue;
                    }
                    float distance = Vector3.Distance(door.Transform.position, otherDoor.Transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedDoor = door;
                        selectedOtherDoor = otherDoor;
                    }
                }
            }
            doorPos = selectedDoor;
            otherDoorPos = selectedOtherDoor;
            return minDistance;
        }
        public virtual void Initialize(FloorGenerator parentGenerator)
        {
            transitions = new List<Transition>();
            SpawnManager.instance.enemyPositions.AddRange(enemiesSpawnPoints);
        }
    }
}