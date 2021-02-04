using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFare : BusEvent {
    public delegate void QueueCommuters();
    public static QueueCommuters OnQueueCommuters;

    public GameObject fareSpawn;
    public Button acceptButton;
    public Button rejectButton;

    Coroutine timeoutRoutine;
    Coroutine eventRoutine;
    float postedFare = 5;
    float farePaid;
    bool hasResponded;
    bool timesUp;
    float nextTime;

    private void Start() {
        acceptButton.onClick.AddListener(OnClick);
        rejectButton.onClick.AddListener(OnClick);
    }

    private void OnEnable() {
        OnQueueCommuters += Init;
    }

    private void OnDisable() {
        OnQueueCommuters -= Init;
    }

    public void Init() {
        eventRoutine = StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopCoroutine(eventRoutine);
        StopCoroutine(timeoutRoutine);
        CoinSpawn.OnClearSpawn?.Invoke();
    }

    public void Accept() {
        if (farePaid >= postedFare) {
            LevelManager.OnComplete?.Invoke();
        } else if (farePaid < postedFare) {
            LevelManager.OnMiss?.Invoke();
        }
        FareWindow.OnClose?.Invoke(false);
        CoinSpawn.OnClearSpawn?.Invoke();
    }

    public void Reject() {
        if ((farePaid >= postedFare) || (farePaid < postedFare)) {
            LevelManager.OnMiss?.Invoke();
        } else if ((farePaid < postedFare) || (farePaid >= postedFare)) {
            LevelManager.OnComplete?.Invoke();
        }
        FareWindow.OnClose?.Invoke(false);
        CoinSpawn.OnClearSpawn?.Invoke();
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
        CalculateFarePaid();
        FareWindow.OnOpen?.Invoke(false);
    }

    IEnumerator Timeout() {
        timesUp = false;
        if (isRushHour) {
            yield return new WaitForSeconds(5f);
        } else {
            yield return new WaitForSeconds(7f);
        }
        CoinSpawn.OnClearSpawn?.Invoke();
        FareWindow.OnClose?.Invoke(false);
        timesUp = true;
    }

    void Complete() {
        StopCoroutine(timeoutRoutine);
        BusOverlay.OnHideFare?.Invoke();
    }

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(4f, 5f);
        } else {
            nextTime = Random.Range(7f, 8f);
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

    void CalculateFarePaid() {
        int numOfToonies = Random.Range(0, 2);
        int numOfLoonies = Random.Range(0, 2);
        int numOfQuarters = Random.Range(0, 3);
        int numOfDimes = Random.Range(0, 2);
        int numOfNickels = Random.Range(0, 2);

        CoinSpawn.OnGetCoinsAmount?.Invoke(numOfToonies, numOfLoonies, numOfQuarters, numOfDimes, numOfNickels);
        farePaid = (float)(numOfToonies * 2 + numOfLoonies * 1 + numOfQuarters * 0.25 + numOfDimes * 0.1 * numOfNickels * 0.05);
    }
}
