using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationFromLeftOrRight : MonoBehaviour
{
    public float moveDistance = 100f;

    public AnimationCurve moveCurve;

    public float animationLength = 1.0f;

    public bool fromRight = false;

    public bool scaleByCanvas;

    private Canvas parentCanvas;

    private float canvasScale;

    private IEnumerator moveCor;

    private Vector3 startPos;

    private float curveAnimationProgress;

    private bool startPosSet = false;

    [ContextMenu("Animate")]
    public void Animate()
    {
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }

        if (gameObject.activeInHierarchy)
        {
            moveCor = AnimateCor();
            StartCoroutine(moveCor);
        }
    }

    [ContextMenu("Return")]
    public void Return()
    {
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }

        if (gameObject.activeInHierarchy)
        {
            moveCor = ReturnCor();
            StartCoroutine(moveCor);
        }
    }


    protected virtual IEnumerator AnimateCor()
    {
        //Debug.Log("Animate cor");
        if (!startPosSet)
        {
            startPos = transform.localPosition;
            canvasScale = 1.0f;

            if (scaleByCanvas)
            {
                parentCanvas = transform.root.gameObject.GetComponent<Canvas>();
                if (parentCanvas == null)
                {
                    parentCanvas = transform.parent.gameObject.GetComponentInChildren<Canvas>();
                }
            }
            startPosSet = true;
        }
    
        //Debug.Log("Start pos: " + startPos);
        if (parentCanvas != null)
        {
            canvasScale = parentCanvas.transform.localScale.x;
        }
        if (canvasScale < Mathf.Epsilon)
        {
            canvasScale = 1f;
        }

        curveAnimationProgress = 0f;// Mathf.Abs(target - 1f);

        //Vector3 heightenedPos = startPos + new Vector3(0f, dropHeight, 0f);

        Vector3 fraction = new Vector3(moveDistance * canvasScale, 0f, 0f);
        
        if(!fromRight)
        {
            fraction *= -1f;
        }

        //Debug.Log("Cor start");
        while (curveAnimationProgress != 1f)
        {
            //Debug.Log("1");
            curveAnimationProgress = Mathf.Lerp(0f, 1f, curveAnimationProgress + (Time.deltaTime / animationLength));
            transform.localPosition = startPos + fraction * moveCurve.Evaluate(curveAnimationProgress);

            //Debug.Log("Doing cor "+transform.position + " curve animation progress: "+curveAnimationProgress);
            yield return null;
        }
        moveCor = null;
    }

    protected virtual IEnumerator ReturnCor()
    {
        //Debug.Log("1");
        Vector3 curPos = Vector3.zero;
        curPos = transform.localPosition;

        while (curveAnimationProgress != 0f)
        {
            curveAnimationProgress = Mathf.Lerp(0f, 1f, curveAnimationProgress - (Time.deltaTime / animationLength));
            transform.localPosition = Vector3.Lerp(startPos, curPos, moveCurve.Evaluate(curveAnimationProgress));

            yield return null;
        }
        moveCor = null;
        /*
         protected virtual IEnumerator ReturnCor()
    {
        //Debug.Log("1");
        Vector3 curPos = Vector3.zero;
        curPos = transform.localPosition;


        Vector3 fraction = new Vector3(moveDistance * canvasScale, 0f, 0f);

        Vector3 goalPos = startPos - fraction * moveCurve.Evaluate(1f);

        while (curveAnimationProgress != 0f)
        {
            curveAnimationProgress = Mathf.Lerp(0f, 1f, curveAnimationProgress - (Time.deltaTime / animationLength));
            transform.localPosition = Vector3.Lerp(startPos, goalPos, moveCurve.Evaluate(curveAnimationProgress));

            yield return null;
        }
        moveCor = null;
    }*/
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        transform.localPosition = startPos;
        curveAnimationProgress = 0f;
        Debug.Log("Reset position " + transform.position.ToString());
    }

    public void UnResetPosition()
    {
        Vector3 fraction = new Vector3(moveDistance * canvasScale, 0f, 0f);
        StopAllCoroutines();
        transform.localPosition = startPos + fraction;
        curveAnimationProgress = 1f;
        Debug.Log("UnReset position " + transform.position.ToString());
    }
}
