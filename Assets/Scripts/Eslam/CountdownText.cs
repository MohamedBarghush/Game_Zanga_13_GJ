using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using TMPro;

public class CountdownText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [SerializeField] GameObject canva;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        canva.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        int countdownValue = 3; 
        if (int.TryParse(textComponent.text.Trim(), out int parsedValue))
        {
            countdownValue = parsedValue;
        }

        while (countdownValue > 0)
        {
            textComponent.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f); 
            countdownValue--;
        }

        textComponent.text = "Starting now";

        yield return new WaitForSeconds(1f);

        if (transform.parent != null)
        {
            canva.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
