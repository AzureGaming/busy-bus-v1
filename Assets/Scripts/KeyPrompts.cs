using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompts : BusEvent {
    KeyCode expectedKey;
    KeyCode[] promptKeys;
    Coroutine waitRoutine;
    bool stopListening;

    private void Awake() {
        promptKeys = new KeyCode[2] { KeyCode.D, KeyCode.A };
    }

    protected override void OnEvent() {
        int randomIndex = Random.Range(0, promptKeys.Length);
        expectedKey = promptKeys[randomIndex];
        DrivingPrompt.OnRender?.Invoke(expectedKey.ToString());
        stopListening = false;
        waitRoutine = StartCoroutine(Wait());
    }

    protected override void OnEventComplete() {
        StopCoroutine(waitRoutine);
        DrivingPrompt.OnHide?.Invoke();
    }

    protected override IEnumerator EventListener() {
        yield return new WaitUntil(() => {
            string input = Input.inputString;
            if (input == expectedKey.ToString().ToLower()) {
                LevelManager.OnComplete?.Invoke();
                return true;
            }
            if (input.Length > 0 || stopListening) {
                LevelManager.OnMiss?.Invoke();
                return true;
            }
            return false;
        });
    }

    protected override IEnumerator StartTimer() {
        yield return new WaitForSeconds(1f);
    }

    protected override void OnEventStop() {
        OnEventComplete();
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(10f);
        stopListening = true;
    }
}
