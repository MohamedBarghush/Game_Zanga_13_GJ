using UnityEngine;

public abstract class GameState
{
    protected GameStateManager machine;

    public void Init(GameStateManager machine) => this.machine = machine;

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}

public class StartPhase : GameState{
    public override void OnEnter() => Debug.Log("Entering Start Phase");
    public override void OnUpdate() => Debug.Log("Updating Start Phase");
    public override void OnExit() => Debug.Log("Exiting Start Phase");
}

public class PassPhonePhase : GameState {
    private int playerIndex;
    private PassingPhonePhase passingPhonePhase;

    public PassPhonePhase(int playerIndex, PassingPhonePhase passingPhonePhase)
    {
        this.playerIndex = playerIndex;
        this.passingPhonePhase = passingPhonePhase;
    }

    public override void OnEnter()
    {
        passingPhonePhase.Init(playerIndex);
        passingPhonePhase.OnEnter();
    }
    public override void OnUpdate() { }
    public override void OnExit()
    {
        passingPhonePhase.OnExit();
    }
}

public class BargainingPhase : GameState {

    private int playerIndex;
    public BargainingPhase(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public override void OnEnter() => Debug.Log("Entering Bargaining Phase");
    public override void OnUpdate() => Debug.Log("Updating Bargaining Phase");
    public override void OnExit() => Debug.Log("Exiting Bargaining Phase");
}

public class EventTriggerPhase : GameState {
    private int playerIndex;
    private EventTriggeringPhase currentPhase;
    public EventTriggerPhase(int playerIndex, EventTriggeringPhase currentPhase)
    {
        this.playerIndex = playerIndex;
        this.currentPhase = currentPhase;
    }
    public override void OnEnter() {
        currentPhase.Init(playerIndex);
        currentPhase.OnEnter();
    }
    public override void OnUpdate() => currentPhase.OnUpdate();
    public override void OnExit() => currentPhase.OnExit();
}

public class NegotiationPhase : GameState {
    public override void OnEnter() => Debug.Log("Entering Negotiation Phase");
    public override void OnUpdate() => Debug.Log("Updating Negotiation Phase");
    public override void OnExit() => Debug.Log("Exiting Negotiation Phase");
}

public class ConditionCheckPhase : GameState {
    private int playerIndex;

    public ConditionCheckPhase(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public override void OnEnter() => Debug.Log("Entering Condition Check Phase");
    public override void OnUpdate() => Debug.Log("Updating Condition Check Phase");
    public override void OnExit() => Debug.Log("Exiting Condition Check Phase");
}

public class UpdateGamePhase : GameState {
    public override void OnEnter() => Debug.Log("Entering Update Game Phase");
    public override void OnUpdate() => Debug.Log("Updating Update Game Phase");
    public override void OnExit() => Debug.Log("Exiting Update Game Phase");
}