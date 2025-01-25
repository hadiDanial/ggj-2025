using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIGrowAnimation : MonoBehaviour
{
    //[SerializeField]
    public GameObject changeSizeObj; //Object to grow or shrink. If null, automatically becomes this object
    [SerializeField]
    private AnimationCurve growCurve; //The curve for the grow animation. Shrink animation curve is the opposite
    [SerializeField]
    private float animationLength = 0.25f;
    [SerializeField]
    private float curveScale = 0.125f; //curve * scale
    [SerializeField]
    private float addSize = 1.0f; //curve * scale + addsize

    private float curveAnimationProgress; //0->1 progress of the curve animation

    [SerializeField]
    private bool preventSpam = false;

    private IEnumerator changeSizeCor;

    private float target;

    private bool stopped;

    private UnityEvent eventOnGrow;

    //private bool growAfterChangeSize;
    //private bool shrinkAfterChangeSize;

    //Make sure something changes it's size
    void OnValidate()
    {
        if (changeSizeObj == null)
        {
            changeSizeObj = gameObject;
        }
    }

    public bool CheckMaxSize()
    {
        //Check if at max size
        return changeSizeObj.transform.localScale.x == growCurve.Evaluate(1) * curveScale + addSize;
    }

    public bool CheckMinSize()
    {
        return changeSizeObj.transform.localScale.x == growCurve.Evaluate(0) * curveScale + addSize;
    }

    public void Grow()
    { 
        if(changeSizeCor != null)
        {
            if(preventSpam)
            {
                return;
            }
            else
            {
                stopped = true;
                StopCoroutine(changeSizeCor);
            }
        }
        target = 1.0f;
        if(gameObject.activeInHierarchy)
        {
            changeSizeCor = ChangeSize();
            StartCoroutine(changeSizeCor);
        }
    }

    public void Shrink()
    {
        if(changeSizeCor != null)
        {
            stopped = true;
            StopCoroutine(changeSizeCor);
        }
        target = 0.0f;
        if (gameObject.activeInHierarchy)
        {
            changeSizeCor = ChangeSize();
            StartCoroutine(changeSizeCor);
        }
    }

    private IEnumerator ChangeSize()
    {
        if(stopped)
        {
            stopped = false;
        }
        else
        {
            curveAnimationProgress = Mathf.Abs(target - 1f);
        }
        while (curveAnimationProgress != target)
        {
            //Progression of curve or reverse curve
            curveAnimationProgress = Mathf.Lerp(0f, 1f, curveAnimationProgress + (Time.deltaTime / animationLength) * ((target - 0.5f) * 2f));
            //Animation
            float newScale = growCurve.Evaluate(curveAnimationProgress /* * ((target - 0.5f) * 2f)*/) * curveScale + addSize;
            changeSizeObj.transform.localScale = new Vector3(newScale, newScale, 1f);
            yield return null;
        }
        /*
        if(growAfterChangeSize)
        {
            growAfterChangeSize = false;
            Grow();
        }
        else if(shrinkAfterChangeSize)
        {
            shrinkAfterChangeSize = false;
            Shrink();
        }
        */
        changeSizeCor = null;

        if(eventOnGrow != null)
        {
            eventOnGrow.Invoke();
        }

    }
    /*
    public void GrowShrink()
    {
        growAfterChangeSize = true;
        Grow();
    }
    */
    public void Cancel()
    {
        if(changeSizeCor != null)
        {
            StopCoroutine(changeSizeCor);
        }
        changeSizeObj.transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        if(transform != null)
        {
            changeSizeObj.transform.localScale = Vector3.one;
        }
    }

    public void SetEventOnGrow(UnityEvent setEvent)
    {
        eventOnGrow = setEvent;
    }
}

