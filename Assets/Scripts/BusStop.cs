using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusStop : MonoBehaviour {
    public delegate void PassengerBoard();
    public static PassengerBoard OnPasengerBoard;
    public delegate void PassengerLeave();
    public static PassengerLeave OnPassengerLeave;

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Start() {
        HideImage();
    }

    private void OnEnable() {
        OnPasengerBoard += ShowImage;
        OnPassengerLeave += HideImage;
    }

    private void OnDisable() {
        OnPasengerBoard -= ShowImage;
        OnPassengerLeave -= HideImage;
    }

    void HideImage() {
        image.enabled = false;
    }

    void ShowImage() {
        image.enabled = true;
    }
}
