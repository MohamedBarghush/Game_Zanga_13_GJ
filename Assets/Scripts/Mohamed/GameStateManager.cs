using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    private GameState _currentState;
    [SerializeField] private StateID _lastStateID;
    [SerializeField] private StateID _nextStateID;
    [SerializeField] private StateID _currentID;
    public int _playerIndex;
    private const int MaxPlayers = 4;

    [Header("Game States")]
    [SerializeField] EventTriggeringPhase eventTriggeringPhase;
    [SerializeField] PassingPhonePhase passingPhonePhase;
    [SerializeField] BargainingPhaseController bargainingPhaseController;
    [SerializeField] NegoHandler negotiationPhase;
    [SerializeField] EnterNameController enterNameController;
    [SerializeField] IntroHandler introHandler;
    [SerializeField] EndStateHandler endStateHandler;

    void Awake()
    {
        Instance = this;
    } 

    void Start()
    {
        _playerIndex = 0;
        SwitchState(StateID.Start);
        // SwitchState(StateID.PassPhone);
    }

    void Update()
    {
        _currentState?.OnUpdate();

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     // Example of switching state manually for testing
        //     UpdateLastState(StateID.EventTrigger);
        //     SwitchState(StateID.EventTrigger);
        // }

        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     // Example of switching state manually for testing
        //     UpdateLastState(StateID.Bargaining);
        //     SwitchState(StateID.PassPhone);
        // }

        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     SwitchState(StateID.Negotiation);
        // }
    }

    public void SwitchState(StateID newID)
    {
        // Allow manual state switching from the Inspector in Editor
#if UNITY_EDITOR
        if (_currentID != newID)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        if (_currentState != null && _currentID == newID) return;

        _currentState?.OnExit();

        _currentID = newID;
        _currentState = CreateState(newID);
        _currentState?.Init(this);
        _currentState?.OnEnter();
    }

    private GameState CreateState(StateID id)
    {
        return id switch
        {
            StateID.Start => new StartPhase(introHandler),
            StateID.EnterName => new EnterNamePhase(_playerIndex, enterNameController),
            StateID.PassPhone => new PassPhonePhase(_playerIndex, passingPhonePhase),
            StateID.Bargaining => new BargainingPhase(_playerIndex, bargainingPhaseController),
            StateID.EventTrigger => new EventTriggerPhase(_playerIndex, eventTriggeringPhase),
            StateID.Negotiation => new NegotiationPhase(negotiationPhase),
            StateID.ConditionCheck => new ConditionCheckPhase(endStateHandler),
            StateID.UpdateGame => new UpdateGamePhase(),
            _ => null
        };
    }

    public void UpdateLastState (StateID newID)
    {
        _lastStateID = newID;
    }

    public StateID GetLastStateID()
    {
        return _lastStateID;
    }

    public void NextPlayer()
    {
        _playerIndex ++;
        SwitchState(StateID.PassPhone);
    }

    public int GetMaxPlayers() => MaxPlayers;

    public int GetCurrentPlayerIndex() => _playerIndex;
    public StateID GetCurrentStateID() => _currentID;

    public StateID GetNextStateID()
    {
        return _nextStateID;
    }

    public void SetNextStateID(StateID nextStateID)
    {
        _nextStateID = nextStateID;
    }
}
