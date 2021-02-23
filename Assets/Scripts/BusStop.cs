using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusStop : MonoBehaviour {
    public delegate void Show();
    public static Show OnShow;
    public delegate void Hide();
    public static Hide OnHide;

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start() {
        HideImage();
    }

    private void OnEnable() {
        OnShow += ShowImage;
        OnHide += HideImage;
    }

    private void OnDisable() {
        OnShow -= ShowImage;
        OnHide -= HideImage;
    }

    void HideImage() {
        image.enabled = false;
    }

    void ShowImage() {
        image.enabled = true;
    }
}
