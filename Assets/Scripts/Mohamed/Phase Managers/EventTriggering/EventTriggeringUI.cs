using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class EventTriggeringUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject eventTriggeringPanel;
    public GameObject playerPopup;
    public GameObject cardPickSprite;
    public GameObject effectPanel;
    public TMP_Text effectDescriptionText;
    public TMP_Text effectPlayersText;
    public GameObject continueButton;

    private EventTriggeringPhase phaseLogic;
    private void Awake()
    {
        if (phaseLogic == null)
            phaseLogic = GetComponent<EventTriggeringPhase>();
        phaseLogic.OnEnterEvent += OnStart;
        phaseLogic.OnShowPlayerPopup += ShowPlayerPopup;
        phaseLogic.OnHidePlayerPopup += HidePlayerPopup;
        phaseLogic.OnShowCardPickSprite += ShowCardPickSprite;
        phaseLogic.OnHideCardPickSprite += HideCardPickSprite;
        phaseLogic.OnShowEffect += ShowEffect;
        phaseLogic.OnHideCard += HideEffectPanel;
        phaseLogic.OnAnimateAttributeChange += AnimateAttributeChangeAllPlayers;
        phaseLogic.OnPhaseEnd += HideAll;
    }

    private void OnStart()
    {
        Debug.Log("EventTriggeringUI: Starting phase");
        eventTriggeringPanel.SetActive(true);
        playerPopup.SetActive(false);
        cardPickSprite.SetActive(false);
        if (effectPanel != null) effectPanel.SetActive(false);
        if (effectDescriptionText != null) effectDescriptionText.text = "";
        if (effectPlayersText != null) effectPlayersText.text = "";
    }

    private void OnExit()
    {
        if (phaseLogic == null) return;
        phaseLogic.OnShowPlayerPopup -= ShowPlayerPopup;
        phaseLogic.OnHidePlayerPopup -= HidePlayerPopup;
        phaseLogic.OnShowCardPickSprite -= ShowCardPickSprite;
        phaseLogic.OnHideCardPickSprite -= HideCardPickSprite;
        phaseLogic.OnShowEffect -= ShowEffect;
        phaseLogic.OnHideCard -= HideEffectPanel;
        phaseLogic.OnAnimateAttributeChange -= AnimateAttributeChangeAllPlayers;
        phaseLogic.OnPhaseEnd -= HideAll;
    }

    // Show the effect description panel
    private void ShowEffect(string effectDesc)
    {
        if (effectPanel != null) effectPanel.SetActive(true);
        if (effectDescriptionText != null) effectDescriptionText.text = effectDesc;
        if (effectPlayersText != null) effectPlayersText.text = "";
    }

    private void HideEffectPanel()
    {
        if (effectPanel != null) effectPanel.SetActive(false);
        if (effectDescriptionText != null) effectDescriptionText.text = "";
        if (effectPlayersText != null) effectPlayersText.text = "";
    }

    // Show popup with player index
    public void ShowPlayerPopup(int playerIndex)
    {
        // if (playerPopup == null || playerPopupText == null) return;
        Debug.Log($"Showing Special Effect Round");
        playerPopup.SetActive(true);
        playerPopup.transform.localScale = Vector3.zero;
        playerPopup.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    public void HidePlayerPopup()
    {
        if (playerPopup == null) return;
        playerPopup.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            playerPopup.SetActive(false);
        });
    }

    // Show animated sprite for card picking
    public void ShowCardPickSprite()
    {
        if (cardPickSprite == null) return;
        cardPickSprite.SetActive(true);
        cardPickSprite.transform.localScale = Vector3.zero;
        cardPickSprite.transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic);
    }

    public void HideCardPickSprite()
    {
        if (cardPickSprite == null) return;
        cardPickSprite.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            cardPickSprite.SetActive(false);
        });
    }

    // Animate attribute changes for all players
    private void AnimateAttributeChangeAllPlayers(Attributes attr, int[] oldVals, int[] newVals)
    {
        StartCoroutine(AnimateAllPlayersCoroutine(attr, oldVals, newVals));
    }

    private IEnumerator AnimateAllPlayersCoroutine(Attributes attr, int[] oldVals, int[] newVals)
    {
        if (effectPlayersText == null) yield break;
        float duration = 1.5f;
        float elapsed = 0f;
        int playerCount = oldVals.Length;
        string[] playerLines = new string[playerCount];
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            for (int i = 0; i < playerCount; i++)
            {
                int val = Mathf.RoundToInt(Mathf.Lerp(oldVals[i], newVals[i], t));
                playerLines[i] = $"Player {i + 1}: {attr} {oldVals[i]}→{val}";
            }
            effectPlayersText.text = string.Join("\n", playerLines);
            yield return null;
        }
        for (int i = 0; i < playerCount; i++)
        {
            playerLines[i] = $"Player {i + 1}: {attr} {oldVals[i]}→{newVals[i]}";
        }
        effectPlayersText.text = string.Join("\n", playerLines);
        yield return new WaitForSeconds(1.0f);
        ShowContinueButton();
    }

    // Hide all UI elements
    public void HideAll()
    {
        Invoke(nameof(HideAllCoroutine), 0.5f);
        // OnExit();
    }

    private void ShowContinueButton()
    {
        if (continueButton == null) return;
        continueButton.SetActive(true);
        var canvasGroup = continueButton.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = continueButton.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);

        var rect = continueButton.GetComponent<RectTransform>();
        if (rect != null)
        {
            Vector3 startPos = rect.anchoredPosition;
            rect.anchoredPosition = startPos + new Vector3(0, -100, 0); // Start a bit down
            rect.localScale = Vector3.zero;
            rect.DOAnchorPos(startPos, 0.5f).SetEase(Ease.OutBack);
            rect.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            // fallback for non-UI button
            continueButton.transform.localScale = Vector3.zero;
            continueButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void HideAllCoroutine()
    {
        if (eventTriggeringPanel != null) eventTriggeringPanel.SetActive(false);
        if (playerPopup != null) playerPopup.SetActive(false);
        if (cardPickSprite != null) cardPickSprite.SetActive(false);
        // Removed references to old card/effect fields (cardPanel, effect1Text, effect2Text)
        if (continueButton != null) continueButton.SetActive(false);
    }
}
