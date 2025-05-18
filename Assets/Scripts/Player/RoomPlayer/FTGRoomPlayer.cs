using Mirror;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FTGRoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private List<GameObject> playerPrefabs;

    [SyncVar(hook = nameof(SelecetedPlayerIdxHook))] public int selectedPlayerIdx;

    [SyncVar] public string playerName;

    public Action UpdatedEvent;

    public List<GameObject> PlayerPrefabs { get => playerPrefabs; set => playerPrefabs = value; }
    public int SelectedPlayerIdx { get => selectedPlayerIdx; set => selectedPlayerIdx = value; }


    public override void OnStartClient()
    {
        base.OnStartClient();
        transform.GetChild(0).SetParent(GameObject.Find("ParentPanel").transform);

        if (SteamManager.Initialized)
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
        }
        else
        {
            CmdSetPlayerNameWithConnectionId("Player_");
        }
    }

    [Command]
    public void CmdSetPlayerName(string name)
    {
        playerName = name;
        ((FTGNetworkManager)NetworkManager.singleton).playerNames[connectionToClient] = playerName;
        UpdatedEvent?.Invoke();
    }

    [Command]
    public void CmdSetPlayerNameWithConnectionId(string name)
    {
        var nameBuff = name;
        nameBuff += connectionToClient.ToString();
        playerName = nameBuff;
        ((FTGNetworkManager)NetworkManager.singleton).playerNames[connectionToClient] = playerName;
        UpdatedEvent?.Invoke();
    }

    public override void OnStartServer()
    {
        
    }

    public void ToggleReady()
    {

        CmdChangeReadyState(!readyToBegin);
    }


    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        UpdatedEvent?.Invoke();
        base.ReadyStateChanged(oldReadyState, newReadyState);
    }

    public void SwitchPlayer(bool inverseDirection)
    {
        CmdSwitchPlayer(inverseDirection);
    }

    [Command]
    private void CmdSwitchPlayer(bool inverseDirection)
    {
        var buff = selectedPlayerIdx;
        var modification = inverseDirection ? 1 : -1;
        buff += modification;
        if (buff == playerPrefabs.Count)
        {
            buff = 0;
        }
        else if (buff == -1)
        {
            buff = playerPrefabs.Count - 1;
        }
        selectedPlayerIdx = buff;
    }

    private void SelecetedPlayerIdxHook(int oldVal, int newVal)
    {
        UpdatedEvent?.Invoke();
    }

    public GameObject GetPlayerPrefab()
    {
        return playerPrefabs[selectedPlayerIdx];
    }
   
}
