using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BusEvent : MonoBehaviour {
    Coroutine initRoutine;

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
            yield return StartCoroutine(StartTimer());
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
}
