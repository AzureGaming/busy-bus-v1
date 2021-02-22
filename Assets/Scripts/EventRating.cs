using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRating : MonoBehaviour {
    public delegate void Fail();
    public static Fail OnFail;
    public delegate void Good();
    public static Good OnGood;
    public delegate void Great();
    public static Great OnGreat;
    public delegate void Awesome();
    public static Awesome OnAwesome;

    public Sprite failSprite;
    public Sprite goodSprite;
    public Sprite greatSprite;
    public Sprite awesomeSprite;

    Image image;
    Color startColor;

    private void OnEnable() {
        OnFail += ShowFail;
        OnGood += ShowGood;
        OnGreat += ShowGreat;
        OnAwesome += ShowAwesome;
    }

    private void OnDisable() {
        OnFail -= ShowFail;
        OnGood -= ShowGood;
        OnGreat -= ShowGreat;
        OnAwesome -= ShowAwesome;
    }

    void Awake() {
        image = GetComponent<Image>();
    }

    void Start() {
        startColor = image.color;
        image.color = Color.clear;
    }

    void ShowFail() {
        image.sprite = failSprite;
        StartCoroutine(Hide());
    }

    void ShowGood() {
        image.sprite = goodSprite;
        StartCoroutine(Hide());
    }

    void ShowGreat() {
        image.sprite = greatSprite;
        StartCoroutine(Hide());
    }

    void ShowAwesome() {
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
