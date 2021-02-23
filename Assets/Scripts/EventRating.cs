using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRating : MonoBehaviour {
    public Sprite failSprite;
    public Sprite goodSprite;
    public Sprite greatSprite;
    public Sprite awesomeSprite;

    Image image;
    Color startColor;

    void Awake() {
        image = GetComponent<Image>();
    }

    void Start() {
        startColor = image.color;
        image.color = Color.clear;
    }

    public void ShowFail() {
        image.sprite = failSprite;
        StartCoroutine(Hide());
    }

    public void ShowGood() {
        image.sprite = goodSprite;
        StartCoroutine(Hide());
    }

    public void ShowGreat() {
        image.sprite = greatSprite;
        StartCoroutine(Hide());
    }

    public void ShowAwesome() {
        image.sprite = awesomeSprite;
        StartCoroutine(Hide());
    }

    IEnumerator Hide() {
        float timeElapsed = 0f;
        float totalTime = 0.75f;

        while (timeElapsed < totalTime) {
            image.color = Color.Lerp(startColor, Color.clear, ( timeElapsed / totalTime ));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
