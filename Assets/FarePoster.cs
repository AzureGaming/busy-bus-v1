using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarePoster : MonoBehaviour {
    public delegate void UpdateFare(float range1, float range2, float range3, float range4);
    public static UpdateFare OnUpdateFare;

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;

    private void OnEnable() {
        OnUpdateFare += UpdatePoster;
    }

    private void OnDisable() {
        OnUpdateFare += UpdatePoster;
    }

    void UpdatePoster(float range1, float range2, float range3, float range4) {
        slot1.GetComponentsInChildren<TMP_Text>()[1].text = string.Format("${0:F2}", range1);
        slot2.GetComponentsInChildren<TMP_Text>()[1].text = string.Format("${0:F2}", range2);
        slot3.GetComponentsInChildren<TMP_Text>()[1].text = string.Format("${0:F2}", range3);
        slot4.GetComponentsInChildren<TMP_Text>()[1].text = string.Format("${0:F2}", range4);
    }
}
