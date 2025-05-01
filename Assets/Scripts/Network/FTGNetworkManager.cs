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
        FTGRoomPlayer fTGRoomPlayer = roomPlayer.GetComponent<FTGRoomPlayer>();
        var startPos = GetStartPosition();
        GameObject player = Instantiate(fTGRoomPlayer.GetPlayerPrefab(), startPos.position, startPos.rotation);
        var listBuff = aliveManager.AlivePlayers.Select(x => x).ToList();
        listBuff.Add(player);
        aliveManager.AlivePlayers.Clear();
        aliveManager.AlivePlayers.AddRange(listBuff);

        return player;
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        base.OnRoomServerSceneChanged(sceneName);
        if (Utils.IsSceneActive(RoomScene))
        {
            return;
        }
        aliveManager.AlivePlayers.Clear();
        aliveManager.DeadPlayers.Clear();
    }

    //Particullarly from base NetworkRoomManager
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        clientIndex++;

        if (Utils.IsSceneActive(RoomScene))
        {
            allPlayersReady = false;
            GameObject newRoomGameObject = OnRoomServerCreateRoomPlayer(conn);
            if (newRoomGameObject == null)
                newRoomGameObject = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

            NetworkServer.AddPlayerForConnection(conn, newRoomGameObject);
        }
        else
        {
            OnRoomServerAddPlayer(conn);
        }
    }

    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        var startPos = GetStartPosition();
        GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        
        NetworkServer.AddPlayerForConnection(conn, player);
        var listBuff = aliveManager.AlivePlayers.Select(x => x).ToList();
        listBuff.Add(player);
        aliveManager.AlivePlayers.Clear();
        aliveManager.AlivePlayers.AddRange(listBuff);
    }
}
