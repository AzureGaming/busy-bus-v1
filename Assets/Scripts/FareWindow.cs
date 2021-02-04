using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareWindow : MonoBehaviour {
    public delegate void Open(bool isInstant);
    public static Open OnOpen;
    public delegate void Close(bool isInstant);
    public static Close OnClose;

    public Transform openedPos;
    public Transform closedPos;

    float OPEN_SLIDE_TIME = 0.5f;
    float CLOSE_SLIDE_TIME = 0.5f;

    private void OnEnable() {
        OnOpen += OpenWindow;
        OnClose += CloseWindow;
    }

    private void OnDisable() {
        OnOpen -= OpenWindow;
        OnClose -= CloseWindow;
    }

    void OpenWindow(bool isInstant) {
        StartCoroutine(SlideLeft(isInstant));
    }

    void CloseWindow(bool isInstant) {
        StartCoroutine(SlideRight(isInstant));
    }

    IEnumerator SlideLeft(bool isInstant) {
        float elapsedTime = 0f;
        Vector2 currentPos = transform.position;
        while (elapsedTime < OPEN_SLIDE_TIME && !isInstant) {
            transform.position = Vector2.Lerp(currentPos, openedPos.position, (elapsedTime / OPEN_SLIDE_TIME));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = openedPos.position;
    }

    IEnumerator SlideRight(bool isInstant) {
        float elapsedTime = 0f;
        Vector2 currentPos = transform.position;
        while (elapsedTime < CLOSE_SLIDE_TIME && !isInstant) {
            Debug.Log("Helo");
            transform.position = Vector2.Lerp(currentPos, closedPos.position, (elapsedTime / CLOSE_SLIDE_TIME));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = closedPos.position;
    }
}
