using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour {
    public GameObject oldStars;
    public GameObject starsContainer;
    public TMP_Text fareCheckFail;
    public TMP_Text fareCheckGood;
    public TMP_Text fareCheckGreat;
    public TMP_Text fareCheckAmazing;
    public TMP_Text drivingCheckFail;
    public TMP_Text drivingCheckGood;
    public TMP_Text drivingCheckGreat;
    public TMP_Text drivingCheckAmazing;
    public TMP_Text dollarsEarned;

    private void OnEnable() {
        RenderStars();
        RenderFareChecks();
        RenderDrivingChecks();
        RenderDollarsEarned();
    }

    public void TryAgain() {
        SceneManager.LoadScene("Bus");
    }

    public void MainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    void RenderStars() {
        foreach (Transform child in oldStars.transform) {
            if (child.gameObject.activeInHierarchy) {
                Instantiate(child.gameObject, starsContainer.transform);
            }
        }
    }

    void RenderFareChecks() {
        fareCheckFail.text = BusEvent.checkFareScores[0].ToString();
        fareCheckGood.text = BusEvent.checkFareScores[1].ToString();
        fareCheckGreat.text = BusEvent.checkFareScores[2].ToString();
        fareCheckAmazing.text = BusEvent.checkFareScores[3].ToString();
    }

    void RenderDrivingChecks() {
        drivingCheckFail.text = BusEvent.drivingScores[0].ToString();
        drivingCheckGood.text = BusEvent.drivingScores[1].ToString();
        drivingCheckGreat.text = BusEvent.drivingScores[2].ToString();
        drivingCheckAmazing.text = BusEvent.drivingScores[3].ToString();
    }

    void RenderDollarsEarned() {
        dollarsEarned.text = "$" + starsContainer.transform.childCount;
    }
}
