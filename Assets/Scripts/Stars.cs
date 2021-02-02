using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {
    public GameObject starPrefab;

    public void RenderStar() {
        Instantiate(starPrefab, transform);
    }

    public void ClearStars() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void DestroyStar() {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
