using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggeringPhase : MonoBehaviour
{
    // Debugging
    private int playerIndex;

    // Card definition
    [System.Serializable]
    public class Card
    {
        public string name;
        public Attributes attb1;
        public int value1;
        public Attributes attb2;
        public int value2;
    }

    // Predefined cards
    public List<Card> cards = new List<Card>
    {
        new Card { name = "Temptation", attb1 = Attributes.Lust, value1 = 10, attb2 = Attributes.Intelligence, value2 = -5 },
        new Card { name = "Study Hard", attb1 = Attributes.Intelligence, value1 = 8, attb2 = Attributes.Fame, value2 = -3 },
        new Card { name = "Gamble", attb1 = Attributes.Money, value1 = 15, attb2 = Attributes.Charm, value2 = -7 },
        new Card { name = "Charity", attb1 = Attributes.Fame, value1 = 5, attb2 = Attributes.Money, value2 = -10 },
        new Card { name = "Flirt", attb1 = Attributes.Charm, value1 = 7, attb2 = Attributes.Lust, value2 = 3 },
    };

    private Card currentCard;
    private bool phaseActive = false;

    // Events for UI to subscribe to
    public event System.Action OnEnterEvent;
    public event System.Action<int> OnShowPlayerPopup;
    public event System.Action OnHidePlayerPopup;
    public event System.Action OnShowCardPickSprite;
    public event System.Action OnHideCardPickSprite;
    public event System.Action<Card> OnShowCard;
    public event System.Action OnHideCard;
    public event System.Action<Attributes, int, int, Attributes, int, int> OnAnimateAttributeChange;
    public event System.Action OnPhaseEnd;

    public void Init(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnEnter()
    {
        // Start the event triggering phase
        Debug.Log($"EventTriggeringPhase: Starting for player {playerIndex}");
        phaseActive = true;
        OnEnterEvent?.Invoke();
        StartCoroutine(PhaseFlow());
    }

    private IEnumerator PhaseFlow()
    {
        yield return new WaitForSeconds(0.5f); // Small delay before starting the flow
        Debug.Log($"EventTriggeringPhase: Flow started for player {playerIndex}");
        // 1. Show popup with player index
        OnShowPlayerPopup?.Invoke(playerIndex);
        yield return new WaitForSeconds(2.5f);
        // Add a small delay before showing the card picking animation
        yield return new WaitForSeconds(0.5f);
        OnHidePlayerPopup?.Invoke();

        // 2. Show animated sprite (card picking)
        OnShowCardPickSprite?.Invoke();
        yield return new WaitForSeconds(2.0f); // Wait for animation
        OnHideCardPickSprite?.Invoke();

        // 3. Pick a random card
        currentCard = cards[Random.Range(0, cards.Count)];

        // 4. Show card with animation
        OnShowCard?.Invoke(currentCard);
        yield return new WaitForSeconds(2.0f);

        // 5. Apply card effects to player data
        var player = GameManager.Instance.Players[playerIndex];
        int oldValue1 = player.currentAttbs[currentCard.attb1];
        int oldValue2 = player.currentAttbs[currentCard.attb2];
        int newValue1 = oldValue1 + currentCard.value1;
        int newValue2 = oldValue2 + currentCard.value2;
        player.currentAttbs[currentCard.attb1] = newValue1;
        player.currentAttbs[currentCard.attb2] = newValue2;

        // 6. Animate attribute changes
        OnAnimateAttributeChange?.Invoke(currentCard.attb1, oldValue1, newValue1, currentCard.attb2, oldValue2, newValue2);
        yield return new WaitForSeconds(1.5f + 1.0f); // match UI animation duration + wait
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        // ...existing code...
    }

    public void LeaveEventTriggeringPhase()
    {
        GameStateManager.Instance.UpdateLastState(StateID.EventTrigger);
        GameStateManager.Instance.NextPlayer();
    }

    public void OnExit()
    {
        // 7. Hide card
        OnHideCard?.Invoke();
        // Disable Everything related to the event triggering phase
        OnPhaseEnd?.Invoke();
        phaseActive = false;
    }
}
