using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {
    public float value;

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void LoadData(CoinScriptableObject data) {
        image.sprite = data.sprite;
        value = data.value;
    }
}
