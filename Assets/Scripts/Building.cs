using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    private void Start() {
        transform.localScale = new Vector3(0.1f, 0.1f);
        Move();
    }

    void Move() {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine() {
        // TODO: translate more realistically (curve?)
        Vector2 angle = (Quaternion.Euler(0, 0, 210) * Vector2.right);
        while (transform.localPosition.x > -35) {
            Vector3 newScale = transform.localScale;
            newScale.x += 0.0002f;
            newScale.y += 0.0002f;

            transform.Translate(angle * 0.05f);
            transform.localScale = newScale;
            yield return null;
        }
        Destroy(gameObject);
    }
}
