using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public Dictionary<Attributes, int> currentAttbs { get; set; }
    public Dictionary<Attributes, int> requiredAttbs { get; set; }
    public Dictionary<Attributes, Tuple<int, char>> sinkingAttbs { get; set; }
    // X: 50, '>'
    public string playerName { get; set; }
    public string privateName { get; set; }
    public PlayerData(int[] currentAttbs, int[] requiredAttbs) 
    {
        this.currentAttbs = new Dictionary<Attributes, int>();
        this.requiredAttbs = new Dictionary<Attributes, int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(Attributes)).Length; i++)
        {
            this.currentAttbs[(Attributes)i] = currentAttbs[i];
            this.requiredAttbs[(Attributes)i] = requiredAttbs[i];
        }
    }
    public PlayerData(int[] currentAttbs, int[] requiredAttbs, List<Tuple<int, char>> sinkingAttbs, string generatedName, string playerName)
    {
        this.currentAttbs = new Dictionary<Attributes, int>();
        this.requiredAttbs = new Dictionary<Attributes, int>();
        this.sinkingAttbs = new Dictionary<Attributes, Tuple<int, char>>(); 
        this.privateName = generatedName;

        for (int i = 0; i < System.Enum.GetValues(typeof(Attributes)).Length; i++)
        {
            this.currentAttbs[(Attributes)i] = currentAttbs[i];
            this.requiredAttbs[(Attributes)i] = requiredAttbs[i];
            this.sinkingAttbs[(Attributes)i] = sinkingAttbs[i]; 
        }

        this.playerName = playerName;
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
            Players[i] = new PlayerData(new int[] { -1, 60, 70, 30, -1 }, new int[] { -1, 80, 50, -1, -1 }, new List<Tuple<int, char>> { new(-1, '>')
                , new(-1, '>'), new(-1, '>'),new(-1, '>'),new(20, '>')}, "tstFake", "tstReal");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
