using Mirror;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class FTGNetworkManager : NetworkRoomManager
{
    [SerializeField] private AliveManager aliveManager;
    [SerializeField] private AliveManager aliveManagerRef;

    public override void OnRoomServerPlayersReady()
    {
        aliveManager = Instantiate(aliveManagerRef);
        NetworkServer.Spawn(aliveManager.gameObject);
        base.OnRoomServerPlayersReady();
    }


    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        print("OnRoomServerCreateGamePlayer");
        FTGRoomPlayer fTGRoomPlayer = roomPlayer.GetComponent<FTGRoomPlayer>();
        GameObject player = Instantiate(fTGRoomPlayer.GetPlayerPrefab());
        var listBuff = aliveManager.AlivePlayers.Select(x => x).ToList();
        listBuff.Add(player);
        aliveManager.AlivePlayers.Clear();
        aliveManager.AlivePlayers.AddRange(listBuff);

        return player;
    }
}
