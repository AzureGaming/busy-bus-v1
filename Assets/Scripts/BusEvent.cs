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
        Debug.Log("start rush");
        isRushHour = true;
    }

    void EndRush() {
        isRushHour = false;
    }
}
