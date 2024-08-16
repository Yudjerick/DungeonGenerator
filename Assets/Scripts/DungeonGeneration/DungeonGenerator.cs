using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [field: SerializeField] public List<FloorGenerator> floorGenerators { get; set; } = new List<FloorGenerator>();
    [Button]
    public void Generate()
    {
        foreach (var floorGenerator in floorGenerators)
        {
            floorGenerator.Initialize(this, floorGenerators.IndexOf(floorGenerator));
        }
        foreach (var floorGenerator in floorGenerators)
        {
            floorGenerator.Generate();
        }
    }
}
