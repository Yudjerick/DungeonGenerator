using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Generation/Corridor Segment Pack", fileName = "Pack")]
public class CorridorSegmentPack : ScriptableObject
{
    [Serializable]
    class CorridorSegment
    {
        public List<Vector3> keys;
        public GameObject segmentObject;
    }

    [SerializeField] private List<CorridorSegment> corridorSegments; 

    public GameObject GetSegment(List<Vector3> keys)
    {
        //return corridorSegments.Where(c => keys.All(k => c.keys.Contains(k))).FirstOrDefault().segmentObject;
        return corridorSegments.Where(c => c.keys.Contains(keys[0]) && c.keys.Contains(keys[1])).FirstOrDefault().segmentObject;
    }
}