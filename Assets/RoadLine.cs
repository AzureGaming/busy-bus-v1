using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLine : MonoBehaviour {
    public delegate void OffScreen();
    public static OffScreen OnOffScreen;
    public delegate void Init();
    public static Init OnInit;
    public delegate void Stop();
    public static Stop OnStop;

    Coroutine routine;
    bool isVisible;

    private void OnEnable() {
        //OnInit += Move;
        OnStop += Kill;
    }

    private void OnDisable() {
        //OnInit -= Move;
        OnStop -= Kill;
    }

    private void Start() {
        Move();
    }

    private void OnBecameInvisible() {
        Debug.Log("invisible");
        isVisible = false;
    }

    void Move() {
        isVisible = true;
        routine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        while (transform.position.y > -404) {
            transform.Translate(-Vector3.up);
            yield return null;
        }
        RoadLines.OnInit?.Invoke();
        Kill();
    }

    void StopMoveRoutine() {
        if (routine != null) {
            StopCoroutine(routine);
        }
    }

    void Kill() {
        StopMoveRoutine();
        Destroy(gameObject);
    }
}
