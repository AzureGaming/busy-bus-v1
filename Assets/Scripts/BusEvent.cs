using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BusEvent : MonoBehaviour {
    public LevelManager levelManager;

    Coroutine initRoutine;
    float nextTime;

    public void Init() {
        initRoutine = StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopCoroutine(initRoutine);
        OnEventStop();
    }

    IEnumerator EventRoutine() {
        for (; ; ) {
            OnEvent();
            yield return StartCoroutine(EventListener());
            OnEventComplete();
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
            //yield return StartCoroutine(StartTimer());
        }
    }

    protected virtual IEnumerator StartTimer() {
        // ...
        yield return null;
    }

    protected virtual void OnEvent() {
        // ...
    }

    protected virtual void OnEventComplete() {
        // ...
    }

    protected virtual IEnumerator EventListener() {
        // ...
        yield return null;
    }

    protected virtual void OnEventStop() {
        // ...
    }

    void LoadNextTime() {
        float initialValue = 0.1f;
        float rate = 0.1f;
        nextTime = initialValue * Mathf.Pow((1 + rate), levelManager.GetTimeElapsed());
        Debug.Log("Next Time: " + nextTime);
    }
}
