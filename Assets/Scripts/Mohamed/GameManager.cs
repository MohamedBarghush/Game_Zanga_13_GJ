using UnityEngine;

public class Player {
    public int[] Attbs { get; set; }
    public Player(int[] attbs) {
        Attbs = attbs;
    }
}

public enum Attributes {
    Fame,
    Lust,
    Money,
    Intelligence,
    Charm
}

public class GameManager : MonoBehaviour
{
    public Player[] Players { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
