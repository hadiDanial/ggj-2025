using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class UIDropAnimation : MonoBehaviour
{
    public float dropHeight = 100;

    public AnimationCurve dropCurve;

    public float animationLength = 1.0f;

    public bool flipCurveY = true;

    public float waitTime = 1;

    protected float target;

    public bool scaleByCanvas = true;

    private float canvasScale;

    private Canvas parentCanvas;
    private Vector3 startPos;

    private IEnumerator moveCor;

    [SerializeField]
    private bool dropped;

    private RectTransform rectTransform;

    public bool usesRectTran = false; //Use the UI rect transform for scaling

    public bool startPosSet = false;

    /*
    private RectTransform rectTran;

    private void Awake()
    {
        if (rectTran == null)
        {
            rectTran = GetComponent<RectTransform>();
        }
    }
    */

    private void OnEnable()
    {
        canvasScale = 1f;
        if(scaleByCanvas)
        {
            parentCanvas = GetComponentInParent<Canvas>();
            if(parentCanvas == null)
            {
                parentCanvas = transform.parent.gameObject.GetComponentInChildren<Canvas>();
            }
        }

       
    }

    [ContextMenu("Drop Anim")]
    public virtual void Drop()
    {
        if(destroyed)
        {
            return;
        }
        if(moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }

        target = 1;
        if(gameObject.activeInHierarchy)
        {
            moveCor = MoveYCor();
            StartCoroutine(moveCor);
            dropped = true;
        }
    }

    public void CancelDrop()
    {
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }
    }

    [ContextMenu("Rise Anim")]
    public virtual void Rise()
    {
        //StopCoroutine(MoveYCor());
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }
        target = 0;

        //StartCoroutine(MoveYCor());
        if (gameObject.activeInHierarchy)
        {
            moveCor = MoveYCor();
            StartCoroutine(moveCor);

            dropped = false;
        }
       
    }

    public void DropWait(float waitTime)
    {
        Invoke("Drop", waitTime);
    }

    public void RiseWait(float waitTime)
    {
        Invoke("RiseWait", waitTime);
    }

    public void DropWait()
    {
        Invoke("Drop", waitTime);
    }

    public void RiseWait()
    {
        Invoke("RiseWait", waitTime);
    }

    public void DropWaitRise()
    {
        Drop();
        Invoke("Rise", waitTime + animationLength);
    }



    protected virtual IEnumerator MoveYCor()
    {
        //Debug.Log("Move Y Cor");
        if (rectTransform == null)
        {
            rectTransform = transform.GetComponent<RectTransform>();
        }
        if (!startPosSet)
        {
            startPos = transform.localPosition;//rectTransform.anchoredPosition;
            if(usesRectTran)
            {
                startPos = rectTransform.anchoredPosition;
            }
            startPosSet = true;
        }
        //Debug.Log("Start pos: " + startPos);
        //Debug.Log("Has canvas: " + (parentCanvas != null));
        if (parentCanvas != null)
        {
            canvasScale = parentCanvas.transform.localScale.x;
        }
        if (canvasScale < Mathf.Epsilon)
        {
            canvasScale = 1f;
        }

        //Debug.Log("canvas scale: " + canvasScale);
        float curveAnimationProgress = Mathf.Abs(target - 1f);

        //Vector3 heightenedPos = startPos + new Vector3(0f, dropHeight, 0f);
        Vector3 fraction;
        if (scaleByCanvas)
        {
            fraction = new Vector3(0f, dropHeight / canvasScale, 0f);
        }
        else
        {
            fraction = new Vector3(0f, dropHeight , 0f);
        }

        //Debug.Log("Cor start");
        while (curveAnimationProgress != target)
        {
            curveAnimationProgress = Mathf.Lerp(0f, 1f, curveAnimationProgress + (Time.deltaTime / animationLength) * ((target - 0.5f) * 2f));

            Vector3 movePos = DoYMove(fraction, curveAnimationProgress);

            if (usesRectTran)
            {
                rectTransform.anchoredPosition = movePos;
            }
            else
            {
                transform.localPosition = movePos;
            }
            /*
            if (flipCurveY)
            {
                transform.localPosition = startPos + fraction *  (1.0f - dropCurve.Evaluate(curveAnimationProgress));
            }
            else
            {
                transform.localPosition = startPos + fraction * dropCurve.Evaluate(curveAnimationProgress);
            }
            */
            //Debug.Log("Doing cor "+transform.position + " curve animation progress: "+curveAnimationProgress);
            yield return null;
        }
    }

    private Vector3 DoYMove(Vector3 fraction, float curveAnimationProgress)
    {
        if (flipCurveY)
        {
            return startPos + fraction * (1.0f - dropCurve.Evaluate(curveAnimationProgress));
        }
        else
        {
            return startPos + fraction * dropCurve.Evaluate(curveAnimationProgress);
        }
    }

    public bool GetDropped()
    {
        return dropped;
    }

    public void SetStartPos(Vector3 pos)
    {
        startPos = pos;
    }

    private bool destroyed = false; 

    private void OnDestroy()
    {
        destroyed = true;
    }
}
