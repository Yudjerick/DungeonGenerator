using Mirror;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NetworkPlayerBootstrap : NetworkBehaviour
{
    [SerializeField] List<GameObject> localPlayerObjects;
    [SerializeField] List<MonoBehaviour> localPlayerScripts;
    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (GameObject obj in localPlayerObjects)
            {
                Destroy(obj);
            }
            foreach (MonoBehaviour obj in localPlayerScripts)
            {
                Destroy(obj);
            }
        }
        
    }
}
