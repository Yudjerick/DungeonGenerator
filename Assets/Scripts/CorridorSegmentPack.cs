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
        foreach (var segment in corridorSegments)
        {
            bool flag = true;
            foreach (var key in keys)
            {
                bool hasKey = false;
                foreach (var segmentKey in segment.keys)
                {
                    if (Vector3.Distance(key,segmentKey) < 0.01f)
                    {
                        hasKey = true;
                    }
                }
                if (!hasKey)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                return segment.segmentObject;
            }

        }
        return null;
        //return corridorSegments.Where(c => keys.All(k => c.keys.Contains(k))).FirstOrDefault().segmentObject; 
    }
}