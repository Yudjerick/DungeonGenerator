using Mirror;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FTGNetworkManager : NetworkManager
{
    [SerializeField] private List<GameObject> playerCharacters;
    [SerializeField] private int characterIndex;

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // you can send the message here, or wherever else you want
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
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(playerCharacters[characterIndex]);

        // Apply data from the message however appropriate for your game
        // Typically Player would be a component you write with syncvars or properties


        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);
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
