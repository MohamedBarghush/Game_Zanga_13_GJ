using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    private GameState _currentState;
    [SerializeField]
    private StateID _currentID;
    private int _playerIndex;
    private const int MaxPlayers = 4;

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
            StateID.PassPhone => new PassPhonePhase(_playerIndex),
            StateID.Bargaining => new BargainingPhase(_playerIndex),
            StateID.EventTrigger => new EventTriggerPhase(),
            StateID.Negotiation => new NegotiationPhase(),
            StateID.ConditionCheck => new ConditionCheckPhase(_playerIndex),
            StateID.UpdateGame => new UpdateGamePhase(),
            _ => null
        };
    }

    public void NextPlayer()
    {
        _playerIndex = (_playerIndex + 1) % MaxPlayers;
        SwitchState(StateID.PassPhone);
    }

    public int GetCurrentPlayerIndex() => _playerIndex;
    public StateID GetCurrentStateID() => _currentID;
}
