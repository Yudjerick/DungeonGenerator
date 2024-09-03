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

        [field: SerializeField] public int TransitionGroupId { get; set; } = 0;

        public List<Transition> Transitions => transitions;
        [ReadOnly]
        [SerializeField] private List<Transition> transitions = new List<Transition>();

        [OnValueChanged("UpdateDoorDataInspectorView")][SerializeField] private bool limitedDoorUse = false; 
        [field: SerializeField] public List<DoorData> AvailableDoors { get; set; }

        [field: SerializeField] public List<ExtraRoomSpawningRequest> AssociatedRooms { get; set; }

        private void UpdateDoorDataInspectorView()
        {
            foreach (DoorData doorData in AvailableDoors)
            {
                doorData.ShowMaxUses = limitedDoorUse;
            }
        }

        public void AddBidirectionalTransition(RoomData roomData, DoorData doorPos, DoorData otherDoorPos)
        {
            if (!transitions.Where(x => x.NextRoom == roomData).Any())
            {
                var transition = new Transition { NextRoom = roomData, Door = doorPos };
                transitions.Add(transition);
                if (limitedDoorUse)
                {
                    
                }
            }
            if (!roomData.transitions.Where(x => x.NextRoom == this).Any())
            {
                var transition = new Transition { NextRoom = this, Door = otherDoorPos };
                roomData.transitions.Add(transition);
                if (roomData.limitedDoorUse)
                {
                    
                }
            }
        }

        public float OptimalDoorDistance(RoomData other, out DoorData doorPos, out DoorData otherDoorPos)
        {
            float minDistance = float.MaxValue;
            DoorData selectedDoor = null;
            DoorData selectedOtherDoor = null;
            foreach (var door in AvailableDoors)
            {
                foreach (var otherDoor in other.AvailableDoors)
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
    }
}