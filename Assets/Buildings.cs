using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour {
    public delegate void Init();
    public static Init OnInit;
    public GameObject buildingPrefab;

    private void OnEnable() {
        OnInit += SpawnBuilding;
    }

    private void OnDisable() {
        OnInit -= SpawnBuilding;
    }

    void SpawnBuilding() {
        Instantiate(buildingPrefab, transform);
    }
}
