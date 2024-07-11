using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Selectable;
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

    private List<RoomData> _rooms = new List<RoomData>();
    private bool[,] _solidMap;
    private List<CorridorDirection>[,] _corridorMap;
    private GameObject _floor;
    private DungeonGenerator _dungeonGenerator;
    private int _floorIndex;
    
    public void Initialize(DungeonGenerator dungeonGenerator, int floorIndex)
    {
        _floorIndex = floorIndex;
        _dungeonGenerator = dungeonGenerator;
        _floor = new GameObject("Floor");
        _solidMap = new bool[dungeonWidth, dungeonDepth];
        _corridorMap = new List<CorridorDirection>[dungeonWidth, dungeonDepth];
    }
    public bool CanPlaceRoom(int x, int z, RoomData room, int rotationsCount){
        int width = room.Width;
        int height = room.Height;
        int depth = room.Depth;
        if(rotationsCount % 2 != 0){
            depth = room.Width;
            width = room.Depth;
        }
        if(x+width+gap >= dungeonWidth || z+depth+gap >= dungeonDepth)
        {
            return false;
        }
        if(x-gap < 0 || z-gap < 0){
            return false;
        }
        for (int i = -gap; i < width + gap; i++){
            for (int k = -gap; k < depth + gap; k++)
            {
                if (_solidMap[x + i, z + k])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public RoomData PlaceRoom(int x, int z, RoomData room, int rotationsCount){
        int width = room.Width;
        int height = room.Height;
        int depth = room.Depth;
        int xOffset = 0;
        int zOffset = 0;
        if(rotationsCount % 2 != 0){
            depth = room.Width;
            width = room.Depth;
        }
        if(rotationsCount == 2 || rotationsCount == 3){
            xOffset = width - 1;
        }
        if(rotationsCount == 1 || rotationsCount == 2){
            zOffset = depth - 1;
        }
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                for(int k = 0; k < depth; k++){
                    
                    _solidMap[x + i, z + k] = true;
                }
            }
        }
        var RoomDimensions = Instantiate(room, new Vector3(x + xOffset, levelY,  z + zOffset) * gridCellSize,
            Quaternion.Euler(0,90 * rotationsCount,0), _floor.transform);
        _rooms.Add(RoomDimensions);
        return RoomDimensions;
    }

    

    public void SetMinimalTransitions(){
        List<RoomData> unexplored = new List<RoomData>(_rooms);
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
        List<RoomData> unexplored = new List<RoomData>(_rooms);
        List<RoomData> explored = new List<RoomData>();
        List<float> distances = _rooms.Select(x => float.PositiveInfinity).ToList();
        distances[startRoomIndex] = 0;
        int currentRoomIndex = startRoomIndex;
        while(explored.Count < _rooms.Count) 
        {
            explored.Add(_rooms[currentRoomIndex]);
            unexplored.Remove(_rooms[currentRoomIndex]);
            foreach (var transition in _rooms[currentRoomIndex].Transitions) 
            {
                int nextRoomIndex = _rooms.IndexOf(transition.NextRoom);
                distances[nextRoomIndex] = Mathf.Min(distances[currentRoomIndex] + 1, distances[nextRoomIndex]);
            }
            foreach (var unexploredRoom in unexplored)
            {
                if(unexploredRoom.Transitions.Select(x => x.NextRoom).Where(y => explored.Contains(y)).Any())
                {
                    currentRoomIndex = _rooms.IndexOf(unexploredRoom);
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

            RoomData fromRoom = _rooms.OrderBy(x => x.Transitions.Count).ToList()[loopsCreated];
            int fromRoomIndex = _rooms.IndexOf(fromRoom);

            var hops = HopsToRooms(fromRoomIndex);

            foreach (var room in _rooms)
            {
                int toRoomIndex = _rooms.IndexOf(room);
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
        foreach (var room in _rooms){
            foreach(var transition in room.Transitions){
                if(!transition.PathBuilt){
                    Vector3 start = transition.Door.position;
                    Transition endTransition = transition.NextRoom.Transitions.Where(t => t.NextRoom == room).FirstOrDefault();
                    Vector3 end = endTransition.Door.position;
                    Pathfinder pathfinder = new Pathfinder(_solidMap, (int)Mathf.Round(start.x), (int)Mathf.Round(start.z), (int)Mathf.Round(end.x), (int)Mathf.Round(end.z));
                    
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
        corridor.transform.parent = _floor.transform;
        for (int i = 0; i < dungeonWidth; i++)
        {
            for (int k = 0; k < dungeonDepth; k++)
            {
                if (_corridorMap[i, k] != null)
                {
                    Instantiate(segmentPack.GetSegment(_corridorMap[i, k]), new Vector3(i, levelY, k), Quaternion.identity, corridor.transform);

                }
            }

        }
    }

    public void PlaceAllRooms(int y)
    {
        foreach (RoomData room in roomRefs)
        {
            int x = Random.Range(0, dungeonWidth);
            int z = Random.Range(0, dungeonDepth);
            int rotationsCount = Random.Range(0,3);
            int tryCount = 0;
            bool roomSkiped = false;
            while (!CanPlaceRoom(x, z, room, rotationsCount))
            {
                if (tryCount > 10000)
                {
                    print(x + " " + y + " " + z);
                    print("Skipping room");
                    roomSkiped = true;
                    break;
                }
                tryCount++;
                x = Random.Range(0, dungeonWidth);
                z = Random.Range(0, dungeonDepth);
            }
            if (!roomSkiped)
            {
                PlaceRoom(x, z, room, rotationsCount);
                foreach(var extraRoom in room.AssociatedRooms)
                {
                    _dungeonGenerator.floorGenerators[_floorIndex + extraRoom.FloorOffset].PlaceRoom(x, z, extraRoom.ExtraRoom, rotationsCount); 
                }
            }

        }
    }

    public void Generate(){

        int seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);
        PlaceAllRooms(levelY);
        //PlaceAllRooms(20);
        //PlaceAllRooms(30);
        SetMinimalTransitions();
        AddLoops(loopCount);
        MakePathes();
        InstantiateCorridors();
    }
}


