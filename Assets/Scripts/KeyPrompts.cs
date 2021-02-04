using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompts : BusEvent {
    public bool isDebug;

    public enum ActionName {
        Right,
        Left,
        Forward,
        Stop
    }
    KeyCode expectedKey;
    KeyCode[] keyCodes;
    Dictionary<ActionName, KeyCode> keyCodeMap;
    Coroutine eventRoutine;
    Coroutine timeoutRoutine;
    float nextTime;
    bool timesUp;

    private void Awake() {
        keyCodes = new KeyCode[4] { KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.S };
        keyCodeMap = new Dictionary<ActionName, KeyCode>();
        keyCodeMap.Add(ActionName.Right, keyCodes[0]);
        keyCodeMap.Add(ActionName.Left, keyCodes[1]);
        keyCodeMap.Add(ActionName.Forward, keyCodes[2]);
        keyCodeMap.Add(ActionName.Stop, keyCodes[3]);
    }

    public void Init() {
        if (isDebug) {
            eventRoutine = StartCoroutine(DebugRoutine());
        } else {
            eventRoutine = StartCoroutine(EventRoutine());
        }
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
        int randomIndex = Random.Range(0, keyCodeMap.Count);
        ActionName actionName = keyCodeMap.ElementAt(randomIndex).Key;
        DrivingPrompt.OnPrompt?.Invoke(actionName);
    }

    void Prompt(int index) {
        expectedKey = keyCodeMap.ElementAt(index).Value;
        ActionName actionName = keyCodeMap.ElementAt(index).Key;
        Debug.Log("Expected Key: " + expectedKey);
        Debug.Log("Action Name: " + actionName);
        DrivingPrompt.OnPrompt?.Invoke(actionName);
    }

    void Complete() {
        if (expectedKey == KeyCode.S) {
            CheckFare.OnQueueCommuters?.Invoke();
        }

        if (timeoutRoutine != null) {
            StopCoroutine(timeoutRoutine);
        }

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

    IEnumerator DebugRoutine() {
        for (; ; ) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Prompt(0);
                yield return null;
                yield return StartCoroutine(Listen());
                Complete();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Prompt(1);
                yield return null;
                yield return StartCoroutine(Listen());
                Complete();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Prompt(2);
                yield return null;
                yield return StartCoroutine(Listen());
                Complete();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                Prompt(3);
                yield return null;
                yield return StartCoroutine(Listen());
                Complete();
            }
            yield return null;
        }
    }
}
