using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFare : BusEvent {
    public delegate void UpdateFare(float fare);
    public static UpdateFare OnUpdateFare;

    public GameObject fareSpawn;
    public GameObject civilianPrefab;
    public Button acceptButton;
    public Button rejectButton;

    Coroutine timeoutRoutine;
    float fare;
    float farePaid;
    float timeLeft;
    float timeTotal;
    float nextTime;
    bool answer;
    bool hasResponded;
    bool failedEvent;

    private void OnEnable() {
        OnUpdateFare += SetFare;
    }

    private void OnDisable() {
        OnUpdateFare -= SetFare;
    }

    public void Init() {
        StartCoroutine(EventRoutine());
    }

    public void Stop() {
        StopAllCoroutines();
        CoinSpawn.OnClearSpawn?.Invoke();
    }

    public void Accept() {
        hasResponded = true;
        answer = true;
    }

    public void Reject() {
        hasResponded = true;
        answer = false;
    }

    IEnumerator EventRoutine() {
        for (; ; ) {
            LoadNextTime();
            yield return new WaitForSeconds(nextTime);
            Prompt();
            timeoutRoutine = StartCoroutine(Timeout());
            yield return StartCoroutine("Listen");
            Complete();
        }
    }

    void Prompt() {
        hasResponded = false;
        failedEvent = false;
        CalculateFarePaid();
        FareWindow.OnOpen?.Invoke(false);
        Passenger.OnEnterBus?.Invoke();
        BusStop.OnShow?.Invoke();
    }

    IEnumerator Timeout() {
        timeTotal = isRushHour ? 5f : 7f;
        timeLeft = timeTotal;

        while (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        failedEvent = true;

        CoinSpawn.OnClearSpawn?.Invoke();
        FareWindow.OnClose?.Invoke(false);
        Passenger.OnLeaveBus?.Invoke();
        BusStop.OnHide?.Invoke();
    }

    void Complete() {
        if (timeoutRoutine != null) {
            StopCoroutine(timeoutRoutine);
        }

        if (failedEvent) {
            Fail();
        } else {
            if (answer) {
                if (farePaid >= fare) {
                    Rate(timeLeft, timeTotal);
                } else {
                    Fail();
                }
            } else if (!answer) {
                if (farePaid >= fare) {
                    Fail();
                } else {
                    Rate(timeLeft, timeTotal);
                }
            }
        }

        Passenger.OnLeaveBus?.Invoke();
        BusStop.OnHide?.Invoke();
        FareWindow.OnClose?.Invoke(false);
        CoinSpawn.OnClearSpawn?.Invoke();
    }

    IEnumerator Listen() {
        yield return new WaitUntil(() => hasResponded || failedEvent);
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

    void LoadNextTime() {
        if (isRushHour) {
            nextTime = Random.Range(1f, 2f);
        } else {
            nextTime = Random.Range(3f, 5f);
        }
        Debug.Log("Next Check Fare: " + nextTime);
    }
}
