using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLines : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public GameObject linePrefab;

    private void OnEnable() {
        OnInit += SpawnLine;
    }

    private void OnDisable() {
        OnInit -= SpawnLine;
    }

    void SpawnLine() {
        Instantiate(linePrefab, transform);
    }
}
