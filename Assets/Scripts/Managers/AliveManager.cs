using Mirror;
using UnityEngine;

public class AliveManager : NetworkBehaviour
{
    public static AliveManager instance; 

    public readonly SyncList<GameObject> AlivePlayers = new SyncList<GameObject>();
    public readonly SyncList<GameObject> DeadPlayers = new SyncList<GameObject>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void Init()
    {
        instance = this;
    }
}
