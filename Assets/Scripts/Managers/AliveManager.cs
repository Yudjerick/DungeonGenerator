using Mirror;
using UnityEngine;

public class AliveManager : NetworkBehaviour
{
    public static AliveManager instance; 

    public readonly SyncList<GameObject> AlivePlayers = new SyncList<GameObject>();
    public readonly SyncList<GameObject> DeadPlayers = new SyncList<GameObject>();

    [Scene]
    [SerializeField]
    private string deadScene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
        DeadPlayers.OnAdd = OnAddDeadPlayer;
    }

    [Server]
    public void SetPlayerDead(GameObject player)
    {
        AlivePlayers.Remove(player);
        DeadPlayers.Add(player);

        if (AlivePlayers.Count == 0)
        {
            NetworkManager.singleton.ServerChangeScene(deadScene);
            print("All players dead");
        }
    }

    public void Init()
    {
        instance = this;
    }

    private void OnAddDeadPlayer(int idx)
    {
        
    }
}
