using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggeringPhase : MonoBehaviour
{
    private int playerIndex;

    public StateID nextStateID;

    public enum SpecialEffectType
    {
        MultiplyBy2,
        MultiplyBy3,
        CutInHalf,
        Add10,
        Subtract10,
        Add50Percent,
        Subtract25Percent
    }

    public class SpecialEffect
    {
        public string name;
        public SpecialEffectType effectType;
        public Attributes attribute;
    }

    private SpecialEffect currentEffect;

    public event System.Action OnEnterEvent;
    public event System.Action<int> OnShowPlayerPopup;
    public event System.Action OnHidePlayerPopup;
    public event System.Action OnShowCardPickSprite;
    public event System.Action OnHideCardPickSprite;
    public event System.Action<string> OnShowEffect; // Show effect name/description
    public event System.Action OnHideCard;
    public event System.Action<Attributes, int[], int[]> OnAnimateAttributeChange; // Attribute, old values, new values
    public event System.Action OnPhaseEnd;

    public void Init(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public void OnEnter()
    {
        Debug.Log($"EventTriggeringPhase: Special effect phase");
        OnEnterEvent?.Invoke();
        StartCoroutine(PhaseFlow());
    }

    private IEnumerator PhaseFlow()
    {
        yield return new WaitForSeconds(0.5f);
        OnShowPlayerPopup?.Invoke(playerIndex);
        yield return new WaitForSeconds(1.0f);
        OnHidePlayerPopup?.Invoke();

        OnShowCardPickSprite?.Invoke();
        yield return new WaitForSeconds(2.5f);
        OnHideCardPickSprite?.Invoke();

        // 1. Pick a random attribute
        var attributes = System.Enum.GetValues(typeof(Attributes));
        Attributes chosenAttr = (Attributes)attributes.GetValue(Random.Range(0, attributes.Length));

        // 2. Pick a random effect
        SpecialEffectType[] possibleEffects = new SpecialEffectType[] {
            SpecialEffectType.MultiplyBy2,
            SpecialEffectType.MultiplyBy3,
            SpecialEffectType.CutInHalf,
            SpecialEffectType.Add10,
            SpecialEffectType.Subtract10,
            SpecialEffectType.Add50Percent,
            SpecialEffectType.Subtract25Percent
        };
        SpecialEffectType chosenEffect = possibleEffects[Random.Range(0, possibleEffects.Length)];

        string effectDesc = GetEffectDescription(chosenEffect, chosenAttr);
        currentEffect = new SpecialEffect { name = effectDesc, effectType = chosenEffect, attribute = chosenAttr };

        // 3. Show effect description
        OnShowEffect?.Invoke(effectDesc);
        yield return new WaitForSeconds(2.0f);

        // 4. Apply effect to all players
        var players = GameManager.Instance.Players;
        int[] oldVals = new int[players.Length];
        int[] newVals = new int[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            int oldVal = players[i].currentAttbs[chosenAttr];
            int newVal = ApplyEffect(chosenEffect, oldVal);
            oldVals[i] = oldVal;
            newVals[i] = newVal;
            players[i].currentAttbs[chosenAttr] = newVal;
        }

        // 5. Animate attribute changes for all players
        OnAnimateAttributeChange?.Invoke(chosenAttr, oldVals, newVals);
        yield return new WaitForSeconds(2.5f);
    }

    private int ApplyEffect(SpecialEffectType effect, int value)
    {
        switch (effect)
        {
            case SpecialEffectType.MultiplyBy2: return value * 2;
            case SpecialEffectType.MultiplyBy3: return value * 3;
            case SpecialEffectType.CutInHalf: return Mathf.Max(1, value / 2);
            case SpecialEffectType.Add10: return value + 10;
            case SpecialEffectType.Subtract10: return value - 10;
            case SpecialEffectType.Add50Percent: return value + (value / 2);
            case SpecialEffectType.Subtract25Percent: return value - (value / 4);
            default: return value;
        }
    }

    private string GetEffectDescription(SpecialEffectType effect, Attributes attr)
    {
        switch (effect)
        {
            case SpecialEffectType.MultiplyBy2: return $"Special Game Effect\n All players' {attr} is multiplied by 2!";
            case SpecialEffectType.MultiplyBy3: return $"Special Game Effect\n All players' {attr} is multiplied by 3!";
            case SpecialEffectType.CutInHalf: return $"Special Game Effect\n All players' {attr} is cut in half! (minimum 1)";
            case SpecialEffectType.Add10: return $"Special Game Effect\n All players' {attr} increases by 10!";
            case SpecialEffectType.Subtract10: return $"Special Game Effect\n All players' {attr} decreases by 10!";
            case SpecialEffectType.Add50Percent: return $"Special Game Effect\n All players' {attr} increases by 50%!";
            case SpecialEffectType.Subtract25Percent: return $"Special Game Effect\n All players' {attr} decreases by 25%!";
            default: return $"Special Game Effect\n All players' {attr} is changed!";
        }
    }

    public void OnUpdate()
    {
        // ...existing code...
    }

    public void LeaveEventTriggeringPhase()
    {
        // GameStateManager.Instance.UpdateLastState(StateID.EventTrigger);
        // GameStateManager.Instance.NextPlayer();
        GameStateManager.Instance.SwitchState(StateID.ConditionCheck);
    }

    public void OnExit()
    {
        OnHideCard?.Invoke();
        OnPhaseEnd?.Invoke();
    }
}
