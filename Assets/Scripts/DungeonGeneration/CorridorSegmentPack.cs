using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonGeneration
{
    /// <summary>
    /// ScriptableObject used for mapping lists of directions to segment prefabs
    /// </summary>
    [CreateAssetMenu(menuName = "Dungeon Generation/Corridor Segment Pack", fileName = "Pack")]
    public class CorridorSegmentPack : ScriptableObject
    {
        [Serializable]
        class CorridorSegmentMap
        {
            public List<CorridorDirection> directions;
            public List<CorridorVariant> variants;
        }

        [Serializable]
        class CorridorVariant
        {
            /// <summary>
            /// segment object prefab
            /// </summary>
            public GameObject segmentObject;
            /// <summary>
            /// Determines relative probability of choosing this segment object
            /// </summary>
            public float weight = 1f;
        }


        [SerializeField] private List<CorridorSegmentMap> corridorSegments;

        /// <summary>
        /// Returns random segement object suitable for given directions
        /// </summary>
        /// <param name="keys">directions that returned segment should have</param>
        /// <returns></returns>
        public GameObject GetSegment(List<CorridorDirection> keys)
        {
            var map = corridorSegments.Where(c => keys.All(k => c.directions.Contains(k))).FirstOrDefault();
            float weightSum = map.variants.Sum(k => k.weight);
            float rand = Random.Range(0f, weightSum);
            foreach(var variant in map.variants)
            {
                if(variant.weight >= rand)
                {
                    return variant.segmentObject;
                }
                rand -= variant.weight;
            }
            return null;
        }

        /// <summary>
        /// Converts Vector to CorridorDirection
        /// </summary>
        /// <param name="vector3">Vector to convert</param>
        /// <returns></returns>
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