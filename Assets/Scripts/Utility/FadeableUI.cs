using UnityEngine;
using System.Collections;


/// <summary>
/// A simple class that can be inherited to enable FadeIn / FadeOut functionality for a UI object.
/// Requires a reference to a canvasGroup.
/// </summary>
public class FadeableUI : MonoBehaviour
{
    protected const float FADE_IN_DUR = 0.3f;
    protected const float FADE_OUT_DUR = 0.2f;

    /// <summary>
    /// Allows the FadeableUI to tween without being affected by TimeScale.
    /// NOTE: This can potentially cause frames to be skipped.
    /// </summary>
    [System.NonSerialized]
    public bool useUnscaledDeltaTimeForUI = true;
    /// <summary>
    /// The canvas group that will be Faded In/Out
    /// </summary>
    [SerializeField]
    protected CanvasGroup canvasGroup;
    /// <summary>
    /// A reference to an active fade coroutine.
    /// </summary>
    protected Coroutine fadeCoroutine;
    private bool isFading = false;

    public bool IsVisible { get; protected set; }

    public CanvasGroup CanvasGroup
    {
        get
        {
            return canvasGroup;
        }
    }
    public bool IsFading
    {
        get
        {
            return isFading;
        }
    }

    void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

    }

    /// <summary>
    /// Immediately displays the canvasGroup.
    /// </summary>
    public virtual void Show()
    {
        IsVisible = true;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.gameObject.SetActive(true);
    }


    /// <summary>
    /// Immediately hides the canvasGroup.
    /// </summary>
    public virtual void Hide()
    {
        IsVisible = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);
    }


    /// <summary>
    /// Fades in the Canvas group over the defined time.
    /// Interaction is disabled until the animation has finished.
    /// </summary>
    public virtual IEnumerator FadeIn(float startAlpha = 0, float endAlpha = 1, float dur = FADE_IN_DUR)
    {
        IsVisible = true;
        isFading = true;
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = startAlpha;

        float duration = dur;
        float timeElapsed = duration * canvasGroup.alpha;

        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration);
            yield return null;
            timeElapsed += useUnscaledDeltaTimeForUI ? Time.unscaledDeltaTime : Time.deltaTime;
        }
        canvasGroup.alpha = endAlpha;
        canvasGroup.blocksRaycasts = true;
        isFading = false;
        yield break;
    }


    /// <summary>
    /// Fades out the Canvas group over the defined time.
    /// Interaction is becomes disabled immediately.
    /// </summary>
    public virtual IEnumerator FadeOut(float endAlpha = 0, float dur = FADE_OUT_DUR)
    {
        IsVisible = false;
        isFading = true;
        float duration = dur;
        float timeElapsed = duration * (1f - canvasGroup.alpha);
        canvasGroup.blocksRaycasts = false;

        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, endAlpha, timeElapsed / duration);
            yield return null;
            timeElapsed += useUnscaledDeltaTimeForUI ? Time.unscaledDeltaTime : Time.deltaTime;
        }
        canvasGroup.alpha = endAlpha;
        canvasGroup.gameObject.SetActive(canvasGroup.alpha != 0);
        isFading = false;
        yield break;
    }


    /// <summary>
    /// Starts the FadeIn coroutine inside this script.
    /// </summary>
    /// <param name="force">If true, any previously running fade animation will be cancelled</param>
    public virtual void SelfFadeIn(bool force = true, float startAlpha = 0, float endAlpha = 1, float dur = FADE_IN_DUR)
    {
        if (!force && isFading)
        {
            return;
        }
        // Make the self active incase disabled, coroutine cant run otherwise.
        this.gameObject.SetActive(true);
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn(startAlpha, endAlpha, dur));
    }


    /// <summary>
    /// Starts the FadeOut coroutine inside this script. 
    /// </summary>
    /// <param name="force">If true, any previously running fade animation will be cancelled</param>
    public virtual void SelfFadeOut(bool force = true, float endAlpha = 0, float dur = FADE_OUT_DUR)
    {
        if (!force && isFading)
        {
            return;
        }
        // Make the self active incase disabled, coroutine cant run otherwise.
        this.gameObject.SetActive(true);
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut(endAlpha, dur));
    }
}