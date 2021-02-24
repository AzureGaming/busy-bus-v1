using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRating : MonoBehaviour {
    public Sprite failSprite;
    public Sprite goodSprite;
    public Sprite greatSprite;
    public Sprite awesomeSprite;

    public AudioSource failSound;
    public AudioSource goodSound;
    public AudioSource greatSound;
    public AudioSource awesomeSound;

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
        failSound.Play();
        image.sprite = failSprite;
        StartCoroutine(Hide());
    }

    public void ShowGood() {
        goodSound.Play();
        image.sprite = goodSprite;
        StartCoroutine(Hide());
    }

    public void ShowGreat() {
        greatSound.time = 0.5f;
        greatSound.Play();
        image.sprite = greatSprite;
        StartCoroutine(Hide());
    }

    public void ShowAwesome() {
        awesomeSound.Play();
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
