using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    private GameState _currentState;
    private StateID _lastStateID;
    [SerializeField]
    private StateID _currentID;
    public int _playerIndex;
    private const int MaxPlayers = 4;

    [Header("Game States")]
    [SerializeField] EventTriggeringPhase eventTriggeringPhase;
    [SerializeField] PassingPhonePhase passingPhonePhase;

    void Awake()
    {
        Instance = this;
    } 

    void Start()
    {
        _playerIndex = 0;
        SwitchState(StateID.Start);
    }

    void Update()
    {
        _currentState?.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Example of switching state manually for testing
            UpdateLastState(StateID.EventTrigger);
            SwitchState(StateID.PassPhone);
        }
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
            StateID.Start => new StartPhase(),
            StateID.PassPhone => new PassPhonePhase(_playerIndex, passingPhonePhase),
            StateID.Bargaining => new BargainingPhase(_playerIndex),
            StateID.EventTrigger => new EventTriggerPhase(_playerIndex, eventTriggeringPhase),
            StateID.Negotiation => new NegotiationPhase(),
            StateID.ConditionCheck => new ConditionCheckPhase(_playerIndex),
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
}
