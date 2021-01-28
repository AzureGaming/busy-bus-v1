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

    readonly int MAX_MISSES = 5;
    readonly int MAX_RATING = 5;
    readonly int START_HOUR = 8;
    readonly int HOURS_IN_DAY = 20; // 00:00 - 20:00
    readonly int DAY_IN_REAL_MINUTES = 2;

    int misses;
    int scoreToday;
    float targetTime;
    float timeElapsed;
    int currentHour;
    float gameHoursPerSecond;
    bool isRushHour;

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
        InitTimer(DAY_IN_REAL_MINUTES, HOURS_IN_DAY);
        StartCoroutine(StartDay());
        StartCoroutine(RushHour());
        StartCoroutine(UpdateHour());
        keyPrompts.Init();
        //checkFare.Init();
        GameManager.OnShowBusOverlay?.Invoke();
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
        Debug.Log("Init Timer");
        Debug.Log("Target time: " + targetTime + " Game hours per second: " + gameHoursPerSecond);
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
}
