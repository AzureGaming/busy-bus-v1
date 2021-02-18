using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
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
        //transform.localScale = new Vector3(0.1f, 0.1f);
        Move();
    }

    void Move() {
        isVisible = true;
        routine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        while (transform.localPosition.y > -700) {
            Vector3 newScale = transform.localScale;
            //if (newScale.x < 1) {
                newScale.x += 0.0001f;
            //}
            //if (newScale.y < 1) {
                newScale.y += 0.0001f;
            //}
            transform.Translate(-Vector3.up * 0.25f);
            transform.localScale = newScale;
            yield return null;
        }
        Buildings.OnInit?.Invoke();
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
