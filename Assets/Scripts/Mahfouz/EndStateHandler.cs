using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndStateHandler : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Button nextStep;
    [SerializeField] private Button endGame;
    [SerializeField] private TMP_Text winnersText;

    private void Start()
    {
        gameManager = GameManager.Instance;
        List<PlayerData> winners = new List<PlayerData>();

        foreach (PlayerData playerData in gameManager.Players)
        {
            var currAttbs = playerData.currentAttbs;
            var reqAttbs = playerData.requiredAttbs;
            var sinkAttbs = playerData.sinkingAttbs;

            bool won = true;

            foreach (var attribute in currAttbs.Keys)
            {
                int currVal = currAttbs[attribute];
                int reqVal = reqAttbs[attribute];
                int sinkVal = sinkAttbs[attribute];

                if (reqVal == -1 || sinkVal == -1)
                    continue;

                if (sinkVal >= currVal)
                {
                    won = false;
                    break;
                }

                if (currVal < reqVal)
                {
                    won = false;
                    break;
                }
            }

            if (won)
            {
                winners.Add(playerData);
            }
        }
        if (!winners.Any())
        {
            nextStep.gameObject.SetActive(true);
            endGame.gameObject.SetActive(false);
            winnersText.color = Color.red;
            winnersText.text = "No one was capable of achieving of their tasks :(((";
        }
        else
        {

            nextStep.gameObject.SetActive(false);
            endGame.gameObject.SetActive(true);
            winnersText.color = Color.green;
            winnersText.text = ("Winners: " + string.Join(", ", winners.Select(w => w.playerName)) + "!!!!!!!!");
        }


       
    }

    public void onContinuePress()
    {
        GameStateManager.Instance.SwitchState(StateID.Bargaining);
    }

    public void onEndGamePress()
    {
        GameStateManager.Instance.SwitchState(StateID.Start);
    }
}
