using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompts : MonoBehaviour {
    public LevelManager levelManager;
    public delegate void RushHourStart();
    public static RushHourStart OnRushHourStart;
    public delegate void RushHourEnd();
    public static RushHourEnd OnRushHourEnd;
    //public delegate void InitEvent();
    //public static InitEvent OnInitEvent;

    KeyCode expectedKey;
    KeyCode[] promptKeys;
    Coroutine eventRoutine;
    Coroutine timeoutRoutine;
    float nextTime;
    bool isRushHour;
    bool timesUp;

    private void Awake() {
        promptKeys = new KeyCode[4] { KeyCode.D, KeyCode.A, KeyCode.W, KeyCode.S };
    }

    private void OnEnable() {
        OnRushHourStart += StartRush;
        OnRushHourEnd += EndRush;
        //OnInitEvent += Init;
    }

    private void OnDisable() {
        OnRushHourStart -= StartRush;
        OnRushHourEnd -= EndRush;
        //OnInitEvent -= Init;
    }

    public void Init() {
        eventRoutine = StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopCoroutine(eventRoutine);
        OnEventStop();
    }

    IEnumerator EventRoutine() {
        for (; ; ) {
            OnEvent();
            timeoutRoutine = StartCoroutine(Timeout());
            yield return StartCoroutine(EventListener());
            OnEventComplete();
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
        }
    }

    void OnEvent() {
        int randomIndex = Random.Range(0, promptKeys.Length);
        expectedKey = promptKeys[randomIndex];
        DrivingPrompt.OnRender?.Invoke(expectedKey.ToString());
    }

    void OnEventComplete() {
        StopCoroutine(timeoutRoutine);
        DrivingPrompt.OnHide?.Invoke();
    }

    IEnumerator EventListener() {
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

    void OnEventStop() {
        OnEventComplete();
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(0.1f, 0.5f);
        } else {
            nextTime = Random.Range(3f, 5f);
        }
        Debug.Log("Next Time: " + nextTime);
    }

    void StartRush() {
        isRushHour = true;
    }

    void EndRush() {
        isRushHour = false;
    }
}
