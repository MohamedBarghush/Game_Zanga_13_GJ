using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PassingPhoneUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject passingPhonePanel;
    public TMP_Text passText;
    public Button continueButton;

    private PassingPhonePhase phaseLogic;

    private void Awake()
    {
        phaseLogic = GetComponent<PassingPhonePhase>();
    }

    private void OnEnable()
    {
        if (phaseLogic == null) return;
        phaseLogic.OnShowPanel += ShowPanel;
        phaseLogic.OnHidePanel += HidePanel;
        phaseLogic.OnShowPassText += ShowPassText;
        phaseLogic.OnShowContinueButton += ShowContinueButton;
        phaseLogic.OnHideContinueButton += HideContinueButton;
    }

    private void OnDisable()
    {
        if (phaseLogic == null) return;
        phaseLogic.OnShowPanel -= ShowPanel;
        phaseLogic.OnHidePanel -= HidePanel;
        phaseLogic.OnShowPassText -= ShowPassText;
        phaseLogic.OnShowContinueButton -= ShowContinueButton;
        phaseLogic.OnHideContinueButton -= HideContinueButton;
    }

    private void ShowPanel()
    {
        if (passingPhonePanel != null) passingPhonePanel.SetActive(true);
    }

    private void HidePanel()
    {
        if (passingPhonePanel != null) passingPhonePanel.SetActive(false);
    }

    private void ShowPassText(int playerIndex)
    {
        if (passText != null) passText.text = $"Pass the phone to player {playerIndex + 1}";
    }

    private void ShowContinueButton()
    {
        if (continueButton == null) return;
        continueButton.gameObject.SetActive(true);
        var rect = continueButton.GetComponent<RectTransform>();
        if (rect != null)
        {
            Vector3 startPos = rect.anchoredPosition;
            rect.anchoredPosition = startPos + new Vector3(0, -100, 0);
            rect.localScale = Vector3.zero;
            rect.DOAnchorPos(startPos, 0.5f).SetEase(Ease.OutBack);
            rect.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            continueButton.transform.localScale = Vector3.zero;
            continueButton.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => phaseLogic.OnContinue());
    }

    private void HideContinueButton()
    {
        if (continueButton != null) continueButton.gameObject.SetActive(false);
    }
}
