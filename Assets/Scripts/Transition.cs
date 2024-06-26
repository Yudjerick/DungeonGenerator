using System;
using UnityEngine;

[Serializable]
public class Transition{
    [field: SerializeField] public RoomData NextRoom {get; set;}
    [field: SerializeField] public Transform Door {get; set;}

    [field: SerializeField] public bool PathBuilt {get; set;} = false;
}