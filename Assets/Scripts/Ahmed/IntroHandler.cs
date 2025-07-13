using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private GameObject rulesMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject thisMenu;
    [SerializeField] private GameObject entireCanvas;
    [SerializeField] private TMP_Text publicNameTitle;
    [SerializeField] private TMP_Text privateNameTitle;
    [SerializeField] private TMP_InputField publicName;
    [SerializeField] private TMP_InputField privateName;
    public static List<Tuple<string, string>> names = new List<Tuple<string, string>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

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

    public void OnNextPlayerPress()
    {
        if (publicName.text == "")
        {
            publicNameTitle.text = "Please Enter your public Name";
            publicNameTitle.color = Color.red;
            return;
        }

        if (privateName.text == "")
        {
            privateNameTitle.text = "Please Enter your private Name";
            privateNameTitle.color = Color.red;
            return;
        }
        names.Add(new Tuple<string, string>(publicName.text, privateName.text));

        GameStateManager.Instance.UpdateLastState(StateID.Bargaining);
        GameStateManager.Instance.NextPlayer();
        DisableEverything();
    }


    public void onStartButtonPress()
    {
        gameMenu.SetActive(true);
        thisMenu.SetActive(false);
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
