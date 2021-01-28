using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
    TMP_Text scoreText;

    private void Awake() {
        scoreText = GetComponent<TMP_Text>();
    }

    public void SetValue(int score) {
        scoreText.text = score.ToString();
    }
}
