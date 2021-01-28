using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFare : BusEvent {

    public GameObject coinPrefab;
    public GameObject fareSpawn;
    public Button acceptButton;
    public Button rejectButton;

    Coroutine timeoutRoutine;
    Coroutine eventRoutine;
    int postedFare = 5;
    int farePaid;
    bool hasResponded;
    bool timesUp;
    float nextTime;

    private void Start() {
        acceptButton.onClick.AddListener(OnClick);
        rejectButton.onClick.AddListener(OnClick);
    }

    public void Init() {
        eventRoutine = StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopCoroutine(eventRoutine);
        StopCoroutine(timeoutRoutine);
        ClearFare();
    }

    public void Accept() {
        if (farePaid >= postedFare) {
            LevelManager.OnComplete?.Invoke();
        } else if (farePaid < postedFare) {
            LevelManager.OnMiss?.Invoke();
        }
        ClearFare();
    }

    public void Reject() {
        if ((farePaid >= postedFare) || (farePaid < postedFare)) {
            LevelManager.OnMiss?.Invoke();
        } else if ((farePaid < postedFare) || (farePaid >= postedFare)) {
            LevelManager.OnComplete?.Invoke();
        }
        ClearFare();
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
        BusOverlay.OnShowFare?.Invoke();
        hasResponded = false;
        LoadFare();
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

    void Complete() {
        StopCoroutine(timeoutRoutine);
        BusOverlay.OnHideFare?.Invoke();
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(0.1f, 0.5f);
        } else {
            nextTime = Random.Range(3f, 5f);
        }
        Debug.Log("Next Check Fare: " + nextTime);
    }

    IEnumerator Listen() {
        yield return new WaitUntil(() => {
            if (hasResponded) {
                return true;
            } else if (timesUp) {
                LevelManager.OnMiss?.Invoke();
                return true;
            }
            return false;
        });
    }

    void OnClick() {
        hasResponded = true;
    }

    // todo: move to fare spawner
    void LoadFare() {
        farePaid = Random.Range(1, 7);
        for (int i = 1; i <= farePaid; i++) {
            GameObject coin = Instantiate(coinPrefab, fareSpawn.transform);
        }
    }

    void ClearFare() {
        foreach (Transform child in fareSpawn.transform) {
            Destroy(child.gameObject);
        }
    }
}
