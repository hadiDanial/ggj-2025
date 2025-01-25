using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeOrAppearAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve appearCurve;

    [SerializeField]
    private float length = 0.5f;

    private CanvasGroup group;

    private IEnumerator currentCor;

    [SerializeField]
    private bool setUnActiveOnFadeEnd;

    private bool isFading;

    [SerializeField]
    private UnityEvent eventOnAppearEnd;

    [SerializeField]
    private UnityEvent eventOnFadeEnd;

    void Awake()
    {
        GroupInitCheck();
    }

    private void GroupInitCheck()
    {
        if (group == null)
        {
            group = GetComponent<CanvasGroup>();
        }
    }

    public void ResetEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        group.alpha = 1f;
    }

    public void ResetHideEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        group.alpha = 0f;
    }

    public void ResetFadeEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        group.alpha = 0f;
    }

    public void Appear()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        if(currentCor != null)
        {
            StopCoroutine(currentCor);
        }

        currentCor = AppearCor();
        if(gameObject != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(currentCor);
        }
    }

    public void FadeInstant()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        currentCor = null;
        group.alpha = 0f;
    }

    public void AppearInstant()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        currentCor = null;
        group.alpha = 1f;
    }

    public void Fade()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }

        currentCor = FadeCor();
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(currentCor);
        }
    }

    public void FadeHalf()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }

        currentCor = FadeHalfCor();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(currentCor);
        }
    }

    private IEnumerator FadeCor()
    {
        GroupInitCheck();
        isFading = true;
        float counter = 0f;
        group.alpha = 1 - appearCurve.Evaluate(0f);
        while (counter < length)
        {
            yield return null;
            counter += Time.deltaTime;
            group.alpha = 1-appearCurve.Evaluate(counter / length);
        }
        group.alpha = 0f;
        if(setUnActiveOnFadeEnd)
        {
            gameObject.SetActive(false);
        }
        currentCor = null;
        isFading = false;
        eventOnFadeEnd.Invoke();
    }

    private IEnumerator FadeHalfCor()
    {
        GroupInitCheck();
        isFading = true;
        float counter = 0f;
        group.alpha = 1 - appearCurve.Evaluate(0f);
        while (counter < length/2f)
        {
            yield return null;
            counter += Time.deltaTime;
            group.alpha = 1 - appearCurve.Evaluate(counter / length);
        }
        currentCor = null;
        isFading = false;
        eventOnFadeEnd.Invoke();
    }

    private IEnumerator AppearCor()
    {
        GroupInitCheck();
        group.alpha = 0f;
        float counter = 0f;
        while (counter < length)
        {
            yield return null;
            counter += Time.deltaTime;
            group.alpha = appearCurve.Evaluate(counter / length);
        }
        group.alpha = 1f;
        currentCor = null;
        eventOnAppearEnd.Invoke();
    }

    public bool IsFading()
    {
        return isFading;
    }

    public void SetFadeLength(float setLength)
    {
        length = setLength;
    }

    public void HideCanvas()
    {
        GroupInitCheck();
        group.alpha = 0f;
    }

    bool destroyed = false;

    private void OnDestroy()
    {
        destroyed = true;
    }
}
