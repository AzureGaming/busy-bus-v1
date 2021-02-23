using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompts : BusEvent {
    public enum ActionName {
        Right,
        Left,
        Forward,
        Stop
    }
    KeyCode expectedKey;
    KeyCode[] keyCodes;
    Dictionary<ActionName, KeyCode> keyCodeMap;
    Coroutine timeoutRoutine;
    float nextTime;
    bool eventFailed;
    float timeLeft;
    float timeTotal;
    string eventResponse;

    private void Awake() {
        keyCodes = new KeyCode[4] { KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.Space };
        keyCodeMap = new Dictionary<ActionName, KeyCode>();
        keyCodeMap.Add(ActionName.Right, keyCodes[0]);
        keyCodeMap.Add(ActionName.Left, keyCodes[1]);
        //keyCodeMap.Add(ActionName.Forward, keyCodes[2]);
        keyCodeMap.Add(ActionName.Stop, keyCodes[3]);
    }

    public void Init() {
        StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopAllCoroutines();
        DrivingPrompt.OnHide?.Invoke();
    }

    IEnumerator EventRoutine() {
        for (; ; ) {
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
            Prompt();
            if (expectedKey == KeyCode.Space) {
                yield return StartCoroutine(ListenForBrake());
                CompleteBrake();
            } else {
                yield return StartCoroutine(Listen());
                Complete();
            }
        }
    }

    void Prompt() {
        int randomIndex = Random.Range(0, keyCodeMap.Count);
        ActionName actionName = keyCodeMap.ElementAt(randomIndex).Key;
        expectedKey = keyCodeMap.ElementAt(randomIndex).Value;
        eventFailed = false;
        timeoutRoutine = StartCoroutine(Timeout());
        DrivingPrompt.OnPrompt?.Invoke(actionName, timeTotal);
    }

    void Complete() {
        if (timeoutRoutine != null) {
            StopCoroutine(timeoutRoutine);
        }

        if (eventFailed || eventResponse != expectedKey.ToString().ToLower()) {
            Fail();
        } else {
            Rate(timeLeft, timeTotal);
        }
        DrivingPrompt.OnHide?.Invoke();
    }

    void CompleteBrake() {
        if (timeoutRoutine != null) {
            StopCoroutine(timeoutRoutine);
        }

        if (eventFailed) {
            Fail();
        } else {
            Rate(timeLeft, timeTotal);
        }
        DrivingPrompt.OnHide?.Invoke();
    }

    IEnumerator Listen() {
        yield return new WaitUntil(() => {
            eventResponse = Input.inputString;
            return eventResponse.Length > 0 || eventFailed;
        });
    }

    IEnumerator ListenForBrake() {
        int counter = 0;
        int index = 0;
        while (index < 3) {
            if (eventFailed) {
                yield break;
            }
            if (counter == 3) {
                counter = 0;
                index++;
                DrivingPrompt.OnPromptBrake?.Invoke(index);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                counter++;
            }
            yield return null;
        }
    }

    IEnumerator Timeout() {
        if (expectedKey == KeyCode.Space) {
            timeTotal = 5f;
        } else {
            if (isRushHour) {
                timeTotal = 0.75f;
            } else {
                timeTotal = 3f;
            }
        }
        timeLeft = timeTotal;

        while (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        eventFailed = true;
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(1f, 2f);
        } else {
            nextTime = Random.Range(3f, 5f);
        }
        Debug.Log("Next Key Prompt: " + nextTime);
    }
}
