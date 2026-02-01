using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

namespace DungeonGeneration
{
    public class DungeonGenerator : MonoBehaviour
    {
        
        [field: SerializeField] public List<FloorGenerator> floorGenerators { get; set; } = new List<FloorGenerator>();

        [SerializeField] private int2 clusterZeroXZ;

        [Button]
        public void Generate()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var floorGenerator in floorGenerators)
            {
                floorGenerator.Initialize(this, floorGenerators.IndexOf(floorGenerator), clusterZeroXZ.x, clusterZeroXZ.y);
            }
            foreach (var floorGenerator in floorGenerators)
            {
                floorGenerator.Generate();
            }
            stopWatch.Stop();
            print(stopWatch.Elapsed.Milliseconds);
        }
    }
}