using Assets.Scripts.Items;
using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LateJoinManager : NetworkBehaviour
{
    public LateJoinManager Instance { get; private set; }

    public List<GameObject> pickedUpItems;

    public void SyncPickupsWithClient(NetworkConnectionToClient connectionToClient)
    {
        RpcSyncPickups(connectionToClient, pickedUpItems.ToArray());
    }

    [TargetRpc]
    public void RpcSyncPickups(NetworkConnectionToClient target, GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            item.GetComponent<InventoryItem>().PlayerSyncHook(null, item.GetComponent<InventoryItem>().Player);
        }
    }
}
