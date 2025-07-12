using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class EventTriggeringUI : MonoBehaviour
{
    // Assign these in the inspector
    [Header("UI Elements")]
    public GameObject eventTriggeringPanel;
    public GameObject playerPopup;
    public TMP_Text playerPopupText;
    public GameObject cardPickSprite;
    public GameObject cardPanel;
    public TMP_Text cardNameText;
    public TMP_Text effect1Text;
    public TMP_Text effect2Text;
    public GameObject continueButton; // Reference to your button

    // Reference to the logic (auto-assign from same GameObject)
    private EventTriggeringPhase phaseLogic;
    private void Awake()
    {
        // Always get from same GameObject
        if (phaseLogic == null)
            phaseLogic = GetComponent<EventTriggeringPhase>();
        phaseLogic.OnEnterEvent += OnStart;

        phaseLogic.OnShowPlayerPopup += ShowPlayerPopup;
        phaseLogic.OnHidePlayerPopup += HidePlayerPopup;
        phaseLogic.OnShowCardPickSprite += ShowCardPickSprite;
        phaseLogic.OnHideCardPickSprite += HideCardPickSprite;
        phaseLogic.OnShowCard += ShowCard;
        phaseLogic.OnHideCard += HideCard;
        phaseLogic.OnAnimateAttributeChange += AnimateAttributeChangeHandler;
        phaseLogic.OnPhaseEnd += HideAll;
    }

    private void OnStart()
    {
        Debug.Log("EventTriggeringUI: Starting phase");
        eventTriggeringPanel.SetActive(true);

        playerPopup.SetActive(false);
        cardPickSprite.SetActive(false);
        cardPanel.SetActive(false);
        effect1Text.gameObject.SetActive(false);
        effect2Text.gameObject.SetActive(false);
    }

    private void OnExit()
    {
        if (phaseLogic == null) return;
        phaseLogic.OnShowPlayerPopup -= ShowPlayerPopup;
        phaseLogic.OnHidePlayerPopup -= HidePlayerPopup;
        phaseLogic.OnShowCardPickSprite -= ShowCardPickSprite;
        phaseLogic.OnHideCardPickSprite -= HideCardPickSprite;
        phaseLogic.OnShowCard -= ShowCard;
        phaseLogic.OnHideCard -= HideCard;
        phaseLogic.OnAnimateAttributeChange -= AnimateAttributeChangeHandler;
        phaseLogic.OnPhaseEnd -= HideAll;
    }

    // Helper to start coroutine from event
    private void AnimateAttributeChangeHandler(Attributes attb1, int old1, int new1, Attributes attb2, int old2, int new2)
    {
        StartCoroutine(AnimateAttributeChange(attb1, old1, new1, attb2, old2, new2));
    }

    // Show popup with player index
    public void ShowPlayerPopup(int playerIndex)
    {
        // if (playerPopup == null || playerPopupText == null) return;
        Debug.Log($"Showing player popup for player {playerIndex + 1}");
        playerPopup.SetActive(true);
        playerPopupText.text = $"Player {playerIndex + 1}'s Turn!";
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

    // Show card with effects
    public void ShowCard(EventTriggeringPhase.Card card)
    {
        if (cardPanel == null || cardNameText == null || effect1Text == null || effect2Text == null) return;
        cardPanel.SetActive(true);
        cardPanel.transform.localScale = Vector3.zero;
        cardPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        cardNameText.text = card.name;
        effect1Text.text = FormatEffectText(card.attb1, card.value1);
        effect2Text.text = FormatEffectText(card.attb2, card.value2);
    }

    private string FormatEffectText(Attributes attb, int value)
    {
        // Used for initial card display (no old value)
        string arrow = value > 0 ? "<color=green>▲</color>" : value < 0 ? "<color=red>▼</color>" : "";
        string sign = value > 0 ? "+" : value < 0 ? "-" : "";
        return $"{attb}: {sign}{Mathf.Abs(value)} {arrow}";
    }

    // Overload for animation to show old and new values
    private string FormatEffectText(Attributes attb, int newValue, int delta, int? oldValue = null)
    {
        string arrow = delta > 0 ? "<color=green>▲</color>" : delta < 0 ? "<color=red>▼</color>" : "";
        string sign = delta > 0 ? "+" : delta < 0 ? "-" : "";
        if (oldValue.HasValue)
        {
            // Example: Lust 10→15 +5 ▲
            return $"{attb} {oldValue.Value}→{newValue} {sign}{Mathf.Abs(newValue - oldValue.Value)} {arrow}";
        }
        else
        {
            return $"{attb}: {sign}{Mathf.Abs(newValue)} {arrow}";
        }
    }

    public void HideCard()
    {
        if (cardPanel == null) return;
        cardPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            cardPanel.SetActive(false);
        });
    }

    // Animate attribute changes
    public IEnumerator AnimateAttributeChange(Attributes attb1, int old1, int new1, Attributes attb2, int old2, int new2)
    {
        if (effect1Text == null || effect2Text == null) yield break;
        int delta1 = new1 - old1;
        int delta2 = new2 - old2;
        effect1Text.text = FormatEffectText(attb1, old1, delta1, old1);
        effect2Text.text = FormatEffectText(attb2, old2, delta2, old2);
        effect1Text.gameObject.SetActive(true);
        effect2Text.gameObject.SetActive(true);

        float duration = 1.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int val1 = Mathf.RoundToInt(Mathf.Lerp(old1, new1, t));
            int val2 = Mathf.RoundToInt(Mathf.Lerp(old2, new2, t));
            effect1Text.text = FormatEffectText(attb1, val1, delta1, old1);
            effect2Text.text = FormatEffectText(attb2, val2, delta2, old2);
            yield return null;
        }
        effect1Text.text = FormatEffectText(attb1, new1, delta1, old1);
        effect2Text.text = FormatEffectText(attb2, new2, delta2, old2);
        yield return new WaitForSeconds(1.0f);
        ShowContinueButton();
        // Optionally hide after animation
        // effect1Text.gameObject.SetActive(false);
        // effect2Text.gameObject.SetActive(false);


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
        if (cardPanel != null) cardPanel.SetActive(false);
        if (effect1Text != null) effect1Text.gameObject.SetActive(false);
        if (effect2Text != null) effect2Text.gameObject.SetActive(false);
        if (continueButton != null) continueButton.SetActive(false);
    }
}
