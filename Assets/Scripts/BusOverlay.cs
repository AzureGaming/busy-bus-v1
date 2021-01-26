using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusOverlay : MonoBehaviour {
    public delegate void HideFare();
    public static HideFare OnHideFare;
    public delegate void ShowFare();
    public static ShowFare OnShowFare;

    public GameObject fareWindow;

    private void OnEnable() {
        OnHideFare += HideFareWindow;
        OnShowFare += ShowFareWindow;
    }

    private void OnDisable() {
        OnHideFare -= HideFareWindow;
        OnShowFare -= ShowFareWindow;
    }

    void HideFareWindow() {
        SetFareWindow(false);
    }

    void ShowFareWindow() {
        SetFareWindow(true);
    }

    void SetFareWindow(bool isActive) {
        fareWindow.SetActive(isActive);
    }
}
