using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseController : MonoBehaviour
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
    [SerializeField] private GameObject blankImage;

    private GameManager gameManager;
    private PlayerData playerData;
    private int currentPlayer;

    private void OnEnable()
    {
        currentPlayer = PlayerPrefs.GetInt("CurrentPlayer", 0);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerData = gameManager.Players[currentPlayer];

        SetupPlayerUI();
    }

    private void SetupPlayerUI()
    {
        publicNameText.text = playerData.playerName;
        internalNameText.text = playerData.privateName;

        SetupAttributeSection(playerData.currentAttbs, currentAttbUI, true);
        SetupAttributeSection(playerData.requiredAttbs, requiredAttbUI);
        SetupSinkingAttributes(playerData.sinkingAttbs);
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

    private void SetupSinkingAttributes(Dictionary<Attributes, Tuple<int, char>> attributes)
    {
        int index = 0;

        foreach (var kvp in attributes)
        {
            var (value, symbol) = kvp.Value;
            int attrIndex = (int)kvp.Key;

            if (value <= 0 || index >= sinkingAttbUI.Count)
                continue;

            GameObject uiObj = sinkingAttbUI[index];

            var image = uiObj.GetComponentInChildren<Image>();
            var label = uiObj.GetComponentInChildren<TMP_Text>();

            if (image != null) image.sprite = attributesSprites[attrIndex];
            if (label != null) label.text = $"{attributesNames[attrIndex]}{symbol}{value}";

            index++;
        }
    }

    public void OnNextPlayerPress()
    {
        if (currentPlayer <= 3)
        {
            PlayerPrefs.SetInt("CurrentPlayer", currentPlayer++);
            blankImage.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            // TODO: Transition to Negotiation Phase
        }
    }

    public void OnTradeButtonClicked(int attributeIndex)
    {
        Debug.Log($"Trade Button for Attribute Index: {attributeIndex}");

        TradeHandler tradeHandler = FindObjectOfType<TradeHandler>();
        if (tradeHandler != null)
        {
            tradeHandler.OpenTradeUI(attributeIndex, attributesSprites[attributeIndex]);
        }
    }
}
