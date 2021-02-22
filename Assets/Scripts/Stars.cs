using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {
    public GameObject starPrefab100;
    public GameObject starPrefab75;
    public GameObject starPrefab50;
    public GameObject starPrefab25;

    public void Render100Star() {
        Instantiate(starPrefab100, transform);
    }

    public void Render75Star() {
        Instantiate(starPrefab75, transform);
    }

    public void Render50Star() {
        Instantiate(starPrefab50, transform);
    }

    public void Render25Star() {
        Instantiate(starPrefab25, transform);
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
