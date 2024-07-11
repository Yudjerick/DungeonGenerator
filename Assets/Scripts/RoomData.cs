using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[SelectionBase]
public class RoomData : MonoBehaviour
{
    [field: SerializeField] public int Width {get; set;}
    [field: SerializeField] public int Height {get; set;}
    [field: SerializeField] public int Depth {get; set;}

    public List<Transition> Transitions => transitions;  
    [SerializeField] private List<Transition> transitions = new List<Transition>();

    [SerializeField] private List<Transform> availableDoors;

    [field: SerializeField] public List<ExtraRoomSpawningRequest> AssociatedRooms { get; set; }

    public void AddBidirectionalTransition(RoomData roomData, Transform doorPos, Transform otherDoorPos){
        if(!transitions.Where(x => x.NextRoom == roomData).Any()){
            var transition = new Transition {NextRoom = roomData, Door = doorPos};
            transitions.Add(transition);
            //availableDoors.Remove(doorPos);
        }
        if(!roomData.transitions.Where(x => x.NextRoom == this).Any()){
            var transition = new Transition {NextRoom = this, Door = otherDoorPos};
            roomData.transitions.Add(transition);
            //roomData.availableDoors.Remove(otherDoorPos);
        }
    }

    public float OptimalDoorDistance(RoomData other, out Transform doorPos, out Transform otherDoorPos){
        float minDistance = float.MaxValue;
        Transform selectedDoor = null;
        Transform selectedOtherDoor = null;
        foreach(var door in availableDoors){
            foreach (var otherDoor in other.availableDoors){
                if(Mathf.Abs(door.position.y - otherDoor.position.y) > 0.01f)
                {
                    continue;
                }
                float distance = Vector3.Distance(door.position, otherDoor.position);
                if(distance < minDistance){
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

