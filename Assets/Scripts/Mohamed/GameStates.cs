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
    private IntroHandler introHandler;

    public StartPhase(IntroHandler introHandler)
    {
        this.introHandler = introHandler;
    }
    public override void OnEnter() => introHandler.OnPhaseEnter();
    //public override void OnUpdate() => Consoe.Log("Updating Start Phase");
    public override void OnExit() => Debug.Log("Exiting Start Phase");
}

public class EnterNamePhase : GameState {
    private int playerIndex;
    private EnterNameController enterNameController;

    public EnterNamePhase(int playerIndex, EnterNameController enterNameController)
    {
        this.playerIndex = playerIndex;
        this.enterNameController = enterNameController;
    }

    public override void OnEnter() {
        enterNameController.Init(playerIndex);
        enterNameController.OnEnter();
    }
    public override void OnUpdate() => Debug.Log("Updating Enter Name Phase");
    public override void OnExit() => Debug.Log("Exiting Enter Name Phase");
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
    private BargainingPhaseController bargainingPhaseController;
    public BargainingPhase(int playerIndex, BargainingPhaseController bargainingPhaseController)
    {
        this.playerIndex = playerIndex;
        this.bargainingPhaseController = bargainingPhaseController;
    }

    public override void OnEnter() {
        bargainingPhaseController.Init(playerIndex);
        bargainingPhaseController.OnPhaseEnter(); 
    }
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
    private NegoHandler negotiationPhase;
    public NegotiationPhase(NegoHandler negotiationPhase)
    {
        this.negotiationPhase = negotiationPhase;
    }

    public override void OnEnter() => negotiationPhase.OnEnter();
    public override void OnUpdate() => Debug.Log("Updating Negotiation Phase");
    public override void OnExit() => Debug.Log("Exiting Negotiation Phase");
}

public class ConditionCheckPhase : GameState {
    private EndStateHandler endStateHandler;

    public ConditionCheckPhase(EndStateHandler endStateHandler)
    {
        this.endStateHandler = endStateHandler;
    }

    public override void OnEnter() {
        endStateHandler.OnEnter();
        Debug.Log("Entering Condition Check Phase");
    }
    public override void OnUpdate() => Debug.Log("Updating Condition Check Phase");
    public override void OnExit() => endStateHandler.OnExit();
}

public class UpdateGamePhase : GameState {
    public override void OnEnter() => Debug.Log("Entering Update Game Phase");
    public override void OnUpdate() => Debug.Log("Updating Update Game Phase");
    public override void OnExit() => Debug.Log("Exiting Update Game Phase");
}