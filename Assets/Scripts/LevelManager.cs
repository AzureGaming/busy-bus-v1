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

    int misses;
    int scoreToday;

    private void OnEnable() {
        OnMiss += MissEvent;
        OnComplete += CompleteEvent;
    }

    private void OnDisable() {
        OnMiss -= MissEvent;
        OnComplete -= CompleteEvent;
    }

    private void Start() {
        LoadDay();    
    }

    public void LoadDay() {
        misses = 0;
        scoreToday = 0;

        StartCoroutine(StartTimer());
        keyPrompts.Init();
        checkFare.Init();
        GameManager.OnShowBusOverlay?.Invoke();
    }

    void LoseDay() {
        keyPrompts.Stop();
        checkFare.Stop();
        GameManager.OnShowLoseScreen?.Invoke();
    }

    void CompleteDay() {
        keyPrompts.Stop();
        checkFare.Stop();
        GameManager.OnShowResults?.Invoke();
    }

    void MissEvent() {
        if (misses < MAX_MISSES) {
            if (scoreToday > 0) {
                scoreToday--;
                ScoreRating.OnScoreDecrease?.Invoke();
            }
            misses++;
            Misses.OnUpdate?.Invoke(misses);
        } else {
            LoseDay();
        }
    }

    void CompleteEvent() {
        if (scoreToday < MAX_RATING) {
            scoreToday++;
            ScoreRating.OnScoreIncrease?.Invoke();
        }
    }

    IEnumerator StartTimer() {
        // todo: make calculation automatic
        int HOUR_PER_SECOND = 5;
        float targetTime = 45; // 5 seconds each hour, 9 hours a day 5*9 = 45
        int timeElapsed = 0; // seconds
        int currentHour = START_HOUR;
        int lastTime = -1;

        OnHourChange?.Invoke(currentHour);

        for (; ; ) {
            targetTime -= Time.deltaTime;
            timeElapsed = (int)targetTime % 60;
            //Debug.Log("Time elapsed: " + timeElapsed + ". Last time: " + lastTime);

            if (timeElapsed % HOUR_PER_SECOND == 0 && lastTime != timeElapsed) {
                lastTime = timeElapsed;
                currentHour++;
                OnHourChange?.Invoke(currentHour);
            }

            if (targetTime <= 0) {
                OnHourChange?.Invoke(currentHour);
                CompleteDay();
                Debug.Log("Day ends");
                yield break;
            }

            yield return null;
        }
    }
}
