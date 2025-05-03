using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnObj;
    [SerializeField] private float delay = 3f;

    public override void OnStartServer()
    {
        Invoke("Spawn", delay);
    }

    public void Spawn()
    {
        if(!isServer)
        {
            return;
        }
        var instance = Instantiate(spawnObj);
        NetworkServer.Spawn(instance);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
