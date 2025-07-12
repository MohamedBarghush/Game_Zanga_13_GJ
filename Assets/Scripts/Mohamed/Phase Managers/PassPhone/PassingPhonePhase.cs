using UnityEngine;
using System;

public class PassingPhonePhase : MonoBehaviour
{
    private int playerIndex;
    public StateID nextPhase;

    // Events for UI
    public event Action<int> OnShowPassText;
    public event Action OnShowPanel;
    public event Action OnHidePanel;
    public event Action OnShowContinueButton;
    public event Action OnHideContinueButton;
    public event Action OnPhaseEnd;

    public void Init(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public void OnEnter()
    {
        // Clean up UI state before starting
        OnHidePanel?.Invoke();
        OnHideContinueButton?.Invoke();

        int maxPlayers = GameStateManager.Instance.GetMaxPlayers();
        playerIndex = GameStateManager.Instance.GetCurrentPlayerIndex();
        if (playerIndex >= maxPlayers)
        {
            // Debug.LogError("Player index is " + playerIndex + ", which exceeds the maximum number of players: " + maxPlayers);
            GameStateManager.Instance._playerIndex = 0; // Reset to 0 if player index exceeds max players;
            GameStateManager.Instance.SwitchState(nextPhase);
            return;
        }

        OnShowPanel?.Invoke();
        OnShowPassText?.Invoke(playerIndex);
        OnShowContinueButton?.Invoke();
    }

    public void OnContinue()
    {
        OnHidePanel?.Invoke();
        OnHideContinueButton?.Invoke();

        if (playerIndex >= GameStateManager.Instance.GetMaxPlayers())
        {
            GameStateManager.Instance.SwitchState(nextPhase);
        }
        else
        {
            GameStateManager.Instance.SwitchState(GameStateManager.Instance.GetLastStateID());
        }
        OnPhaseEnd?.Invoke();
    }

    public void OnExit()
    {
        // Clean up UI state on exit
        OnHidePanel?.Invoke();
        OnHideContinueButton?.Invoke();
    }
}
