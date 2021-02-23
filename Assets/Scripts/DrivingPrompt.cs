using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingPrompt : MonoBehaviour {
    public delegate void Prompt(KeyPrompts.ActionName actionName, float totalTime);
    public static Prompt OnPrompt;
    public delegate void Hide();
    public static Hide OnHide;
    public delegate void PromptBrake(int index);
    public static PromptBrake OnPromptBrake;

    public Image background;
    public Image prompt;
    public Sprite arrowPromptSprite;
    public Sprite[] brakeSprites;

    CanvasGroup canvasGroup;
    Vector3 promptLocalScaleStart;
    Coroutine invisibleRoutine;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() {
        promptLocalScaleStart = prompt.GetComponent<RectTransform>().localScale;
    }

    private void OnEnable() {
        OnPrompt += Show;
        OnHide += Invisible;
        OnPromptBrake += ShowBrake;
    }

    private void OnDisable() {
        OnPrompt -= Show;
        OnHide -= Invisible;
        OnPromptBrake -= ShowBrake;
    }

    void Invisible() {
        if (invisibleRoutine != null) {
            StopCoroutine(invisibleRoutine);
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator InvisibleRoutine(float totalTime) {
        //float timeElapsed = 0f;
        //float startingAlpha = canvasGroup.alpha;

        //while (timeElapsed < totalTime) {
        //    float alpha;
        //    alpha = Mathf.Lerp(startingAlpha, 0, ( timeElapsed / totalTime ));
        //    timeElapsed += Time.deltaTime;
        //    canvasGroup.alpha = alpha;
        //    yield return null;
        //}
        yield return new WaitForSeconds(totalTime);
        canvasGroup.alpha = 0;
    }

    void Show(KeyPrompts.ActionName actionName, float totalTime) {
        canvasGroup.alpha = 1;
        RectTransform promptRectTransform = prompt.GetComponent<RectTransform>();

        switch (actionName) {
            case KeyPrompts.ActionName.Forward:
                break;
            case KeyPrompts.ActionName.Left:
                prompt.sprite = arrowPromptSprite;
                promptRectTransform.localScale = promptLocalScaleStart;
                break;
            case KeyPrompts.ActionName.Right:
                Vector3 newScale = promptLocalScaleStart;
                newScale.x *= -1;
                prompt.sprite = arrowPromptSprite;
                promptRectTransform.localScale = newScale;
                break;
            case KeyPrompts.ActionName.Stop:
                promptRectTransform.localScale = promptLocalScaleStart;
                prompt.sprite = brakeSprites[0];
                break;
            default:
                break;
        }
        invisibleRoutine = StartCoroutine(InvisibleRoutine(totalTime));
    }

    void ShowBrake(int index) {
        prompt.sprite = brakeSprites[index];
    }
}
