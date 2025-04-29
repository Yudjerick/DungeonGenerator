using Mirror;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class FTGNetworkManager : NetworkRoomManager
{
    [SerializeField] private List<GameObject> playerCharacters;
    [SerializeField] private int characterIndex;
    [SerializeField] private AliveManager aliveManager;
    [SerializeField] private AliveManager aliveManagerRef;

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        print("New client connected");
        
    }

    public override void OnRoomServerPlayersReady()
    {
        aliveManager = Instantiate(aliveManagerRef);
        NetworkServer.Spawn(aliveManager.gameObject);
        base.OnRoomServerPlayersReady();
    }


    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        print("OnRoomServerCreateGamePlayer");
        GameObject player = Instantiate(playerPrefab);
        if (AliveManager.instance == null)
        {
            aliveManager.Init();
        }
        var listBuff = aliveManager.AlivePlayers.Select(x => x).ToList();
        listBuff.Add(player);
        aliveManager.AlivePlayers.Clear();
        aliveManager.AlivePlayers.AddRange(listBuff);

        return player;
    }

    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        
    }
}
