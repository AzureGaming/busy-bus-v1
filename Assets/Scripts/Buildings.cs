using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public delegate void Stop();
    public static Stop OnStop;
    public delegate void Continue();
    public static Continue OnContinue;

    public GameObject buildingPrefab;
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

    void SpawnBuilding() {
        GameObject instance = Instantiate(buildingPrefab, spawnPoint.transform.position, Quaternion.identity, transform);
        instance.transform.SetSiblingIndex(0);
    }

    void StartMove() {
        StartCoroutine(SpawnBuildingRoutine());
    }

    IEnumerator SpawnBuildingRoutine() {
        for (; ; ) {
            if (!isStopped) {
                SpawnBuilding();
                // TODO: spawn with more variety?
                yield return new WaitForSeconds(2f);
            } else {
                yield return null;
            }
        }
    }

    void StopMoving() {
        isStopped = true;
        Building.OnStop?.Invoke();
    }

    void ResumeMoving() {
        isStopped = false;
        Building.OnContinue?.Invoke();
    }
}
