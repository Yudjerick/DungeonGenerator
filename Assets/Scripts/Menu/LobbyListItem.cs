using Edgegap;
using Steamworks;
using TMPro;
using UnityEngine;

public class LobbyListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text serverNameText;
    [SerializeField] private TMP_Text hostNameText;
    [SerializeField] private TMP_Text playerCountText;
    private CSteamID lobbyId;
    public void Init(string serverName, string hostName, int playerCount, CSteamID id)
    {
        serverNameText.text = serverName;
        hostNameText.text = hostName;
        playerCountText.text = playerCount.ToString();
        lobbyId = id;
    }

    public void JoinLobby()
    {
        SteamMatchmaking.JoinLobby(lobbyId);
    }
}
