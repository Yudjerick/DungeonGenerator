using NaughtyAttributes;
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
        [Foldout("Initial Room")]
        [SerializeField] private bool hasInitialRoom = false;
        [Foldout("Initial Room")]
        [SerializeField] private RoomData initialRoom;
        [Foldout("Initial Room")]
        [SerializeField] private int initialRoomX;
        [Foldout("Initial Room")]
        [SerializeField] private int initialRoomZ;
        [Foldout("Initial Room")]
        [SerializeField] private int initialRoomRotationCount;

        private List<CorridorDirection>[,] _corridorMap;
        private DungeonGenerator _dungeonGenerator;
        private int _floorIndex;

        public bool[,] SolidMap { get; set; }
        [field: SerializeField] public List<RoomData> Rooms { get; set; } = new List<RoomData>();
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

        [field: SerializeField]public int ClusterZeroX { get; set; }
        [field: SerializeField] public int ClusterZeroZ { get; set; }

        public void Initialize(DungeonGenerator dungeonGenerator, int floorIndex, int clusterZeroX, int clusterZeroZ)
        {
            ClusterZeroX = clusterZeroX;
            ClusterZeroZ = clusterZeroZ;
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

        public void MarkDoorTypes()
        {
            foreach (var room in Rooms)
            {
                foreach (var transition in room.Transitions)
                {
                    transition.Door.doorType = transition.NextRoom.DoorType;
                }
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
                        Vector3 start = transition.Door.Transform.position / GridCellSize
                            - new Vector3(ClusterZeroX, 0, ClusterZeroZ);
                        start.y = 0;
                        Transition endTransition = transition.NextRoom.Transitions.Where(t => t.NextRoom == room).FirstOrDefault();
                        Vector3 end = endTransition.Door.Transform.position / GridCellSize
                            - new Vector3(ClusterZeroX, 0, ClusterZeroZ);
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

        public GameObject InstantiateCorridors()
        {
            var corridor = new GameObject("Corridor");
            corridor.transform.parent = Floor.transform;
            for (int i = 0; i < DungeonWidth; i++)
            {
                for (int k = 0; k < DungeonDepth; k++)
                {
                    if (_corridorMap[i, k] != null)
                    {
                        Instantiate(segmentPack.GetSegment(_corridorMap[i, k]), new Vector3((i + ClusterZeroX) * gridCellSize,
                            LevelY, (k + ClusterZeroZ) * gridCellSize), Quaternion.identity, corridor.transform);

                    }
                }

            }
            return corridor;
        }

        public void AddCorridorsToUnusedDoors()
        {
            foreach (RoomData room in Rooms)
            {
                List<DoorData> unusedDoors = room.AvailableDoors.Where(d => !room.Transitions.Select(x => x.Door).Contains(d)).ToList();
                foreach (DoorData unusedDoor in unusedDoors)
                {
                    int x = (int)Mathf.Round(unusedDoor.Transform.position.x / gridCellSize) - ClusterZeroX;
                    int z = (int)Mathf.Round(unusedDoor.Transform.position.z / gridCellSize) - ClusterZeroZ;
                    if (_corridorMap[x, z] == null)
                    {
                        _corridorMap[x, z] = new List<CorridorDirection>();
                    }
                    _corridorMap[x, z].Add(CorridorSegmentPack.Vector3ToCorridorDirection(unusedDoor.Transform.forward));
                }
            }
        }

        public void SpawnInitialRoom()
        {
            RoomPlacingStrategy roomPlacing = new RandomRoomPlacingStrategy();
            roomPlacing.PlaceRoom(initialRoomX, initialRoomZ, initialRoom, initialRoomRotationCount, this);
        }

        public void Generate()
        {

            //int seed = Random.Range(0, int.MaxValue);
            //Random.InitState(seed);

            if(hasInitialRoom)
            {
                SpawnInitialRoom();
            }

            RoomPlacingStrategy roomPlacing = new PushRoomPlacingStrategy();
            roomPlacing.PlaceAllRooms(levelY, this);
            SetMinimalTransitions();
            AddLoops(loopCount);
            MarkDoorTypes();
            MakePathes();
            AddCorridorsToUnusedDoors();

            var corridor = InstantiateCorridors();

            /*corridor.AddComponent<MeshFilter>();
            MeshFilter[] meshFilters = corridor.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine);
            corridor.GetComponent<MeshFilter>().sharedMesh = mesh;
            //transform.gameObject.SetActive(true);*/
        }
    }
}