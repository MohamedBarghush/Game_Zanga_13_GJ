using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BargainingPhaseController : MonoBehaviour
{
    [Header("Attribute UI References")]
    [SerializeField] private List<Sprite> attributesSprites;
    [SerializeField] private List<string> attributesNames;
    [SerializeField] private List<Button> tradeButtons;
    [SerializeField] private List<GameObject> currentAttbUI;
    [SerializeField] private List<GameObject> requiredAttbUI;
    [SerializeField] private List<GameObject> sinkingAttbUI;

    [Header("Player Info")]
    [SerializeField] private TMP_Text publicNameText;
    [SerializeField] private TMP_Text internalNameText;
    [SerializeField] private GameObject entireCanvas;

    private PlayerData playerData;
    private int _playerIndex;

    public void Awake()
    {
        if (entireCanvas != null)
        {
            DisableEverything();
        }
    }

    public void Init(int playerIndex)
    {
        _playerIndex = playerIndex;
    }

    public void OnPhaseEnter()
    {
        playerData = GameManager.Instance.GetPlayerData(_playerIndex);
        EnableEverything();
        SetupPlayerUI();
    }

    private void EnableEverything()
    {
        entireCanvas.SetActive(true);
    }

    private void DisableEverything()
    {
        entireCanvas.SetActive(false);
    }

    private void SetupPlayerUI()
    {
        publicNameText.text = playerData.playerName;
        internalNameText.text = playerData.privateName;

        SetupAttributeSection(playerData.currentAttbs, currentAttbUI, true);
        SetupAttributeSection(playerData.requiredAttbs, requiredAttbUI);
        SetupAttributeSection(playerData.sinkingAttbs, sinkingAttbUI);
    }

    private void SetupAttributeSection(Dictionary<Attributes, int> attributes, List<GameObject> uiElements, bool setupTradeButtons = false)
    {
        int index = 0;

        foreach (var kvp in attributes)
        {
            int attrValue = kvp.Value;
            int attrIndex = (int)kvp.Key;

            if (attrValue <= 0 || index >= uiElements.Count)
                continue;

            GameObject uiObj = uiElements[index];

            var image = uiObj.GetComponentInChildren<Image>();
            var label = uiObj.GetComponentInChildren<TMP_Text>();

            if (image != null) image.sprite = attributesSprites[attrIndex];
            if (label != null) label.text = $"{attributesNames[attrIndex]} : {attrValue}";

            if (setupTradeButtons && index < tradeButtons.Count)
            {
                int capturedIndex = attrIndex;
                tradeButtons[index].image.sprite = attributesSprites[attrIndex];
                tradeButtons[index].onClick.AddListener(() => OnTradeButtonClicked(capturedIndex));
            }

            index++;
        }
    }

    public void OnNextPlayerPress()
    {
        GameStateManager.Instance.UpdateLastState(StateID.Bargaining);
        GameStateManager.Instance.NextPlayer();
        DisableEverything();
    }

    public void OnTradeButtonClicked(int attributeIndex)
    {
        Debug.Log($"Trade Button for Attribute Index: {attributeIndex}");

        TradeHandler tradeHandler = FindAnyObjectByType<TradeHandler>();
        if (tradeHandler != null)
        {
            tradeHandler.OpenTradeUI(attributeIndex, attributesSprites[attributeIndex]);
        }
    }
}
