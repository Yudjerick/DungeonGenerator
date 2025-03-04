using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGeneration
{
    public class RoomPlacingComponent
    {
        public bool CanPlaceRoom(int x, int z, RoomData room, int rotationsCount, FloorGenerator floorGenerator)
        {
            int width = room.Width;
            int height = room.Height;
            int depth = room.Depth;
            if (rotationsCount % 2 != 0)
            {
                depth = room.Width;
                width = room.Depth;
            }
            if (x + width + floorGenerator.Gap >= floorGenerator.DungeonWidth || z + depth + floorGenerator.Gap >= floorGenerator.DungeonWidth)
            {
                return false;
            }
            if (x - floorGenerator.Gap < 0 || z - floorGenerator.Gap < 0)
            {
                return false;
            }
            for (int i = -floorGenerator.Gap; i < width + floorGenerator.Gap; i++)
            {
                for (int k = -floorGenerator.Gap; k < depth + floorGenerator.Gap; k++)
                {
                    if (floorGenerator.SolidMap[x + i, z + k])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public RoomData PlaceRoom(int x, int z, RoomData roomRef, int rotationsCount, FloorGenerator fg)
        {
            int width = roomRef.Width;
            int height = roomRef.Height;
            int depth = roomRef.Depth;
            int xOffset = 0;
            int zOffset = 0;
            if (!roomRef.RandomRotation) // move somewhere else later
            {
                rotationsCount = 0;
            }
            if (rotationsCount % 2 != 0)
            {
                depth = roomRef.Width;
                width = roomRef.Depth;
            }
            if (rotationsCount == 2 || rotationsCount == 3)
            {
                xOffset = width - 1;
            }
            if (rotationsCount == 1 || rotationsCount == 2)
            {
                zOffset = depth - 1;
            }
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < depth; k++)
                {

                    fg.SolidMap[x + i, z + k] = true;
                }
            }
            var roomInstance = UnityEngine.Object.Instantiate(roomRef, new Vector3((fg.ClusterZeroX + x + xOffset) * fg.GridCellSize,
                fg.LevelY, (fg.ClusterZeroZ + z + zOffset) * fg.GridCellSize), Quaternion.Euler(0, 90 * rotationsCount, 0), fg.Floor.transform);
            roomInstance.Initialize(fg);
            
            fg.Rooms.Add(roomInstance);
            return roomInstance;
        }

        public void PlaceAllRooms(int y, FloorGenerator fg)
        {
            foreach (RoomData room in fg.RoomRefs)
            {
                int x = Random.Range(0, fg.DungeonWidth);
                int z = Random.Range(0, fg.DungeonDepth);
                int rotationsCount = Random.Range(0, 3);
                int tryCount = 0;
                bool roomSkiped = false;
                while (!CanPlaceRoom(x, z, room, rotationsCount, fg))
                {
                    if (tryCount > 10000)
                    {
                        //print(x + " " + y + " " + z);
                        //print("Skipping room");
                        roomSkiped = true;
                        break;
                    }
                    tryCount++;
                    x = Random.Range(0, fg.DungeonWidth);
                    z = Random.Range(0, fg.DungeonDepth);
                }
                if (!roomSkiped)
                {
                    PlaceRoom(x, z, room, rotationsCount, fg);
                    foreach (var extraRoom in room.AssociatedRooms)
                    {
                        PlaceRoom(x, z, extraRoom.ExtraRoom, rotationsCount, fg.DungeonGenerator.floorGenerators[fg.FloorIndex + extraRoom.FloorOffset]);
                    }
                }

            }
        }

        public void PlaceAllRoomsPush(int y, FloorGenerator fg)
        {
            foreach (RoomData room in fg.RoomRefs)
            {
                Vector3 randomDirection = Random.insideUnitCircle.normalized;
                int centerX = fg.DungeonWidth / 2;
                int centerZ = fg.DungeonDepth / 2;
                int x = centerX;
                int z = centerZ;
                int rotationsCount = Random.Range(0, 3);
                int tryCount = 1;
                bool roomSkiped = false;
                while (!CanPlaceRoom(x, z, room, rotationsCount, fg))
                {
                    if (tryCount > 10000)
                    {
                        roomSkiped = true;
                        break;
                    }
                    tryCount++;
                    x = centerX + (int)(randomDirection.x * tryCount);
                    z = centerZ + (int)(randomDirection.y * tryCount);
                }
                if (!roomSkiped)
                {
                    PlaceRoom(x, z, room, rotationsCount, fg);
                    foreach (var extraRoom in room.AssociatedRooms)
                    {
                        PlaceRoom(x, z, extraRoom.ExtraRoom, rotationsCount, fg.DungeonGenerator.floorGenerators[fg.FloorIndex + extraRoom.FloorOffset]);
                    }
                }

            }
        }
    }
}