using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public enum QTEInputMode
{
    Keyboard, 
    Controller
}

public class GraffitiQTE : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private Slider timerSlider;

    [Header("Settings")]
    [SerializeField] private int requiredInputs = 8;
    [SerializeField] private float timeLimit = 3f;

    private int currentInputCount;
    private bool qteActive;
    private QTEInputMode currentMode;

    private Action onSuccess;
    private Action onFail;

    private Coroutine qteRoutine;

    private void Start()
    {
        qtePanel.SetActive(false);
    }

    public void StartQTE(QTEInputMode mode, Action successCallback, Action failCallback)
    {
        if (qteActive) return;

        currentMode = mode;
        onSuccess = successCallback;
        onFail = failCallback;

        currentInputCount = 0;
        qteActive = true;

        qtePanel.SetActive(true);
        UpdatePrompt();

        if (qteRoutine != null)
        {
            StopCoroutine(qteRoutine);
        }

        qteRoutine = StartCoroutine(QTETimer());
    }

    private IEnumerator QTETimer()
    {
        float timer = timeLimit;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timerSlider != null)
            {
                timerSlider.value = timer / timeLimit;
            }

            yield return null;
        }

        FailQTE();
    }

    public void OnQTEKeyboard(InputValue value)
    {
        if (!qteActive) return;
        if (currentMode != QTEInputMode.Keyboard) return;
        if (!value.isPressed) return;

        RegisterInput();
    }

    public void OnQTEController(InputValue value)
    {
        if (!qteActive) return;
        if (currentMode != QTEInputMode.Controller) return;
        if (!value.isPressed) return;

        RegisterInput();
    }

    private void RegisterInput()
    {
        currentInputCount++;
        UpdatePrompt();

        if (currentInputCount >= requiredInputs)
        {
            CompleteQTE();
        }
    }

    private void UpdatePrompt()
    {
        string inputName = currentMode == QTEInputMode.Keyboard ? "E" : "South Button / A";
        promptText.text = $"Mash {inputName}! {currentInputCount}/{requiredInputs}";
    }

    private void CompleteQTE()
    {
        qteActive = false;

        if (qteRoutine != null)
        {
            StopCoroutine(qteRoutine);
        }

        qtePanel.SetActive(false);
        onSuccess?.Invoke();
    }

    private void FailQTE()
    {
        qteActive = false;

        if (qteRoutine != null)
        {
            StopCoroutine(qteRoutine);
        }

        qtePanel.SetActive(false);
        onFail?.Invoke();
    }
}
