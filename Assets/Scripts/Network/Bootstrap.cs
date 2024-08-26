using DungeonGeneration;
using Mirror;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : NetworkBehaviour
{
    [SerializeField] private SeedInitializer seedInitializer;
    void Start()
    {

    }

    public override void OnStartServer()
    {
        seedInitializer.GenerateSeed();
    }

    public override void OnStartClient()
    {
        print("OnStartClient");
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        seedInitializer.SetRandomInitialState();
        
        FindAnyObjectByType<DungeonGenerator>().Generate();
        FindAnyObjectByType<NavMeshBaker>().Bake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
