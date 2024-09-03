using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGeneration
{
    /// <summary>
    /// Generates one dungeon floor
    /// </summary>
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

        public void SetMinimalTransitions()
        {
            var groups = Rooms.GroupBy(x => x.TransitionGroupId);
            foreach (var group in groups)
            {
                var tm = new TransitionMakingComponent(group.ToList());
                tm.SetMinimalTransitions();
            }
        }

        public void AddLoops(int loopCount)
        {
            var groups = Rooms.GroupBy(x => x.TransitionGroupId);
            foreach (var group in groups)
            {
                var tm = new TransitionMakingComponent(group.ToList());
                tm.AddLoops(loopCount, minimalLoopLength);
            }
        }

        public void MakePathes()
        {
            foreach (var room in Rooms)
            {
                foreach (var transition in room.Transitions)
                {
                    if (!transition.PathBuilt)
                    {
                        Vector3 start = transition.Door.Transform.position / GridCellSize;
                        start.y = 0;
                        Transition endTransition = transition.NextRoom.Transitions.Where(t => t.NextRoom == room).FirstOrDefault();
                        Vector3 end = endTransition.Door.Transform.position / GridCellSize;
                        end.y = 0;
                        Pathfinder pathfinder = new Pathfinder(SolidMap, (int)Mathf.Round(start.x), (int)Mathf.Round(start.z), (int)Mathf.Round(end.x), (int)Mathf.Round(end.z));

                        List<Vector3> path = pathfinder.FindPath();



                        if (path != null)
                        {
                            path.Add(start + transition.Door.Transform.forward);
                            path.Insert(0, end + endTransition.Door.Transform.forward);

                            for (int i = 1; i < path.Count - 1; i++)
                            {
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
                        else
                        {
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
            foreach (RoomData room in Rooms)
            {
                List<DoorData> unusedDoors = room.AvailableDoors.Where(d => !room.Transitions.Select(x => x.Door).Contains(d)).ToList();
                foreach (DoorData unusedDoor in unusedDoors)
                {
                    int x = (int)Mathf.Round(unusedDoor.Transform.position.x / gridCellSize);
                    int z = (int)Mathf.Round(unusedDoor.Transform.position.z / gridCellSize);
                    if (_corridorMap[x, z] == null)
                    {
                        _corridorMap[x, z] = new List<CorridorDirection>();
                    }
                    _corridorMap[x, z].Add(CorridorSegmentPack.Vector3ToCorridorDirection(unusedDoor.Transform.forward));
                }
            }
        }

        public void Generate()
        {

            //int seed = Random.Range(0, int.MaxValue);
            //Random.InitState(seed);
            RoomPlacingComponent roomPlacing = new RoomPlacingComponent();
            roomPlacing.PlaceAllRoomsPush(levelY, this);
            SetMinimalTransitions();
            AddLoops(loopCount);
            MakePathes();
            AddCorridorsToUnusedDoors();
            InstantiateCorridors();
        }
    }
}