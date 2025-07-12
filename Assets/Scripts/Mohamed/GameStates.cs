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
    //public override void OnUpdate() => Consoe.Log("Updating Start Phase");
    public override void OnExit() => Debug.Log("Exiting Start Phase");
}

public class PassPhonePhase : GameState {
    private int playerIndex;

    public PassPhonePhase(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }

    public override void OnEnter() => Debug.Log("Entering Pass Phone Phase");
    public override void OnUpdate() => Debug.Log("Updating Pass Phone Phase");
    public override void OnExit() => Debug.Log("Exiting Pass Phone Phase");
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
    public override void OnEnter() => Debug.Log("Entering Event Trigger Phase");
    public override void OnUpdate() => Debug.Log("Updating Event Trigger Phase");
    public override void OnExit() => Debug.Log("Exiting Event Trigger Phase");
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