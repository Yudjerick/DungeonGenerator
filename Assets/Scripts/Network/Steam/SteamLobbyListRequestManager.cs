using NaughtyAttributes;
using Steamworks;
using UnityEngine;

public class SteamLobbyListRequestManager : MonoBehaviour
{
    [SerializeField] private Transform lobbyListContentParent;
    [SerializeField] private LobbyListItem lobbyListItemPrefab;

    protected Callback<LobbyMatchList_t> lobbyMatchListed;

    void Start()
    {
        lobbyMatchListed = Callback<LobbyMatchList_t>.Create(OnLobbyMatchListed);
    }

    private void OnLobbyMatchListed(LobbyMatchList_t callback)
    {
        print("Lobby Match List");
        for (int i = 0; i < lobbyListContentParent.childCount; i++)
        {
            Destroy(lobbyListContentParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < callback.m_nLobbiesMatching; i++)
        {
            var lobbyId = SteamMatchmaking.GetLobbyByIndex(i);

            string hostAddress = SteamMatchmaking.GetLobbyData(lobbyId, "HostKey");
            string serverName = SteamMatchmaking.GetLobbyData(lobbyId, "ServerName");
            int playerCount = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
            var item = Instantiate(lobbyListItemPrefab, lobbyListContentParent);
            item.Init( serverName, SteamFriends.GetFriendPersonaName(new CSteamID(ulong.Parse(hostAddress))),
                playerCount, lobbyId);

        }
    }
    [Button]
    public void GetLobbyList()
    {
        if (!SteamManager.Initialized)
        {
            print("SteamManager not init");
            return;
        }
        SteamMatchmaking.AddRequestLobbyListStringFilter("GameName",
            "Multiplayer4", ELobbyComparison.k_ELobbyComparisonEqual);

        SteamMatchmaking.RequestLobbyList();
    }
}
