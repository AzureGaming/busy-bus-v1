using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRating : MonoBehaviour {
    public delegate void UpdateScore(float score);
    public static UpdateScore OnUpdateScore;

    Stars stars;

    private void Awake() {
        stars = GetComponentInChildren<Stars>();
    }

    private void OnEnable() {
        OnUpdateScore += HandleUpdate;
    }

    private void OnDisable() {
        OnUpdateScore -= HandleUpdate;
    }

    void HandleUpdate(float value) {
        stars.ClearStars();
        if (value >= 5) {
            stars.Render100Star();
            stars.Render100Star();
            stars.Render100Star();
            stars.Render100Star();
            stars.Render100Star();
        } else {
            while (value - 1 >= 0) {
                value--;
                stars.Render100Star();
            }

            while (value - 0.75f >= 0) {
                value -= 0.75f;
                stars.Render75Star();
            }

            while (value - 0.5f >= 0) {
                value -= 0.5f;
                stars.Render50Star();
            }

            while (value - 0.25f >= 0) {
                value -= 0.25f;
                stars.Render25Star();
            }
        }
    }
}
