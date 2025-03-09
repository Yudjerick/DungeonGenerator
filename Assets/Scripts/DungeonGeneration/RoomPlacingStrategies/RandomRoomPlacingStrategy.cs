using System;
using Random = UnityEngine.Random;

namespace DungeonGeneration
{
    public class RandomRoomPlacingStrategy : RoomPlacingStrategy
    {
        public override void PlaceAllRooms(int y, FloorGenerator fg)
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
    }
}
