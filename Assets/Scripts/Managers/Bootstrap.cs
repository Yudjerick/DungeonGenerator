using DungeonGeneration;
using Mirror;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : NetworkBehaviour
{
    [SerializeField] private SeedInitializer seedInitializer;
    [SerializeField] private SpawnManager spawnManager;
    void Start()
    {
        
    }

    public override void OnStartServer()
    {
        seedInitializer.GenerateSeed();
    }

    public override void OnStartClient()
    {
        SpawnManager.instance = spawnManager;
        print("OnStartClient");
        GenerateDungeon();
        spawnManager.SpawnEnemies();
    }

    public void GenerateDungeon()
    {
        if(seedInitializer == null)
        {
            return;
        }
        seedInitializer.SetRandomInitialState();
        
        FindAnyObjectByType<DungeonGenerator>().Generate();
        FindAnyObjectByType<NavMeshBaker>().Bake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
