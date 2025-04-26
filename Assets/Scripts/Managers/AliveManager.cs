using Mirror;
using UnityEngine;

public class AliveManager : NetworkBehaviour
{
    public static AliveManager instance; 

    [SerializeField] private SyncList<GameObject> alivePlayers;
    [SerializeField] private SyncList<GameObject> deadPlayers;

    public SyncList<GameObject> AlivePlayers { get => alivePlayers; set => alivePlayers = value; }
    public SyncList<GameObject> DeadPlayers { get => deadPlayers; set => deadPlayers = value; }



    public void Init()
    {
        instance = this;
        alivePlayers = new SyncList<GameObject>();
        deadPlayers = new SyncList<GameObject>();
    }
}
