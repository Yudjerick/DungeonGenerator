using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class NetworkPlayerBootstrap : NetworkBehaviour
{
    [SerializeField] List<GameObject> localPlayerObjects;
    [SerializeField] List<MonoBehaviour> localPlayerScripts;
    [SerializeField] List<Component> localPlayerComponents;
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
            foreach(Component obj in localPlayerComponents)
            {
                Destroy(obj);
            }
        }
        Destroy(this);
    }
}
