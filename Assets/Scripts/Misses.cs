using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misses : MonoBehaviour {
    public delegate void Update(int misses);
    public static Update OnUpdate;

    public GameObject missPrefab;

    private void OnEnable() {
        OnUpdate += RenderMiss;
    }

    private void OnDisable() {
        OnUpdate -= RenderMiss;
    }

    void RenderMiss(int misses) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < misses; i++) {
            Instantiate(missPrefab, transform);
        }
    }
}
