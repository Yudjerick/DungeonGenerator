
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DungeonGeneration.RoomData;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyRef;
    public static SpawnManager instance;

    public List<SpawnPoint> enemyPositions = new List<SpawnPoint>();

    [Server]
    public void SpawnEnemies()
    {
        foreach (SpawnPoint t in enemyPositions) 
        {
            var enemy = Instantiate(t.pool.GetItem(), t.transform.position, t.transform.rotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
