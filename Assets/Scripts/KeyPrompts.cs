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
    Coroutine eventRoutine;
    Coroutine timeoutRoutine;
    float nextTime;
    bool timesUp;
    float timeLeft;
    float timeTotal;

    private void Awake() {
        keyCodes = new KeyCode[4] { KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.Space };
        keyCodeMap = new Dictionary<ActionName, KeyCode>();
        keyCodeMap.Add(ActionName.Right, keyCodes[0]);
        keyCodeMap.Add(ActionName.Left, keyCodes[1]);
        //keyCodeMap.Add(ActionName.Forward, keyCodes[2]);
        keyCodeMap.Add(ActionName.Stop, keyCodes[3]);
    }

    public void Init() {
        if (GameManager.IS_DEBUG) {
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
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
            Prompt();
            timeoutRoutine = StartCoroutine(Timeout());
            if (expectedKey == KeyCode.Space) {
                yield return StartCoroutine(ListenForBrake());
                Trees.OnStop?.Invoke();
                Buildings.OnStop?.Invoke();
                RoadLine.OnStop?.Invoke();
                Complete();
                yield return new WaitForSeconds(3f);
                BusStop.OnHide?.Invoke();
                Trees.OnContinue?.Invoke();
                Buildings.OnContinue?.Invoke();
                RoadLine.OnContinue?.Invoke();
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
        if (expectedKey == KeyCode.Space && CheckFare.isCheckingFare) {
            Prompt();
        } else {
            Debug.Log("Prompt" + actionName);
            DrivingPrompt.OnPrompt?.Invoke(actionName);
        }
    }

    void Prompt(int index) {
        expectedKey = keyCodeMap.ElementAt(index).Value;
        ActionName actionName = keyCodeMap.ElementAt(index).Key;
        Debug.Log("Expected Key: " + expectedKey);
        Debug.Log("Action Name: " + actionName);
        DrivingPrompt.OnPrompt?.Invoke(actionName);
    }

    void Complete() {
        if (expectedKey == KeyCode.Space) {
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
                Rate(timeLeft, timeTotal);
                return true;
            }
            if (input.Length > 0 || timesUp) {
                Fail();
                return true;
            }
            return false;
        });
    }

    IEnumerator ListenForBrake() {
        int counter = 0;
        int index = 0;
        while (index < 3) {
            if (timesUp) {
                Fail();
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
        Rate(timeLeft, timeTotal);
    }

    IEnumerator Timeout() {
        timesUp = false;
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
        timesUp = true;
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(1f, 2f);
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
                yield return StartCoroutine(ListenForBrake());
                Trees.OnStop?.Invoke();
                Buildings.OnStop?.Invoke();
                RoadLine.OnStop?.Invoke();
                Complete();
                yield return new WaitForSeconds(3f);
                Trees.OnContinue?.Invoke();
                Buildings.OnContinue?.Invoke();
                RoadLine.OnContinue?.Invoke();
            }
            yield return null;
        }
    }
}
