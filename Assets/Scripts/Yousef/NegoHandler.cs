using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class NegoHandler : MonoBehaviour
{
    
    [SerializeField] private GameObject enterPopup;
    [SerializeField] private List<GameObject> player1;
    [SerializeField] private List<GameObject> player2;
    [SerializeField] private List<GameObject> player3;
    [SerializeField] private List<GameObject> player4;
    [SerializeField] private List<TMP_Text> dealers;
    [SerializeField] private List<Button> lockButtons;
    [SerializeField] private List<TMP_Text> lockButtonTexts;
    [SerializeField] private List<TMP_Text> lockedIdx;
    [SerializeField] private List<Sprite> allSprites;
    [SerializeField] private GameObject negotiationCanvas;

    List<List<GameObject>> allPlayerData;

    public static List<lockedBid> lockedBids;
    private int? firstSelectedPlayer = null;

    public void OnEnter() {
        lockedBids = new List<lockedBid>();
        UpdateLockedIdxTexts();
        // Update lockedIdx text for all players
        for (int i = 0; i < lockedIdx.Count; i++)
        {
            int partnerIdx = -1;
            // Find if this player is locked with someone
            foreach (var bid in lockedBids)
            {
                if (bid.dealerIndex == i)
                {
                    partnerIdx = bid.receiverIndex;
                    break;
                }
            }
            if (partnerIdx != -1)
            {
                // Show partner's name
                lockedIdx[i].text = GameManager.Instance.Players[partnerIdx].playerName;
            }
            else
            {
                lockedIdx[i].text = "";
            }
        }
        // Show popup using DoTween
        if (enterPopup != null)
        {
            enterPopup.SetActive(true);
            enterPopup.transform.localScale = Vector3.zero;
            enterPopup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    // Hide after 1.2 seconds
                    enterPopup.transform.DOScale(Vector3.zero, 0.3f).SetDelay(1.2f).SetEase(Ease.InBack)
                        .OnComplete(() => enterPopup.SetActive(false));
                });
        }
        // Initialize the locked bids list
        lockedBids = new List<lockedBid>(); 

        // Group all player GameObject lists for easier access
        allPlayerData = new List<List<GameObject>> { player1, player2, player3, player4 };

        // Get all player names from the GameManager
        var playerNames = GameManager.Instance.Players.Select(p => p.playerName).ToList();

        // Loop through each player to set up UI and data
        for (int i = 0; i < GameManager.Instance.Players.Length; i++)
        {
            var playerData = GameManager.Instance.Players[i];
            var bidding = playerData.biddingAttbs;

            // Set the dealer name in the UI
            if (dealers.Count > i)
                dealers[i].text = playerData.playerName;

            int totalCount = 0;

            // For each non-zero bid, update the UI slot with sprite and value
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

            // Hide unused UI slots for this player
            for (int j = totalCount; j < allPlayerData[i].Count; j++)
                allPlayerData[i][j].SetActive(false);

            // Set up button listeners and text
            if (lockButtons.Count > i && lockButtonTexts.Count > i)
            {
                int idx = i;
                lockButtons[i].onClick.RemoveAllListeners();
                lockButtons[i].onClick.AddListener(() => OnLockButtonPress(idx));
                lockButtonTexts[i].text = "LOCK";
            }

            negotiationCanvas.SetActive(true);
        }
    }
    // Button logic for locking/unlocking two players together
    public void OnLockButtonPress(int index)
    {
        if (index < 0 || index >= GameManager.Instance.Players.Length)
            return;

        // If this player is already locked, unlock them
        if (IsPlayerLocked(index))
        {
            UnlockPlayer(index);
            return;
        }

        // If no player is selected, select this one and mark orange
        if (firstSelectedPlayer == null)
        {
            firstSelectedPlayer = index;
            SetButtonColor(index, Color.Lerp(Color.white, new Color(1f, 0.5f, 0f), 0.7f)); // Orange
            return;
        }

        // If the same player is clicked again, deselect and set to white
        if (firstSelectedPlayer == index)
        {
            SetButtonColor(index, Color.white);
            firstSelectedPlayer = null;
            return;
        }

        // Lock both players together
        int otherIndex = firstSelectedPlayer.Value;
        LockPlayersTogether(otherIndex, index);
        firstSelectedPlayer = null;
    }

    // Helper to set button color
    private void SetButtonColor(int index, Color color)
    {
        if (lockButtons.Count > index && lockButtons[index] != null)
        {
            var colors = lockButtons[index].colors;
            colors.normalColor = color;
            colors.selectedColor = color;
            colors.highlightedColor = color;
            colors.pressedColor = color;
            lockButtons[index].colors = colors;
        }
    }

    private void LockPlayersTogether(int indexA, int indexB)
    {
        UpdateLockedIdxTexts();
        // Update lockedIdx text for both players
        if (lockedIdx.Count > indexA)
            lockedIdx[indexA].text = GameManager.Instance.Players[indexB].playerName;
        if (lockedIdx.Count > indexB)
            lockedIdx[indexB].text = GameManager.Instance.Players[indexA].playerName;

        // Get all attributes with bidding values for indexA
        var biddingAttributes = GameManager.Instance.Players[indexA].biddingAttbs
            .Where(kvp => kvp.Value != 0 && kvp.Value != -1)
            .ToList();

        foreach (var kvp in biddingAttributes)
        {
            var assignedAttribute = kvp.Key;
            var assignedValue = kvp.Value;

            // Lock both ways for easy lookup
            lockedBids.Add(new lockedBid(indexA, indexB, assignedValue, assignedAttribute));
            // lockedBids.Add(new lockedBid(indexB, indexA, assignedValue, assignedAttribute));
        }

        // Update button text and color
        lockButtonTexts[indexA].text = "UNLOCK";
        lockButtonTexts[indexB].text = "UNLOCK";
        SetButtonColor(indexA, Color.red);
        SetButtonColor(indexB, Color.red);
    }

    // Updates lockedIdx text for all players: shows partner's name if locked, else clears
    private void UpdateLockedIdxTexts()
    {
        for (int i = 0; i < lockedIdx.Count; i++)
        {
            int partnerIdx = -1;
            foreach (var bid in lockedBids)
            {
                if (bid.dealerIndex == i)
                {
                    partnerIdx = bid.receiverIndex;
                    break;
                }
            }
            if (partnerIdx != -1)
                lockedIdx[i].text = GameManager.Instance.Players[partnerIdx].playerName;
            else
                lockedIdx[i].text = "";
        }
    }

    private void UnlockPlayer(int index) {
        // Clear lockedIdx text for this player
        if (lockedIdx.Count > index)
            lockedIdx[index].text = "";
        // Also clear for any partner
        for (int i = 0; i < GameManager.Instance.Players.Length; i++)
        {
            if (i == index) continue;
            if (lockedIdx.Count > i && lockedBids.All(b => b.dealerIndex != i || b.receiverIndex != index))
                lockedIdx[i].text = "";
        }
        // Remove all lockedBids involving this player
        lockedBids.RemoveAll(b => b.dealerIndex == index || b.receiverIndex == index);
        // Reset button text and color
        lockButtonTexts[index].text = "LOCK";
        SetButtonColor(index, Color.white);
        // Also reset the other player's button if they were paired
        for (int i = 0; i < GameManager.Instance.Players.Length; i++)
        {
            if (i == index) continue;
            if (lockedBids.Any(b => (b.dealerIndex == index && b.receiverIndex == i) || (b.dealerIndex == i && b.receiverIndex == index)))
            {
                lockButtonTexts[i].text = "LOCK";
                SetButtonColor(i, Color.white);
            }
        }
    }

    private bool IsPlayerLocked(int index)
    {
        return lockedBids.Any(b => b.dealerIndex == index || b.receiverIndex == index);
    }

    // Added UpdateLockedIdxTexts to ensure UI reflects changes
    public void OnReBet () {
        lockedBids.Clear();
        UpdateLockedIdxTexts(); // Ensure lockedIdx is cleared
        GameStateManager.Instance.UpdateLastState(StateID.Bargaining);
        GameStateManager.Instance.SwitchState(StateID.PassPhone);
    }

    // Added UpdateLockedIdxTexts to reflect applied bids
    public void OnContinue () {
        ApplyBids();
        negotiationCanvas.SetActive(false);
        GameStateManager.Instance.SwitchState(StateID.EventTrigger);
    }

    public void ApplyBids() {
        Debug.LogError("Applying bids... Total locked bids: " + lockedBids.Count);
        foreach (var bid in lockedBids)
        {
            // Apply the locked bid's attribute and value to the receiver's data
            GameManager.Instance.Players[bid.receiverIndex].currentAttbs[bid.att] += bid.value;
            GameManager.Instance.Players[bid.dealerIndex].currentAttbs[bid.att] -= bid.value;

            // Reset bidding attributes for both players
            foreach (Attributes attr in System.Enum.GetValues(typeof(Attributes)))
            {
                GameManager.Instance.Players[bid.receiverIndex].biddingAttbs[attr] = 0;
                GameManager.Instance.Players[bid.dealerIndex].biddingAttbs[attr] = 0;
            }
        }
        UpdateLockedIdxTexts(); // Ensure UI reflects the updated state
    }
}

public class lockedBid
{
    public int dealerIndex { get; set; }
    public int receiverIndex { get; set; }
    public int value { get; set; }
    public Attributes att { get; set; }
    public lockedBid(int dealerIndex, int receiverIndex, int value, Attributes att)
    {
        this.dealerIndex = dealerIndex;
        this.receiverIndex = receiverIndex;
        this.value = value;
        this.att = att;
    }
}
