using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NegoHandler : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private List<GameObject> player1;
    [SerializeField] private List<GameObject> player2;
    [SerializeField] private List<GameObject> player3;
    [SerializeField] private List<GameObject> player4;
    [SerializeField] private List<TMP_Text> dealers;
    [SerializeField] private List<TMP_Dropdown> dropdowns;
    [SerializeField] private List<Sprite> allSprites;

    List<List<GameObject>> allPlayerData;

    public static List<lockedBid> lockedBids;

    private void Start()
    {
        lockedBids = new List<lockedBid>(); 

        gameManager = GameManager.Instance;

        allPlayerData = new List<List<GameObject>> { player1, player2, player3, player4 };
        var playerNames = gameManager.Players.Select(p => p.playerName).ToList();

        for (int i = 0; i < gameManager.Players.Length; i++)
        {
            var playerData = gameManager.Players[i];
            var bidding = playerData.biddingAttbs;

            if (dealers.Count > i)
                dealers[i].text = playerData.playerName;

            TMP_Dropdown dropdown = dropdowns[i];

            if (dropdown != null)
            {
                var otherNames = playerNames
                    .Where((name, index) => index != i)
                    .Prepend("No choice") 
                    .ToList();

                dropdown.ClearOptions();
                dropdown.AddOptions(otherNames);
                dropdown.value = 0; 

                dropdown.ClearOptions();
                dropdown.AddOptions(otherNames);
            }

            int totalCount = 0;

            foreach (var kvp in bidding.Where(kvp => kvp.Value != 0))
            {
                var currentSlot = allPlayerData[i][totalCount];
                var images = currentSlot.GetComponentsInChildren<Image>();
                if (images.Length > 1)
                    images[1].sprite = allSprites[(int)kvp.Key];

                var text = currentSlot.GetComponentInChildren<TMP_Text>();
                if (text != null)
                    text.text = kvp.Value.ToString();

                totalCount++;
            }

            for (int j = totalCount; j < allPlayerData[i].Count; j++)
                allPlayerData[i][j].SetActive(false);
        }
    }
    public void OnButtonPress(int index)
    {
        if (index < 0 || index >= gameManager.Players.Length)
        {
            Debug.LogWarning("Invalid player index.");
            return;
        }

        TMP_Dropdown dropdown = dropdowns[index];
        if (dropdown == null || dropdown.value == 0)
        {
            Debug.Log("No choice selected for player " + index);
            return;
        }

        string dealerName = gameManager.Players[index].playerName;
        string receiverName = dropdown.options[dropdown.value].text;

        Dictionary<Attributes, int> bidding = gameManager.Players[index].biddingAttbs;

        foreach (var kvp in bidding)
        {
            if (kvp.Value != 0)
            {
                lockedBid newBid = new lockedBid(dealerName, receiverName, kvp.Value, kvp.Key);
                Debug.Log($"Locked Bid → Dealer: {dealerName}, Receiver: {receiverName}, Attribute: {kvp.Key}, Value: {kvp.Value}");
                lockedBids.Add(newBid);
            }
        }
    }

}

public class lockedBid
{
    string dealer { get; set; }
    string reciever {  get; set; }
    int value { get; set; }
    Attributes att { get; set; }
    public lockedBid(string dealer, string reciever, int value, Attributes att)
    {
        this.dealer = dealer;
        this.reciever = reciever;
        this.value = value;
        this.att = att;
    }
}
