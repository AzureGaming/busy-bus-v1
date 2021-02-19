using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passenger : MonoBehaviour {
    public delegate void EnterBus();
    public static EnterBus OnEnterBus;
    public delegate void LeaveBus();
    public static LeaveBus OnLeaveBus;
    public delegate void StayBus();
    public static StayBus OnStayBus;

    public Transform showPosition;
    public Transform hidePosition;
    public GameObject passenger;

    private void OnEnable() {
        OnEnterBus += Show;
        OnLeaveBus += Leave;
        OnStayBus += Stay;
    }

    private void OnDisable() {
        OnEnterBus -= Show;
        OnLeaveBus -= Leave;
        OnStayBus -= Stay;
    }

    void Show() {
        StartCoroutine(MoveRoutine(showPosition.position));
    }

    void Leave() {
        StartCoroutine(MoveRoutine(hidePosition.position));
    }

    void Stay() {
        StartCoroutine(StayRoutine());
    }

    IEnumerator MoveRoutine(Vector3 endPos) {
        float waitTime = 0.5f;
        float elapsedTime = 0f;
        Vector2 currentPos = passenger.transform.position;
        while (elapsedTime < waitTime) {
            passenger.transform.position = Vector2.Lerp(currentPos, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        passenger.transform.position = endPos;
        BusStop.OnPasengerBoard?.Invoke();
    }

    IEnumerator StayRoutine() {
        Image image = passenger.GetComponent<Image>();
        Color startingColor = image.color;

        image.color = Color.clear;
        passenger.transform.position = hidePosition.position;
        image.color = startingColor;
        yield return null;
        BusStop.OnPassengerLeave?.Invoke();
    }
}
