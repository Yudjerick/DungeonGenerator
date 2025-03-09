using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace DungeonGeneration
{
    public class PushRoomPlacingStrategy : RoomPlacingStrategy
    {
        public override void PlaceAllRooms(int y, FloorGenerator fg)
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
