using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckFare : BusEvent {
    public GameObject coinPrefab;
    public GameObject fareSpawn;
    public Button acceptButton;
    public Button rejectButton;

    Coroutine waitRoutine;
    int postedFare = 5;
    int farePaid;
    bool hasResponded;
    bool stopListening;

    private void Start() {
        acceptButton.onClick.AddListener(OnClick);
        rejectButton.onClick.AddListener(OnClick);
    }

    public void Accept() {
        if (farePaid >= postedFare) {
            LevelManager.OnComplete?.Invoke();
        } else if (farePaid < postedFare) {
            LevelManager.OnMiss?.Invoke();
        }
        ClearFare();
    }

    public void Reject() {
        if ((farePaid >= postedFare) || (farePaid < postedFare)) {
            LevelManager.OnMiss?.Invoke();
        } else if ((farePaid < postedFare) || (farePaid >= postedFare)) {
            LevelManager.OnComplete?.Invoke();
        }
        ClearFare();
    }

    protected override void OnEvent() {
        BusOverlay.OnShowFare?.Invoke();
        hasResponded = false;
        LoadFare();
        waitRoutine = StartCoroutine(Wait());
    }

    protected override void OnEventComplete() {
        StopCoroutine(waitRoutine);
        BusOverlay.OnHideFare?.Invoke();
    }

    protected override IEnumerator EventListener() {
        yield return new WaitUntil(() => {
            if (hasResponded) {
                return true;
            } else if (stopListening) {
                LevelManager.OnMiss?.Invoke();
                return true;
            }
            return false;
        });
    }

    void OnClick() {
        hasResponded = true;
    }

    // todo: move to fare spawner
    void LoadFare() {
        farePaid = Random.Range(1, 7);
        for (int i = 1; i <= farePaid; i++) {
            GameObject coin = Instantiate(coinPrefab, fareSpawn.transform);
        }
    }

    void ClearFare() {
        foreach (Transform child in fareSpawn.transform) {
            Destroy(child.gameObject);
        }
    }

    protected override IEnumerator StartTimer() {
        yield return new WaitForSeconds(2f);
    }

    protected override void OnEventStop() {
        ClearFare();
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(10f);
        stopListening = true;
    }
}
