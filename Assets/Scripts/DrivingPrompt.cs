using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingPrompt : MonoBehaviour {
    public delegate void Prompt(KeyPrompts.ActionName actionName);
    public static Prompt OnPrompt;
    public delegate void Hide();
    public static Hide OnHide;

    public Image background;
    public Image prompt;
    public Sprite promptSprite;

    CanvasGroup canvasGroup;
    Vector3 promptLocalScaleStart;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        promptLocalScaleStart = prompt.GetComponent<RectTransform>().localScale;
    }

    private void OnEnable() {
        OnPrompt += Show;
        OnHide += Invisible;
    }

    private void OnDisable() {
        OnPrompt -= Show;
        OnHide -= Invisible;
    }

    void Invisible() {
        canvasGroup.alpha = 0;
    }

    void Show(KeyPrompts.ActionName actionName) {
        canvasGroup.alpha = 1;
        RectTransform promptRectTransform = prompt.GetComponent<RectTransform>();

        switch (actionName) {
            case KeyPrompts.ActionName.Forward:
                break;
            case KeyPrompts.ActionName.Left:
                prompt.sprite = promptSprite;
                promptRectTransform.localScale = promptLocalScaleStart;
                break;
            case KeyPrompts.ActionName.Right:
                Debug.Log("Render");
                Vector3 newScale = promptLocalScaleStart;
                newScale.x *= -1;
                prompt.sprite = promptSprite;
                promptRectTransform.localScale = newScale;
                break;
            case KeyPrompts.ActionName.Stop:
                break;
            default:
                break;
        }
    }
}
