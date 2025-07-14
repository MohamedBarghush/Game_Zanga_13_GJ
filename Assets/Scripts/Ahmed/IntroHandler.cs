using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    [SerializeField] private GameObject rulesMenu;
    [SerializeField] private GameObject thisMenu;
    [SerializeField] private GameObject entireCanvas;


    private void EnableEverything()
    {
        entireCanvas.SetActive(true);
    }

    private void DisableEverything()
    {
        entireCanvas.SetActive(false);
    }
    public void OnPhaseEnter()
    {
        EnableEverything();
    }

    public void onStartButtonPress()
    {
        DisableEverything();
        GameStateManager.Instance.UpdateLastState(StateID.EnterName);
        GameStateManager.Instance.SwitchState(StateID.PassPhone);
    }
    public void onRulesButtonPress()
    {
        rulesMenu.SetActive(true);
        thisMenu.SetActive(false);
    }
    public void rulesToMain()
    {
        thisMenu.SetActive(true);
        rulesMenu.SetActive(false);
    }
    public void onQuitButtonPress()
    {
        Application.Quit();
    }
}
