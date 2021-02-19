using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public GameObject treePrefab;
    public GameObject spawnPoint;

    private void OnEnable() {
        OnInit += StartMove;
    }

    private void OnDisable() {
        OnInit -= StartMove;
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
            SpawnTree();
            // TODO: spawn with more variety?
            yield return new WaitForSeconds(2f);
        }
    }
}
