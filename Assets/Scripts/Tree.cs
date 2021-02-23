using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    public delegate void Stop();
    public static Stop OnStop;
    public delegate void Continue();
    public static Continue OnContinue;

    bool isStopped = false;

    private void OnEnable() {
        OnStop += StopMoving;
        OnContinue += ResumeMoving;
    }

    private void OnDisable() {
        OnStop -= StopMoving;
        OnContinue -= ResumeMoving;
    }


    private void Start() {
        transform.localScale = new Vector3(0.1f, 0.1f);
        Move();
    }

    void Move() {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        // TODO: translate more realistically (curve?)
        Vector2 angle = ( Quaternion.Euler(0, 0, 220) * Vector2.right );
        while (transform.localPosition.x > -75) {
            if (!isStopped) {
                Vector3 newScale = transform.localScale;
                newScale.x += 0.0003f;
                newScale.y += 0.0003f;

                transform.Translate(angle * 0.05f);
                transform.localScale = newScale;
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    void StopMoving() {
        isStopped = true;
    }

    void ResumeMoving() {
        isStopped = false;
    }
}
