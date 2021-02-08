using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FarePoster : MonoBehaviour {
    public delegate void UpdateFare(float range1, float range2, float range3, float range4);
    public static UpdateFare OnUpdateFare;
    public delegate void HighlightFare(int slot);
    public static HighlightFare OnHighlightFare;

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public List<GameObject> slots;

    private void OnEnable() {
        OnUpdateFare += UpdateAllText;
        OnHighlightFare += UpdateColor;
    }

    private void OnDisable() {
        OnUpdateFare -= UpdateAllText;
        OnHighlightFare -= UpdateColor;
    }

    void UpdateAllText(float range1, float range2, float range3, float range4) {
        List<float> ranges = new List<float>() { range1, range2, range3, range4 };
        for (int i = 0; i < ranges.Count; i++) {
            slots[i].GetComponentsInChildren<TMP_Text>()[1].text = string.Format("${0:F2}", ranges[i]);
        }
    }

    void UpdateColor(int slotIndex) {
        foreach (GameObject slot in slots) {
            TMP_Text[] texts = slot.GetComponentsInChildren<TMP_Text>() ;
            foreach (TMP_Text text in texts) {
                text.color = Color.white;
            }
        }

        foreach (TMP_Text text in slots[slotIndex].GetComponentsInChildren<TMP_Text>()) {
            text.color = Color.green;
        }
    }
}
