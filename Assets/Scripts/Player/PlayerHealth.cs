using Mirror;
using NaughtyAttributes;
using Steamworks;
using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private GameObject deadPlayer;
    public Action DieEvent;

    [SyncVar(hook = nameof(NameChangedHook))] public string playerName;
    [SerializeField]
    private TMP_Text playerNameText;

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerName = ((FTGNetworkManager)NetworkManager.singleton).playerNames[connectionToClient];
    }

    private void NameChangedHook(string oldName, string newName)
    {
        playerNameText.text = newName;
    }

    

    [Command]
    private void CmdDie()
    {
        DieServer();
    }

    public override void DieServer()
    {
        NetworkConnectionToClient connection = connectionToClient;
        print(AliveManager.instance);
        AliveManager.instance.SetPlayerDead(gameObject);
        GameObject newPlayer = Instantiate(deadPlayer, transform.position, transform.rotation);
        GetComponent<Inventory>().DropAllItems();
        RpcDieCLient();
        //AliveManager.instance.DeadPlayers.Add(newPlayer);
        
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, ReplacePlayerOptions.KeepActive);
    }

    [ClientRpc]
    private void RpcDieCLient()
    {
        GetComponentInChildren<RagdollManager>().EnableRagdollMode();
        if (isLocalPlayer)
        {
            GetComponent<PlayerInput>().enabled = false;
        }
        DieEvent?.Invoke();
    } 
}
