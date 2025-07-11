using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public Dictionary<Attributes, int> currentAttbs { get; set; }
    public Dictionary<Attributes, int> requiredAttbs { get; set; }
    public PlayerData(int[] currentAttbs, int[] requiredAttbs) {
        this.currentAttbs = new Dictionary<Attributes, int>();
        this.requiredAttbs = new Dictionary<Attributes, int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(Attributes)).Length; i++)
        {
            this.currentAttbs[(Attributes)i] = currentAttbs[i];
            this.requiredAttbs[(Attributes)i] = requiredAttbs[i];
        }
    }
}

public enum Attributes { Fame, Lust, Money, Intelligence, Charm }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerData[] Players { get; private set; }

    void Awake()
    {
        Instance = this;
        InitializePlayers();
    }

    private void InitializePlayers()
    {
        Players = new PlayerData[4];
        for (int i = 0; i < Players.Length; i++)
            Players[i] = new PlayerData(new int[] { 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0 });
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
