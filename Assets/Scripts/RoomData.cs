using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomData : MonoBehaviour
{
    [Serializable]
    struct Transition{
        public RoomData nextRoom;
        public Transform door;
    }

    [field: SerializeField] public int Width {get; set;}
    [field: SerializeField] public int Height {get; set;}
    [field: SerializeField] public int Depth {get; set;}

    [SerializeField] private List<Transition> transitions = new List<Transition>();

    [SerializeField] private List<Transform> availableDoors;

    public void AddBidirectionalTransition(RoomData roomData, Transform doorPos, Transform otherDoorPos){
        if(!transitions.Where(x => x.nextRoom == roomData).Any()){
            var transition = new Transition {nextRoom = roomData, door = doorPos};
            transitions.Add(transition);
            availableDoors.Remove(doorPos);
        }
        if(!roomData.transitions.Where(x => x.nextRoom == this).Any()){
            var transition = new Transition {nextRoom = this, door = otherDoorPos};
            transitions.Add(transition);
            roomData.availableDoors.Remove(otherDoorPos);
        }
    }

    public Vector3 GetCenter(float gridCellSize){
        return transform.position + (new Vector3(Width, Height, Depth) / 2 * gridCellSize);
    }

    public float OptimalDoorDistance(RoomData other, out Transform doorPos, out Transform otherDoorPos){
        float minDistance = float.MaxValue;
        Transform selectedDoor = null;
        Transform selectedOtherDoor = null;
        foreach(var door in availableDoors){
            foreach (var otherDoor in other.availableDoors){
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

