using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnObj;
    void Start()
    {
        var instance = Instantiate(spawnObj);
        NetworkServer.Spawn(instance);
    }

    public override void OnStartServer()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
