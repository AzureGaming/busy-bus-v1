using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLine : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public delegate void Stop();
    public static Stop OnStop;
    public delegate void Continue();
    public static Continue OnContinue;

    bool isStopped = false;
    Coroutine routine;

    private void OnEnable() {
        OnStop += StopMoving;
        OnContinue += ResumeMoving;
    }

    private void OnDisable() {
        OnStop -= StopMoving;
        OnContinue -= ResumeMoving;
    }

    private void Start() {
        Move();
    }

    void Move() {
        routine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        while (transform.position.y > -404) {
            if (!isStopped) {
                transform.Translate(-Vector3.up * 0.75f);
            }
            yield return null;
        }
        RoadLines.OnInit?.Invoke();
        Destroy(gameObject);
    }

    void StopMoveRoutine() {
        if (routine != null) {
            StopCoroutine(routine);
        }
    }

    void StopMoving() {
        isStopped = true;
    }

    void ResumeMoving() {
        isStopped = false;
    }
}
