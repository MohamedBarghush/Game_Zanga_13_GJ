using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public Dictionary<Attributes, int> currentAttbs { get; set; }
    public Dictionary<Attributes, int> requiredAttbs { get; set; }
    public Dictionary<Attributes, int> sinkingAttbs { get; set; }
    public Dictionary<Attributes, int> biddingAttbs { get; set; }
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
    public PlayerData(int[] currentAttbs, int[] requiredAttbs, int[] sinkingAttbs, int[] biddingAttb, string generatedName, string playerName)
    {
        this.currentAttbs = new Dictionary<Attributes, int>();
        this.requiredAttbs = new Dictionary<Attributes, int>();
        this.sinkingAttbs = new Dictionary<Attributes, int>();
        this.biddingAttbs = new Dictionary<Attributes, int>();
        this.privateName = generatedName;

        for (int i = 0; i < System.Enum.GetValues(typeof(Attributes)).Length; i++)
        {
            this.currentAttbs[(Attributes)i] = currentAttbs[i];
            this.requiredAttbs[(Attributes)i] = requiredAttbs[i];
            this.sinkingAttbs[(Attributes)i] = sinkingAttbs[i];
            this.biddingAttbs[(Attributes)i] = biddingAttb[i];
        }

        this.playerName = playerName;
    }

}


public enum Attributes { Fame, Charm, Money, Intelligence, Health }

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
            Players[i] = new PlayerData(
                new int[] { 0, 60, 70, 30, 0 },
                new int[] { 0, 80, 50, 0, 0 },
                new int[] { 0, 0, 0, 0, 20 },
                new int[] { 0, 0, 0, 0, 0 },
                "tstFake",
                "tstReal"
            );
    }

    public PlayerData GetPlayerData (int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= Players.Length)
            throw new ArgumentOutOfRangeException(nameof(playerIndex), "Invalid player index");

        return Players[playerIndex];
    }

    public void SetPlayerBiddingAttribute(int playerIndex, Attributes attribute, int value)
    {
        if (playerIndex < 0 || playerIndex >= Players.Length)
            throw new System.ArgumentOutOfRangeException(nameof(playerIndex), "Invalid player index");

        if (Players[playerIndex].biddingAttbs == null)
            Players[playerIndex].biddingAttbs = new Dictionary<Attributes, int>();

        Players[playerIndex].biddingAttbs[attribute] = value;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
