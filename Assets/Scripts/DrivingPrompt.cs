using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingPrompt : MonoBehaviour {
    public delegate void Render(string key);
    public static Render OnRender;
    public delegate void Hide();
    public static Hide OnHide;

    Text text;
    CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponent<Text>();
    }

    private void OnEnable() {
        OnRender += UpdateText;
        OnHide += HideText;
    }

    private void OnDisable() {
        OnRender -= UpdateText;
        OnHide -= HideText;
    }

    void UpdateText(string key) {
        text.GetComponent<Text>().text = "Press " + key + "!";
        ShowText();
    }

    void HideText() {
        canvasGroup.alpha = 0;
    }

    void ShowText() {
        canvasGroup.alpha = 1;
    }
}
