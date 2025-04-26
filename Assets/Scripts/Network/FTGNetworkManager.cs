using Mirror;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class FTGNetworkManager : NetworkManager
{
    [SerializeField] private List<GameObject> playerCharacters;
    [SerializeField] private int characterIndex;
    [SerializeField] private AliveManager aliveManager;

    public override void OnStartServer()
    {
        base.OnStartServer();
        //FindAnyObjectByType<AliveManager>().Init();
        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }



    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        print("New client connected");
        
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        if (AliveManager.instance == null)
        {
            aliveManager.Init();
        }

        CreateCharacterMessage characterMessage = new CreateCharacterMessage
        {
            race = Race.Elvish,
            name = "Joe Gaba Gaba",
            hairColor = Color.red,
            eyeColor = Color.green
        };

        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
    {

        GameObject player = Instantiate(playerCharacters[characterIndex]);

        NetworkServer.AddPlayerForConnection(conn, player);
        if(AliveManager.instance == null)
        {
            aliveManager.Init();
        }
        var listBuff = aliveManager.AlivePlayers.Select(x => x).ToList();
        listBuff.Add(player);
        aliveManager.AlivePlayers.Clear();
        aliveManager.AlivePlayers.AddRange(listBuff);
    }

    public struct CreateCharacterMessage : NetworkMessage
    {
        public Race race;
        public string name;
        public Color hairColor;
        public Color eyeColor;
    }

    public enum Race
    {
        None,
        Elvish,
        Dwarvish,
        Human
    }
}
