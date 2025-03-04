using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGeneration
{
    public class RoomCluster: RoomData
    {
        [SerializeField] private FloorGenerator clusterGenerator;
        public override void Initialize(FloorGenerator parentGenerator)
        {
            base.Initialize(parentGenerator);
            RandomRotation = false;
            clusterGenerator.DungeonWidth = Width;
            clusterGenerator.DungeonHeight = Height;
            clusterGenerator.DungeonDepth = Depth;
            clusterGenerator.Initialize(parentGenerator.DungeonGenerator, parentGenerator.FloorIndex, (int)Math.Round(transform.position.x), (int)Math.Round(transform.position.z));
            clusterGenerator.LevelY = parentGenerator.LevelY;

            foreach(var door in AvailableDoors)
            {
                var roomObj = new GameObject("ExposedDoorRoom");
                roomObj.transform.parent = transform;
                roomObj.transform.rotation = door.Transform.rotation * Quaternion.Euler(0,180,0);
                roomObj.transform.position = door.Transform.position + door.Transform.forward * clusterGenerator.GridCellSize;
                roomObj.AddComponent<RoomData>();
                
                RoomData exposedDoorRoomData = roomObj.GetComponent<RoomData>();
                DoorData exposedDoorData = new DoorData();
                exposedDoorData.Transform = roomObj.transform;
                exposedDoorRoomData.AvailableDoors = new List<DoorData> { exposedDoorData };
                clusterGenerator.Rooms.Add(exposedDoorRoomData);
            }
            clusterGenerator.Generate();

        }
    }
}
