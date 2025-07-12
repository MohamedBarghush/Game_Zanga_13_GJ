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
    void Start()
    {
        playerValues = new Dictionary<string, Dictionary<int, int>>();

        gameManager = GameManager.Instance;

        playerData = gameManager.Players[PlayerPrefs.GetInt("CurrentPlayer", 0)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int currentAttb = 0;

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

        if (playerValues.Keys.Contains(playerData.playerName))
        {
            Dictionary<int, int> keyValuePairs = playerValues[playerData.playerName];
            if(keyValuePairs.Keys.Contains(currentAttb))
            {
                if(int.Parse(valueText.text) != 0)
                {
                    keyValuePairs[currentAttb] = int.Parse(valueText.text);
                }
                else
                {
                    keyValuePairs.Remove(currentAttb);
                }
            }
            else
            {
                if (int.Parse(valueText.text) != 0)
                {
                    playerValues[playerData.playerName].Add(currentAttb, int.Parse(valueText.text));
                }
            }
        }
        else
        {
            if (int.Parse(valueText.text) != 0)
            {
                playerValues.Add(playerData.playerName, new Dictionary<int, int>()
            {
                { currentAttb, int.Parse(valueText.text)}
            }
                );
            }
        }

        parent.SetActive(true);

        tradeParent.SetActive(false);

    }

    public void onAddButtonPress()
    {
        valueText.text = ((int.Parse(valueText.text)) + 1).ToString();   
    }
    public void onMinusButtonPress()
    {
        valueText.text = ((int.Parse(valueText.text)) - 1).ToString();
    }
}
