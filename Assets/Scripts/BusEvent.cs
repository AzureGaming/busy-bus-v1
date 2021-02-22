using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BusEvent : MonoBehaviour {
    public delegate void RushHourStart();
    public static RushHourStart OnRushHourStart;
    public delegate void RushHourEnd();
    public static RushHourEnd OnRushHourEnd;

    protected bool isRushHour;

    private void OnEnable() {
        OnRushHourStart += StartRush;
        OnRushHourEnd += EndRush;
    }

    private void OnDisable() {
        OnRushHourStart -= StartRush;
        OnRushHourEnd -= EndRush;
    }

    void StartRush() {
        isRushHour = true;
    }

    void EndRush() {
        isRushHour = false;
    }

    protected void Rate(float timeLeft, float timeTotal) {
        float ratio = timeLeft / timeTotal;
        if (ratio <= 0) {
            EventRating.OnFail?.Invoke();
        } else if (ratio > 0 && ratio <= 0.33) {
            EventRating.OnGood?.Invoke();
        } else if (ratio > 0.33 && ratio <= 0.66) {
            EventRating.OnGreat?.Invoke();
        } else if (ratio > 0.66) {
            EventRating.OnAwesome?.Invoke();
        }
    }
}
