
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TransitionMakingComponent
{
    private List<RoomData> Rooms;

    public TransitionMakingComponent(List<RoomData> rooms)
    {
        Rooms = rooms;
    }

    public void SetMinimalTransitions()
    {
        List<RoomData> unexplored = new List<RoomData>(Rooms);
        List<RoomData> explored = new List<RoomData>();

        RoomData firstRoom = unexplored[0];
        explored.Add(firstRoom);
        unexplored.Remove(firstRoom);

        while (unexplored.Count > 0)
        {
            RoomData selectedFrom = explored[0];
            RoomData selectedTo = unexplored[0];
            Transform selectedFromDoor = null;
            Transform selectedToDoor = null;
            float minDistance = float.MaxValue;
            foreach (var from in explored)
            {
                foreach (var to in unexplored)
                {
                    float distance = from.OptimalDoorDistance(to, out Transform doorPos, out Transform otherDoorPos);
                    if (distance < minDistance)
                    {
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
            Debug.DrawLine(selectedFromDoor.position, selectedToDoor.position, Color.green, 100000f); //Null reference here
        }
    }

    private List<float> HopsToRooms(int startRoomIndex)
    {
        List<RoomData> unexplored = new List<RoomData>(Rooms);
        List<RoomData> explored = new List<RoomData>();
        List<float> distances = Rooms.Select(x => float.PositiveInfinity).ToList();
        distances[startRoomIndex] = 0;
        int currentRoomIndex = startRoomIndex;
        while (explored.Count < Rooms.Count)
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
                if (unexploredRoom.Transitions.Select(x => x.NextRoom).Where(y => explored.Contains(y)).Any())
                {
                    currentRoomIndex = Rooms.IndexOf(unexploredRoom);
                    break;
                }
            }
        }
        return distances;
    }

    public void AddLoops(int loopCount, int minimalLoopLength)
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
}
