using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterNameController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button okButton;
    [SerializeField] private GameObject entireCanvas;

    private readonly string[] funnyNames = new string[]
    {
        "Sir Laughs-a-Lot",
        "Captain Quirk",
        "Waffle McPancake",
        "Banana Hammock",
        "Silly McGoose",
        "Duke Fuzzybottom",
        "Gigglesnort",
        "Professor Pickles",
        "Noodle Nugget",
        "Cheese Whiz",
        "Bumblebee Tuna",
        "Count Chocula",
        "Muffin Top",
        "Sassy Pants",
        "Toasty McToastface"
    };

    private int playerIndex;

    public void Init(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public void OnEnter()
    {
        EnableUI();
    }

    public void OnExit()
    {
        DisableUI();
    }

    private void EnableUI()
    {
        entireCanvas.SetActive(true);
        playerNameText.text = $"Player {playerIndex + 1} Name";
        nameInputField.text = string.Empty;
    }

    private void DisableUI()
    {
        entireCanvas.SetActive(false);
    }

    public void OnOkButtonClicked()
    {
        string enteredName = nameInputField.text;
        if (!string.IsNullOrEmpty(enteredName))
        {
            GameManager.Instance.SetPlayerPublicName(enteredName, playerIndex);
            GameManager.Instance.SetPlayerPrivateName(funnyNames[UnityEngine.Random.Range(0, funnyNames.Length)], playerIndex);

            // Randomize required and sinking attributes
            int[] required = new int[3];
            int[] sinking = new int[3];
            // Pick 2 random indices for required attributes
            int first = UnityEngine.Random.Range(0, 3);
            int second;
            do {
                second = UnityEngine.Random.Range(0, 3);
            } while (second == first);
            int third = 3 - first - second;
            // Assign random values
            required[first] = UnityEngine.Random.Range(50, 81);
            required[second] = UnityEngine.Random.Range(50, 81);
            required[third] = 0;
            sinking[first] = 0;
            sinking[second] = 0;
            sinking[third] = UnityEngine.Random.Range(5, 21);
            GameManager.Instance.SetPlayerRequiredAttributes(required, playerIndex);
            GameManager.Instance.SetPlayerSinkingAttributes(sinking, playerIndex);
            // here
        } else {
            return;
        }

        DisableUI();

        if (playerIndex >= GameStateManager.Instance.GetMaxPlayers()-1) {
            GameStateManager.Instance._playerIndex = 0; // Reset to 0 if player index exceeds max players;
            GameStateManager.Instance.UpdateLastState(StateID.Bargaining);
            GameStateManager.Instance.SetNextStateID(StateID.Bargaining);
            GameStateManager.Instance.SwitchState(StateID.PassPhone);
            // GameStateManager.Instance.SetNextStateID(StateID.EventTrigger);
            // GameStateManager.Instance.NextPlayer();
        } else {

            GameStateManager.Instance.UpdateLastState(StateID.EnterName);
            GameStateManager.Instance.SetNextStateID(StateID.Bargaining);
            GameStateManager.Instance.NextPlayer();
        }
        // GameStateManager.Instance.SwitchState(StateID.PassPhone);
    }
}
