using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeedInitializer : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] private int seed;
    void Start()
    {
        //if (isServer)
        //{
        //    seed = Random.Range(0, int.MaxValue);
        //}





    }

    public override void OnStartServer()
    {
        print("OnStartServer");
        seed = Random.Range(0, int.MaxValue);
    }

    public override void OnStartClient()
    {
        print("OnStartClient");
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        print("RPC");
        Random.InitState(seed);
        FindAnyObjectByType<DungeonGenerator>().Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
