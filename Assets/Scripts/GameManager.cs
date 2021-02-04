using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public delegate void ShowResultsScreen();
    public static ShowResultsScreen OnShowResults;
    public delegate void ShowBusOverlay();
    public static ShowBusOverlay OnShowBusOverlay;
    public delegate void ShowLoseScreen();
    public static ShowLoseScreen OnShowLoseScreen;

    public bool isDebug;
    public static bool IS_DEBUG;
    public GameObject resultsScreen;
    public GameObject busOverlay;
    public GameObject loseScreen;

    private void Awake() {
        IS_DEBUG = isDebug;
    }

    private void OnEnable() {
        OnShowResults += RenderResultsScreen;
        OnShowBusOverlay += RenderBusOverlay;
        OnShowLoseScreen += RenderLoseScreen;
    }

    private void OnDisable() {
        OnShowResults -= RenderResultsScreen;
        OnShowBusOverlay -= RenderBusOverlay;
        OnShowLoseScreen -= RenderLoseScreen;
    }

    void RenderResultsScreen() {
        SetResultsScreen(true);
        SetBusOverlay(false);
        SetLoseScreen(false);
    }

    void RenderBusOverlay() {
        SetResultsScreen(false);
        SetBusOverlay(true);
        SetLoseScreen(false);
        FareWindow.OnClose?.Invoke(true);
    }

    void RenderLoseScreen() {
        SetResultsScreen(false);
        SetBusOverlay(false);
        SetLoseScreen(true);
    }

    void SetResultsScreen(bool isActive) {
        resultsScreen.SetActive(isActive);
    }

    void SetBusOverlay(bool isActive) {
        busOverlay.SetActive(isActive);
    }

    void SetLoseScreen(bool isActive) {
        loseScreen.SetActive(isActive);
    }
}
