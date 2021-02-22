using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFare : BusEvent {
    public delegate void QueueCommuters();
    public static QueueCommuters OnQueueCommuters;
    public delegate void UpdateFare(float fare);
    public static UpdateFare OnUpdateFare;

    public GameObject fareSpawn;
    public GameObject civilianPrefab;
    public Button acceptButton;
    public Button rejectButton;

    Coroutine timeoutRoutine;
    Coroutine eventRoutine;
    float fare;
    float farePaid;
    bool hasResponded;
    bool timesUp;
    float nextTime;
    bool answer;
    float timeLeft;
    float timeTotal;

    private void Start() {
        acceptButton.onClick.AddListener(OnClick);
        rejectButton.onClick.AddListener(OnClick);
    }

    private void OnEnable() {
        OnQueueCommuters += Init;
        OnUpdateFare += SetFare;
    }

    private void OnDisable() {
        OnQueueCommuters -= Init;
        OnUpdateFare -= SetFare;
    }

    public void Init() {
        if (GameManager.IS_DEBUG) {
            StartCoroutine(DebugRoutine());
        } else {
            eventRoutine = StartCoroutine(EventRoutine());
        }
    }

    public void Stop() {
        StopCoroutine(eventRoutine);
        StopCoroutine(timeoutRoutine);
        CoinSpawn.OnClearSpawn?.Invoke();
    }

    public void Accept() {
        answer = true;
    }

    public void Reject() {
        answer = false;
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

    IEnumerator DebugRoutine() {
        Prompt();
        yield return StartCoroutine(Listen());
        Complete();
    }

    void Prompt() {
        hasResponded = false;
        CalculateFarePaid();
        FareWindow.OnOpen?.Invoke(false);
        Passenger.OnEnterBus?.Invoke();
        BusStop.OnShow?.Invoke();
    }

    IEnumerator Timeout() {
        timesUp = false;
        if (isRushHour) {
            timeTotal = 5f;
        } else {
            timeTotal = 7f;
        }
        timeLeft = timeTotal;

        while (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        timesUp = true;

        CoinSpawn.OnClearSpawn?.Invoke();
        FareWindow.OnClose?.Invoke(false);
        Passenger.OnLeaveBus?.Invoke();
        BusStop.OnHide?.Invoke();
    }

    void Complete() {
        if (timeoutRoutine != null) {
            StopCoroutine(timeoutRoutine);
        }
        Debug.Log("Answer: " + answer);
        if (answer) {
            if (farePaid >= fare) {
                LevelManager.OnComplete?.Invoke();
                Passenger.OnStayBus?.Invoke();
            } else {
                LevelManager.OnMiss?.Invoke();
            }
        } else if (!answer) {
            if (farePaid >= fare) {
                LevelManager.OnMiss?.Invoke();
            } else {
                LevelManager.OnComplete?.Invoke();
            }
            Passenger.OnLeaveBus?.Invoke();
            BusStop.OnHide?.Invoke();
        }
        base.Rate(timeLeft, timeTotal);
        FareWindow.OnClose?.Invoke(false);
        CoinSpawn.OnClearSpawn?.Invoke();
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
        int numOfNickels;
        int numOfQuarters;
        int numOfDimes;

        if (numOfToonies == 1 && numOfLoonies == 1) {
            numOfQuarters = Random.Range(0, 3);
            numOfNickels = Random.Range(0, 2);
            numOfDimes = 0;
        } else if (numOfToonies == 1 && numOfLoonies == 0) {
            numOfQuarters = Random.Range(0, 3);
            numOfNickels = Random.Range(0, 2);
            numOfDimes = Random.Range(0, 2);
        } else if (numOfToonies == 0 && numOfLoonies == 1) {
            numOfQuarters = Random.Range(0, 2);
            numOfNickels = Random.Range(0, 1);
            numOfDimes = Random.Range(0, 1);
        } else {
            numOfQuarters = Random.Range(0, 3);
            numOfNickels = Random.Range(0, 1);
            numOfDimes = Random.Range(0, 1);
        }

        CoinSpawn.OnGetCoinsAmount?.Invoke(numOfToonies, numOfLoonies, numOfQuarters, numOfDimes, numOfNickels);
        farePaid = (float)( ( numOfToonies * 2 ) + ( numOfLoonies * 1 ) + ( numOfQuarters * 0.25 ) + ( numOfDimes * 0.1 ) + ( numOfNickels * 0.05 ) );
    }

    void SetFare(float value) {
        fare = value;
    }
}
