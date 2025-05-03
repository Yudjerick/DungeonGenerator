using Mirror;
using NaughtyAttributes;
using Steamworks;
using TMPro;
using UnityEngine;

public class SteamLobbyCreateManager : MonoBehaviour
{

    
    [field: SerializeField] public bool IsPublic { get; private set; }
    [field: SerializeField] public string ServerName { get; private set; }


    public static CSteamID LobbyId { get; private set; }

    protected Callback<LobbyCreated_t> lobbyCreated;


    void Start()
    {
        //DontDestroyOnLoad(this);
        if (!SteamManager.Initialized)
        {
            print("SteamManager not init");
            return;
        }
        ServerName = SteamFriends.GetPersonaName() + "'s Server";
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        

    }

    [Button]
    public void HostLobby()
    {
        if(IsPublic)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
        }
        else
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        }
        
    }



    public void SetPublic(bool value)
    {
        IsPublic = value;
    }

    public void SetServerName(string value)
    {
        ServerName = value;
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        LobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        NetworkManager.singleton.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "ServerName", ServerName);
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostKey",
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "GameName",
            "Multiplayer4");
    }
}
