using TMPro;
using UnityEngine;

public class ServerNameInputField : MonoBehaviour
{
    [SerializeField] private SteamLobbyCreateManager manager;
    [SerializeField] private TMP_InputField inputField;

    private void OnEnable()
    {
        inputField.text = manager.ServerName;
    }
}
