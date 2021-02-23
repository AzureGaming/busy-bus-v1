using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public delegate void Stop();
    public static Stop OnStop;
    public delegate void Continue();
    public static Continue OnContinue;

    public GameObject treePrefab;
    public GameObject spawnPoint;

    bool isStopped = false;

    private void OnEnable() {
        OnInit += StartMove;
        OnStop += StopMoving;
        OnContinue += ResumeMoving;
    }

    private void OnDisable() {
        OnInit -= StartMove;
        OnStop -= StopMoving;
        OnContinue -= ResumeMoving;
    }

    void SpawnTree() {
        GameObject instance = Instantiate(treePrefab, spawnPoint.transform.position, Quaternion.identity, transform);
        instance.transform.SetSiblingIndex(0);
    }

    void StartMove() {
        StartCoroutine(SpawnTreeRoutine());
    }

    IEnumerator SpawnTreeRoutine() {
        for (; ; ) {
            if (!isStopped) {
                SpawnTree();
                // TODO: spawn with more variety?
                yield return new WaitForSeconds(2f);
            } else {
                yield return null;
            }
        }
    }

    void StopMoving() {
        isStopped = true;
        Tree.OnStop?.Invoke();
    }

    void ResumeMoving() {
        isStopped = false;
        Tree.OnContinue?.Invoke();
    }
}
