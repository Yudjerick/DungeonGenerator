
using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyRef;
    public static SpawnManager instance;

    public List<Transform> enemyPositions = new List<Transform>();

    [Server]
    public void SpawnEnemies()
    {
        foreach (Transform t in enemyPositions) 
        {
            var enemy = Instantiate(enemyRef, t.position, t.rotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
