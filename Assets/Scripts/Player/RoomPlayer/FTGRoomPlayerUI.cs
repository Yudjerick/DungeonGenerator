using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FTGRoomPlayerUI : MonoBehaviour
{
    [SerializeField] private FTGRoomPlayer model;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private Image readyButtonImg;
    [SerializeField] private TMP_Text playerNameText;

    private void OnEnable()
    {
        model.UpdatedEvent += UpdateUI;
    }

    private void OnDisable()
    {
        model.UpdatedEvent -= UpdateUI;
    }

    private void UpdateUI()
    {
        classText.text = model.PlayerPrefabs[model.SelectedPlayerIdx].name;
        readyButtonImg.color = model.readyToBegin ? Color.green : Color.white;
        playerNameText.text = model.playerName;
    }
    private void Start()
    {
        UpdateUI();
    }
}
