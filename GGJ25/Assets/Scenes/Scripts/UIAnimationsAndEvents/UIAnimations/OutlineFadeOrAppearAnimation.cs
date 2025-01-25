using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class OutlineFadeOrAppearAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve appearCurve;

    [SerializeField]
    private float length = 0.5f;

    [SerializeField]
    private float maxAlpha = 0.5f;

    private Outline outline;

    private IEnumerator currentCor;

    [SerializeField]
    private bool setUnActiveOnFadeEnd;

    private bool isFading;

    [SerializeField]
    private UnityEvent eventOnAppearEnd;

    [SerializeField]
    private UnityEvent eventOnFadeEnd;

    private Color color; 

    void Awake()
    {
        OutlineInitCheck();
    }

    private void OutlineInitCheck()
    {
        if (outline == null)
        {
            outline = GetComponent<Outline>();
        }
    }

    public void ResetEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        SetAlpha(maxAlpha);
    }


    public void SetAlpha(float a)
    {
        color = outline.effectColor;
        color.a = a;
        outline.effectColor = color;
    }

    public void ResetHideEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        SetAlpha(0f);
    }

    public void ResetFadeEffect()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }
        SetAlpha(0f);
    }

    public void Appear()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }

        currentCor = AppearCor();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(currentCor);
        }
    }

    public void Fade()
    {
        if (currentCor != null)
        {
            StopCoroutine(currentCor);
        }

        currentCor = FadeCor();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(currentCor);
        }
    }

    private IEnumerator FadeCor()
    {
        OutlineInitCheck();
        isFading = true;
        float counter = 0f;
        SetAlpha(maxAlpha);
        while (counter < length)
        {
            yield return null;
            counter += Time.deltaTime;
            SetAlpha((1 - appearCurve.Evaluate(counter / length)) * maxAlpha);
        }
        SetAlpha(0f);
        if (setUnActiveOnFadeEnd)
        {
            gameObject.SetActive(false);
        }
        currentCor = null;
        isFading = false;
        eventOnFadeEnd.Invoke();
    }

    private IEnumerator AppearCor()
    {
        OutlineInitCheck();
        SetAlpha(0f);
        float counter = 0f;
        while (counter < length)
        {
            yield return null;
            counter += Time.deltaTime;
            SetAlpha((appearCurve.Evaluate(counter / length)) * maxAlpha);
        }
        SetAlpha(maxAlpha);
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
        OutlineInitCheck();
        SetAlpha(0f);
    }

    bool destroyed = false;

    private void OnDestroy()
    {
        destroyed = true;
    }

}
