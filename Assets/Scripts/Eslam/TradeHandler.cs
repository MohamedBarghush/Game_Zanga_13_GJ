using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TradeHandler : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Image target;
    [SerializeField] private GameObject tradeParent;
    [SerializeField] private TMP_Text valueText;

    Dictionary<string, Dictionary<int, int>> playerValues;
    GameManager gameManager;
    PlayerData playerData;
    private static int currentAttb;

    void Start()
    {
        playerValues = new Dictionary<string, Dictionary<int, int>>();

        gameManager = GameManager.Instance;

        int playerIndex = GameStateManager.Instance.GetCurrentPlayerIndex();
        playerData = gameManager.Players[playerIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTradeUI(int attributeIndex, Sprite sprite)
    {
        currentAttb = attributeIndex;
        tradeParent.SetActive(true);
        target.sprite = sprite;
        parent.SetActive(false);

        valueText.text = "0";
    }

    public void onConfirmButtonPress()
    {
        // Update the biddingAttbs for the current player in GameManager
        int playerIndex = GameStateManager.Instance.GetCurrentPlayerIndex();
        int value = int.Parse(valueText.text);
        if (value >= 0)
        {
            // Set or update the bidding attribute for this player
            gameManager.SetPlayerBiddingAttribute(playerIndex, (Attributes)currentAttb, value);
        }
        else
        {
            // Remove the bidding attribute if value is zero
            var biddingAttbs = gameManager.Players[playerIndex].biddingAttbs;
            if (biddingAttbs != null && biddingAttbs.ContainsKey((Attributes)currentAttb))
            {
                biddingAttbs.Remove((Attributes)currentAttb);
            }
        }

        // Optionally update UI or text fields here if you have a summary display
        // For example, you could refresh a bidding summary UI here

        parent.SetActive(true);
        tradeParent.SetActive(false);
    }

    public void onAddButtonPress()
    {
        valueText.text = ((int.Parse(valueText.text)) + 1).ToString();   
    }
    public void onMinusButtonPress()
    {
        if (valueText.text == "0") return;
        valueText.text = ((int.Parse(valueText.text)) - 1).ToString();
    }
}
