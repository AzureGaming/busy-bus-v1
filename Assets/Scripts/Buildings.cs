using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public GameObject buildingPrefab;
    public GameObject spawnPoint;

    private void OnEnable() {
        OnInit += StartMove;
    }

    private void OnDisable() {
        OnInit -= StartMove;
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
            SpawnBuilding();
            // TODO: spawn with more variety?
            yield return new WaitForSeconds(2f);
        }
    }
}
