using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public GameObject buildingPrefab;

    Coroutine coroutine;

    private void OnEnable() {
        OnInit += StartMove;
    }

    private void OnDisable() {
        OnInit -= StartMove;
    }

    void SpawnBuilding() {
        Instantiate(buildingPrefab, transform);
    }

    void StartMove() {
        coroutine = StartCoroutine(MoveRoutine());
        //StartCoroutine(SpawnBuildingRoutine());
    }

    IEnumerator MoveRoutine() {
        for (; ; ) {
            //Vector3 newScale = transform.localScale;
            Vector2 angle = new Vector2(Mathf.Cos(260), Mathf.Sin(260)).normalized;
            //newScale.x += 0.0005f;
            //newScale.y += 0.0005f;

            transform.Translate(angle * 0.1f);
            //transform.localScale = newScale;
            yield return null;
        }
    }

    IEnumerator SpawnBuildingRoutine() {
        for (; ; ) {
            yield return new WaitForSeconds(1f);
            SpawnBuilding();
        }
    }
}
