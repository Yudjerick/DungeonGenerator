using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] private int gap = 1;
    [SerializeField] private float gridCellSize = 1;
    [SerializeField] private int dungeonWidth;
    [SerializeField] private int dungeonHeight;
    [SerializeField] private int dungeonDepth;
    [SerializeField] private int levelY = 10;
    [SerializeField] private int loopCount = 3;
    [SerializeField] private int minimalLoopLength = 3;
    [SerializeField] private List<RoomData> roomRefs = new List<RoomData>();
    [SerializeField] private CorridorSegmentPack segmentPack;
    [SerializeField] private GameObject testCorridor;
    private List<CorridorDirection>[,] _corridorMap;
    private DungeonGenerator _dungeonGenerator;
    private int _floorIndex;

    public bool[,] SolidMap { get; set; }
    public List<RoomData> Rooms { get; set; } = new List<RoomData>();
    public GameObject Floor { get; set; }
    public int Gap { get => gap; set => gap = value; }
    public float GridCellSize { get => gridCellSize; set => gridCellSize = value; }
    public int DungeonWidth { get => dungeonWidth; set => dungeonWidth = value; }
    public int DungeonHeight { get => dungeonHeight; set => dungeonHeight = value; }
    public int DungeonDepth { get => dungeonDepth; set => dungeonDepth = value; }
    public int LevelY { get => levelY; set => levelY = value; }
    public List<RoomData> RoomRefs { get => roomRefs; set => roomRefs = value; }
    public DungeonGenerator DungeonGenerator { get => _dungeonGenerator; set => _dungeonGenerator = value; }
    public int FloorIndex { get => _floorIndex; set => _floorIndex = value; }

    public void Initialize(DungeonGenerator dungeonGenerator, int floorIndex)
    {
        FloorIndex = floorIndex;
        DungeonGenerator = dungeonGenerator;
        Floor = new GameObject("Floor");
        SolidMap = new bool[DungeonWidth, DungeonDepth];
        _corridorMap = new List<CorridorDirection>[DungeonWidth, DungeonDepth];
    }

    public void SetMinimalTransitions(){
        List<RoomData> unexplored = new List<RoomData>(Rooms);
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
            Debug.DrawLine( selectedFromDoor.position, selectedToDoor.position, Color.green, 100000f); //Null reference here
        }
    }

    private List<float> HopsToRooms(int startRoomIndex)
    {
        List<RoomData> unexplored = new List<RoomData>(Rooms);
        List<RoomData> explored = new List<RoomData>();
        List<float> distances = Rooms.Select(x => float.PositiveInfinity).ToList();
        distances[startRoomIndex] = 0;
        int currentRoomIndex = startRoomIndex;
        while(explored.Count < Rooms.Count) 
        {
            explored.Add(Rooms[currentRoomIndex]);
            unexplored.Remove(Rooms[currentRoomIndex]);
            foreach (var transition in Rooms[currentRoomIndex].Transitions) 
            {
                int nextRoomIndex = Rooms.IndexOf(transition.NextRoom);
                distances[nextRoomIndex] = Mathf.Min(distances[currentRoomIndex] + 1, distances[nextRoomIndex]);
            }
            foreach (var unexploredRoom in unexplored)
            {
                if(unexploredRoom.Transitions.Select(x => x.NextRoom).Where(y => explored.Contains(y)).Any())
                {
                    currentRoomIndex = Rooms.IndexOf(unexploredRoom);
                    break;
                }
            }
        }
        return distances;
    }

    public void AddLoops(int loopCount)
    {
        int loopsCreated = 0;
        while (loopCount > 0) 
        {
            RoomData selectedTo = null;
            Transform selectedFromDoor = null;
            Transform selectedToDoor = null;
            float minDistance = float.MaxValue;

            RoomData fromRoom = Rooms.OrderBy(x => x.Transitions.Count).ToList()[loopsCreated];
            int fromRoomIndex = Rooms.IndexOf(fromRoom);

            var hops = HopsToRooms(fromRoomIndex);

            foreach (var room in Rooms)
            {
                int toRoomIndex = Rooms.IndexOf(room);
                if (fromRoom == room || fromRoom.Transitions.Any(x => x.NextRoom == room) || hops[toRoomIndex] < minimalLoopLength)
                {
                    continue;
                }
                float distance = fromRoom.OptimalDoorDistance(room, out Transform doorPos, out Transform otherDoorPos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    selectedTo = room;
                    selectedFromDoor = doorPos;
                    selectedToDoor = otherDoorPos;
                }
            }
            fromRoom.AddBidirectionalTransition(selectedTo, selectedFromDoor, selectedToDoor);
            loopCount--;
            loopsCreated++;
            Debug.DrawLine(selectedFromDoor.position, selectedToDoor.position, Color.yellow, 100000f);
        }
    }

    public void MakePathes(){
        foreach (var room in Rooms){
            foreach(var transition in room.Transitions){
                if(!transition.PathBuilt){
                    Vector3 start = transition.Door.position / GridCellSize;
                    start.y = 0;
                    Transition endTransition = transition.NextRoom.Transitions.Where(t => t.NextRoom == room).FirstOrDefault();
                    Vector3 end = endTransition.Door.position / GridCellSize;
                    end.y = 0;
                    Pathfinder pathfinder = new Pathfinder(SolidMap, (int)Mathf.Round(start.x), (int)Mathf.Round(start.z), (int)Mathf.Round(end.x), (int)Mathf.Round(end.z));
                    
                    List<Vector3> path = pathfinder.FindPath();
                   
                    

                    if (path != null){
                        path.Add(start + transition.Door.forward);
                        path.Insert(0, end + endTransition.Door.forward);

                        for (int i = 1; i < path.Count - 1; i++){
                            int x = (int)Mathf.Round(path[i].x);
                            int y = (int)Mathf.Round(path[i].y);
                            int z = (int)Mathf.Round(path[i].z);
                            if (_corridorMap[x, z] == null)
                            {
                                _corridorMap[x, z] = new List<CorridorDirection>();
                            }
                            _corridorMap[x, z].Add(CorridorSegmentPack.Vector3ToCorridorDirection(path[i - 1] - path[i]));
                            _corridorMap[x, z].Add(CorridorSegmentPack.Vector3ToCorridorDirection(path[i + 1] - path[i]));

                        }
                        transition.PathBuilt = true;
                        endTransition.PathBuilt = true;
                    }
                    else{
                        print("Can't build path");
                    }
                    
                }
            }
        }
    }

    public void InstantiateCorridors()
    {
        var corridor = new GameObject("Corridor");
        corridor.transform.parent = Floor.transform;
        for (int i = 0; i < DungeonWidth; i++)
        {
            for (int k = 0; k < DungeonDepth; k++)
            {
                if (_corridorMap[i, k] != null)
                {
                    Instantiate(segmentPack.GetSegment(_corridorMap[i, k]), new Vector3(i * gridCellSize, LevelY, k * gridCellSize), Quaternion.identity, corridor.transform);

                }
            }

        }
    }

    public void AddCorridorsToUnusedDoors()
    {
        foreach(RoomData room in Rooms)
        {
            List<Transform> unusedDoors = room.AvailableDoors.Where(d => !room.Transitions.Select(x => x.Door).Contains(d)).ToList();
            foreach(Transform unusedDoor in unusedDoors)
            {
                int x = (int)Mathf.Round(unusedDoor.position.x / gridCellSize);
                int z = (int)Mathf.Round(unusedDoor.position.z / gridCellSize);
                if (_corridorMap[x, z] == null)
                {
                    _corridorMap[x, z] = new List<CorridorDirection>();
                }
                _corridorMap[x, z].Add(CorridorSegmentPack.Vector3ToCorridorDirection(unusedDoor.forward));
            }
        }
    }

    public void Generate(){

        int seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);
        RoomPlacing roomPlacing = new RoomPlacing();
        roomPlacing.PlaceAllRoomsPush(levelY, this);
        SetMinimalTransitions();
        AddLoops(loopCount);
        MakePathes();
        AddCorridorsToUnusedDoors();
        InstantiateCorridors();
    }
}


