using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        LevelManager.OnHourChange += RenderTime;
    }

    private void OnDisable() {
        LevelManager.OnHourChange -= RenderTime;
    }

    void RenderTime(int time) {
        text.text = ToMeridiem(time);
    }

    string ToMeridiem(int hour) {
        DateTime date = new DateTime(1, 1, 1, hour, 0, 0);
        return date.ToString("h tt", CultureInfo.InvariantCulture);
    }
}
