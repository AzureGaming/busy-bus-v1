using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRating : MonoBehaviour {
    public delegate void ScoreIncrease();
    public static ScoreIncrease OnScoreIncrease;
    public delegate void ScoreDecrease();
    public static ScoreDecrease OnScoreDecrease;
    public GameObject starPrefab;

    private void OnEnable() {
        OnScoreIncrease += RenderStar;
        OnScoreDecrease += DestroyStar;
    }

    private void OnDisable() {
        OnScoreIncrease -= RenderStar;
        OnScoreDecrease -= DestroyStar;
    }

    void RenderStar() {
        Instantiate(starPrefab, transform);
    }

    void DestroyStar() {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
