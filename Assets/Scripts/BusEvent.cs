using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BusEvent : MonoBehaviour {
    public delegate void RushHourStart();
    public static RushHourStart OnRushHourStart;
    public delegate void RushHourEnd();
    public static RushHourEnd OnRushHourEnd;
    public delegate void NewDay();
    public static NewDay OnNewDay;

    public static List<int> checkFareScores;
    public static List<int> drivingScores;

    public EventRating eventRating;

    protected bool isRushHour;
    protected enum Type {
        Driving, Fare
    };

    private void OnEnable() {
        OnRushHourStart += StartRush;
        OnRushHourEnd += EndRush;
        OnNewDay += Reset;
    }

    private void OnDisable() {
        OnRushHourStart -= StartRush;
        OnRushHourEnd -= EndRush;
        OnNewDay -= Reset;
    }

    private void Awake() {
        Reset();
    }

    private void Reset() {
        checkFareScores = new List<int>();
        checkFareScores.Add(0); // fail
        checkFareScores.Add(0); // good
        checkFareScores.Add(0); // great
        checkFareScores.Add(0); // amazing
        drivingScores = new List<int>();
        drivingScores.Add(0); // fail
        drivingScores.Add(0); // good
        drivingScores.Add(0); // great
        drivingScores.Add(0); // amazing
    }

    void StartRush() {
        isRushHour = true;
    }

    void EndRush() {
        isRushHour = false;
    }

    protected void Rate(float timeLeft, float timeTotal, Type type) {
        float ratio = timeLeft / timeTotal;
        int scoreIndex;
        if (ratio <= 0) {
            eventRating.ShowFail();
            LevelManager.OnComplete?.Invoke(-1f);
            scoreIndex = 0;
        } else if (ratio > 0 && ratio <= 0.33) {
            eventRating.ShowGood();
            LevelManager.OnComplete?.Invoke(0.25f);
            scoreIndex = 1;
        } else if (ratio > 0.33 && ratio <= 0.66) {
            eventRating.ShowGreat();
            LevelManager.OnComplete?.Invoke(0.5f);
            scoreIndex = 2;
        } else {
            eventRating.ShowAwesome();
            LevelManager.OnComplete?.Invoke(1f);
            scoreIndex = 3;
        }

        if (type == Type.Driving) {
            drivingScores[scoreIndex]++;
        } else {
            checkFareScores[scoreIndex]++;
        }
    }

    protected void Fail(Type type) {
        eventRating.ShowFail();
        LevelManager.OnComplete?.Invoke(-1f);
        if (type == Type.Driving) {
            drivingScores[0]++;
        } else {
            checkFareScores[0]++;
        }
    }
}
