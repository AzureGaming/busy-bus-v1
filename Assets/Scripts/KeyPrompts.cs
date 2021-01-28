using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompts : BusEvent {
    KeyCode expectedKey;
    KeyCode[] promptKeys;
    Coroutine eventRoutine;
    Coroutine timeoutRoutine;
    float nextTime;
    bool timesUp;

    private void Awake() {
        promptKeys = new KeyCode[4] { KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.S };
    }

    public void Init() {
        eventRoutine = StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopCoroutine(eventRoutine);
        StopCoroutine(timeoutRoutine);
        DrivingPrompt.OnHide?.Invoke();
    }

    IEnumerator EventRoutine() {
        for (; ; ) {
            Prompt();
            timeoutRoutine = StartCoroutine(Timeout());
            yield return StartCoroutine(Listen());
            Complete();
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
        }
    }

    void Prompt() {
        int randomIndex = Random.Range(0, promptKeys.Length);
        expectedKey = promptKeys[randomIndex];
        DrivingPrompt.OnRender?.Invoke(expectedKey.ToString());
    }

    void Complete() {
        StopCoroutine(timeoutRoutine);
        DrivingPrompt.OnHide?.Invoke();
    }

    IEnumerator Listen() {
        yield return new WaitUntil(() => {
            string input = Input.inputString;
            if (input == expectedKey.ToString().ToLower()) {
                LevelManager.OnComplete?.Invoke();
                return true;
            }
            if (input.Length > 0 || timesUp) {
                LevelManager.OnMiss?.Invoke();
                return true;
            }
            return false;
        });
    }

    IEnumerator Timeout() {
        timesUp = false;
        if (isRushHour) {
            yield return new WaitForSeconds(0.5f);
        } else {
            yield return new WaitForSeconds(3f);
        }
        timesUp = true;
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(0.1f, 0.5f);
        } else {
            nextTime = Random.Range(3f, 5f);
        }
        Debug.Log("Next Key Prompt: " + nextTime);
    }
}
