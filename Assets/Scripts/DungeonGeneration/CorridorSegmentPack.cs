using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGeneration
{
    [CreateAssetMenu(menuName = "Dungeon Generation/Corridor Segment Pack", fileName = "Pack")]
    public class CorridorSegmentPack : ScriptableObject
    {
        [Serializable]
        class CorridorSegment
        {
            public List<CorridorDirection> directions;
            public GameObject segmentObject;
        }

        [SerializeField] private List<CorridorSegment> corridorSegments;

        public GameObject GetSegment(List<CorridorDirection> keys)
        {
            return corridorSegments.Where(c => keys.All(k => c.directions.Contains(k))).FirstOrDefault().segmentObject;
        }

        public static CorridorDirection Vector3ToCorridorDirection(Vector3 vector3)
        {
            float tolerance = 0.01f;
            if (Vector3.Distance(Vector3.forward, vector3) <= tolerance)
            {
                return CorridorDirection.Forward;
            }
            if (Vector3.Distance(Vector3.right, vector3) <= tolerance)
            {
                return CorridorDirection.Right;
            }
            if (Vector3.Distance(Vector3.back, vector3) <= tolerance)
            {
                return CorridorDirection.Back;
            }
            if (Vector3.Distance(Vector3.left, vector3) <= tolerance)
            {
                return CorridorDirection.Left;
            }
            return CorridorDirection.Back;
        }
    }
}