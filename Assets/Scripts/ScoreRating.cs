using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRating : MonoBehaviour {
    public delegate void UpdateScore(int score);
    public static UpdateScore OnUpdateScore;

    int MAX_SCORE = 100;

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

    void HandleUpdate(int value) {
        stars.ClearStars();
        int iterations = (int)Mathf.Round(value);
        for (int i = 0; i < iterations; i++) {
            stars.RenderStar();
        }

        if (GameManager.IS_DEBUG) {
            Debug.Log(iterations);
        }
    }
}
