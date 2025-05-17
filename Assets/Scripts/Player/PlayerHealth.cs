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
        if (SteamManager.Initialized)
        {
            playerName = SteamFriends.GetPersonaName();
        }
        else
        {
            playerName = "Player" + connectionToClient.connectionId;
        }
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
        //AliveManager.instance.DeadPlayers.Add(newPlayer); 
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, ReplacePlayerOptions.Destroy);
    }

    private void OnDisable()
    {
        DieEvent?.Invoke();
    } 

    
}
