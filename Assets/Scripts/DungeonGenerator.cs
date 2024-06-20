using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int gap = 1;
    [SerializeField] private float gridCellSize = 1;
    [SerializeField] private int dungeonWidth;
    [SerializeField] private int dungeonHeight;
    [SerializeField] private int dungeonDepth;
    
    [SerializeField] private List<RoomData> roomRefs = new List<RoomData>();

    [SerializeField] private GameObject corridor;
    [SerializeField] int x1;
    [SerializeField] int x2;

    private List<RoomData> rooms = new List<RoomData>();
    private bool[,,] solidMap;
    void Awake(){
        
    }

    private void Start() {
        Generate();
    }
    
    public bool CanPlaceRoom(int x, int y, int z, int width, int height, int depth){
        if(x+width+gap >= dungeonWidth || y+height+gap >= dungeonHeight || z+depth+gap >= dungeonDepth)
        {
            return false;
        }
        if(x-gap < 0 || y-gap < 0 || z-gap < 0){
            return false;
        }
        for (int i = -gap; i < width + gap; i++){
            for (int j = -gap; j < height + gap; j++){
                for(int k = -gap; k < depth + gap; k++){
                    if(solidMap[x+i,y+j,z+k]){
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public RoomData PlaceRoom(int x, int y, int z, RoomData room){
        for (int i = 0; i < room.Width; i++){
            for (int j = 0; j < room.Height; j++){
                for(int k = 0; k < room.Depth; k++){
                    
                    solidMap[x+i,y+j,z+k] = true;
                }
            }
        }
        var RoomDimensions = Instantiate(room, new Vector3(x,y,z) * gridCellSize, Quaternion.identity);
        return RoomDimensions;
    }

    

    public void SetMinimalTransitions(){
        List<RoomData> unexplored = new List<RoomData>(rooms);
        List<RoomData> explored = new List<RoomData>();

        RoomData firstRoom = unexplored[0];
        explored.Add(firstRoom);
        unexplored.Remove(firstRoom);

        while (unexplored.Count > 0){
            RoomData selectedFrom = explored[0];
            RoomData selectedTo = unexplored[0];
            Transform selectedFromDoor = null;
            Transform selectedToDoor = null;
            float minDistance = float.MaxValue;
            foreach (var from in explored){
                foreach (var to in unexplored){
                    float distance = from.OptimalDoorDistance(to, out Transform doorPos, out Transform otherDoorPos);
                    if (distance < minDistance){
                        minDistance = distance;
                        selectedFrom = from;
                        selectedTo = to;
                        selectedFromDoor = doorPos;
                        selectedToDoor = otherDoorPos;
                    }
                }
            }
            selectedFrom.AddBidirectionalTransition(selectedTo, selectedFromDoor, selectedToDoor);
            explored.Add(selectedTo);
            unexplored.Remove(selectedTo);
            Debug.DrawLine( selectedFromDoor.position, selectedToDoor.position, Color.green, 100000f);
        }
    }
    
    public void MakePathes(){
        foreach (var room in rooms){
            foreach(var transition in room.Transitions){
                if(!transition.PathBuilt){
                    Vector3 start = transition.Door.position;
                    Transition endTransition = transition.NextRoom.Transitions.Where(t => t.NextRoom == room).FirstOrDefault();
                    Vector3 end = endTransition.Door.position;
                    Pathfinder pathfinder = new Pathfinder(solidMap, (int)start.x, (int)start.y, (int)start.z,
                        (int)end.x, (int)end.y, (int)end.z);
                    
                    List<Vector3> path = pathfinder.FindPath();
                    if(path != null){
                        foreach (var pos in path){
                            Instantiate(corridor, pos, quaternion.identity);
                            solidMap[(int)pos.x, (int)pos.y, (int)pos.z] = true;
                        }
                        transition.PathBuilt = true;
                        endTransition.PathBuilt = true;
                    }
                    else{
                        print("A");
                    }
                    
                }
            }
        }
    }

    public void Generate(){
        solidMap = new bool[dungeonWidth, dungeonHeight, dungeonDepth];
        Random random = new Random();
        
        foreach (RoomData room in roomRefs){
            int x = random.Next(dungeonWidth);
            int y = 10;
            //int y = random.Next(3) * 8;
            int z = random.Next(dungeonDepth);
            int tryCount = 0;
            bool roomSkiped = false;
            while(!CanPlaceRoom(x,y,z,room.Width, room.Height, room.Depth)){
                if(tryCount > 10000){
                    print(x + " " + y + " " + z);
                    print("Skipping room");
                    roomSkiped = true;
                    break;
                }
                tryCount++;
                x = random.Next(dungeonWidth);
                //y = random.Next(3) * 8;
                y = 10;
                z = random.Next(dungeonDepth);
            }
            if(!roomSkiped){
                rooms.Add(PlaceRoom(x,y,z,room));
            }
            
        }

        SetMinimalTransitions();
        MakePathes();
    }
}


