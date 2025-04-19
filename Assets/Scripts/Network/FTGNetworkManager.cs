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
        print("New client connected");
        /*foreach (var inventorySync in FindObjectsOfType<NetworkInventorySync>())
        {
            inventorySync.OnInventoryUpdated();
            inventorySync.OnSlotIndexUpdated();
        }*/
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

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

        GameObject gameobject = Instantiate(playerCharacters[characterIndex]);

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
