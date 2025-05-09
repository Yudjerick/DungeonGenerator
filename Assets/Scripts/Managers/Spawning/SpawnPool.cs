using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPool", menuName = "Dungeon Generation/SpawnPool")]
public class SpawnPool : ScriptableObject
{
    public List<SpawnPoolItem> items = new List<SpawnPoolItem>();   

    [Serializable]
    public struct SpawnPoolItem
    {
        public GameObject prefab;
        public float weight; 
    }

    public GameObject GetItem()
    {
        float weightSum = items.Sum(x => x.weight);
        float rand = UnityEngine.Random.Range(0f, weightSum);
        foreach (var item in items)
        {
            if (item.weight >= rand)
            {
                return item.prefab;
            }
            rand -= item.weight;
        }
        return null;
    }
}
