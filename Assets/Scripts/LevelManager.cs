using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public delegate void Miss();
    public static Miss OnMiss;
    public delegate void Complete();
    public static Complete OnComplete;
    public delegate void HourChange(int time);
    public static HourChange OnHourChange;

    public KeyPrompts keyPrompts;
    public CheckFare checkFare;

    readonly int MAX_RATING = 5;
    readonly int START_HOUR = 6;
    readonly int HOURS_IN_DAY = 12; // 06:00 - 18:00
    readonly int DAY_IN_REAL_MINUTES = 4;

    int misses;
    int scoreToday;
    float targetTime;
    float timeElapsed;
    int currentHour;
    float gameHoursPerSecond;
    bool isRushHour;
    int commuterQueue;
    // 0: 6am - 9am
    // 1: 9am - 12pm
    // 2: 12pm - 3pm
    // 3: 3pm - 6pm
    List<float> fareRates;

    private void OnEnable() {
        OnMiss += FailEvent;
        OnComplete += CompleteEvent;
    }

    private void OnDisable() {
        OnMiss -= FailEvent;
        OnComplete -= CompleteEvent;
    }

    private void Start() {
        LoadDay();
    }

    public float GetTimeElapsed() {
        return timeElapsed;
    }

    public void LoadDay() {
        misses = 0;
        scoreToday = 0;
        isRushHour = false;
        commuterQueue = 0;
        InitTimer(DAY_IN_REAL_MINUTES, HOURS_IN_DAY);
        LoadFareRates();
        StartCoroutine(StartDay());
        StartCoroutine(RushHour());
        StartCoroutine(UpdateHour());
        keyPrompts.Init();
        DrivingPrompt.OnHide?.Invoke();
        GameManager.OnShowBusOverlay?.Invoke();
        ScoreRating.OnUpdateScore?.Invoke(scoreToday);
    }

    void LoseDay() {
        keyPrompts.Stop();
        checkFare.Stop();
        ScoreRating.OnUpdateScore?.Invoke(scoreToday);
        GameManager.OnShowLoseScreen?.Invoke();
    }

    void CompleteDay() {
        keyPrompts.Stop();
        checkFare.Stop();
        GameManager.OnShowResults?.Invoke();
    }

    void FailEvent() {
        //if (scoreToday > 0) {
        //    scoreToday--;
        //    ScoreRating.OnUpdateScore?.Invoke(scoreToday);
        //} else {
        //    LoseDay();
        //}
        //Misses.OnUpdate?.Invoke(misses);
    }

    void CompleteEvent() {
        if (scoreToday < MAX_RATING) {
            scoreToday++;
            ScoreRating.OnUpdateScore?.Invoke(scoreToday);
        }
    }

    IEnumerator StartDay() {
        while (timeElapsed <= targetTime) {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CompleteDay();
    }

    void InitTimer(float totalRealMinutes, int totalGameHours) {
        targetTime = 60 * totalRealMinutes; // convert to seconds
        timeElapsed = 0;
        currentHour = START_HOUR;
        gameHoursPerSecond = targetTime / totalGameHours;
        if (GameManager.IS_DEBUG) {
            Debug.Log("Init Timer");
            Debug.Log("Target time: " + targetTime + " Game hours per second: " + gameHoursPerSecond);
        }
    }

    IEnumerator UpdateHour() {
        while (timeElapsed <= targetTime) {
            OnHourChange?.Invoke(currentHour);
            currentHour++;
            yield return new WaitForSeconds(gameHoursPerSecond);
        }
    }

    IEnumerator RushHour() {
        while (timeElapsed <= targetTime) {
            if (currentHour == 9) {
                BusEvent.OnRushHourStart?.Invoke();
                yield return new WaitUntil(() => currentHour == 11);
                BusEvent.OnRushHourEnd?.Invoke();
            }

            if (currentHour == 16) {
                BusEvent.OnRushHourStart?.Invoke();
                yield return new WaitUntil(() => currentHour == 18);
                BusEvent.OnRushHourEnd?.Invoke();
            }
            yield return null;
        }
    }

    void LoadFareRates() {
        fareRates = new List<float>();
        for (int i = 0; i < 4; i++) {
            float fare = Random.Range(1f, 4f);
            fare -= (float)(fare % 0.01); // 2 decimal places
            Debug.Log("Original Fare: " + fare);
            fare = Mathf.Round(fare * 10f) / 10f;
            Debug.Log("Rounded Fare: " + fare);
            fareRates.Add(fare);
        }
        FarePoster.OnUpdateFare?.Invoke(fareRates[0], fareRates[1], fareRates[2], fareRates[3]);
    }
}
