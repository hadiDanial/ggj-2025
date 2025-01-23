using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSmoothColorTransition : MonoBehaviour
{
    private Image image;

    [SerializeField]
    private List<Color> transitionList;

    [SerializeField]
    private float transitionLength;

    private IEnumerator tranCor;

    [SerializeField]
    private AnimationCurve transitionCurve;

    private Color lastColor;


    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    public void TransitionInstantTo(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < transitionList.Count && gameObject.activeInHierarchy)
        {
            if (tranCor != null)
            {
                StopCoroutine(tranCor);
            }
            //tranCor = TransitionToCor(transitionList[colorIndex]);
            //StartCoroutine(tranCor);
            if(image == null)
            {
                image = GetComponent<Image>();
            }
            image.color = transitionList[colorIndex];
        }
    }

    public void TransitionTo(int colorIndex)
    {
        //Check that index is in range
        if(colorIndex >= 0 && colorIndex < transitionList.Count && gameObject.activeInHierarchy)
        {
            if(tranCor != null)
            {
                StopCoroutine(tranCor);
            }
            tranCor = TransitionToCor(transitionList[colorIndex]);
            StartCoroutine(tranCor);
        }
    }

    private IEnumerator TransitionToCor(Color targetColor)
    {
        float counter = 0f;
        lastColor = image.color;

        Color startColor = lastColor;

        while (counter < transitionLength)
        {
            yield return null;
            counter += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, transitionCurve.Evaluate(counter / transitionLength));
        }
        tranCor = null;
        image.color = targetColor;
    }

    public void SetTransitionLength(float length)
    {
        transitionLength = length;
    }

    public void TransitionToLastColor()
    {
        if (tranCor != null)
        {
            StopCoroutine(tranCor);
        }
        tranCor = TransitionToCor(lastColor);
        StartCoroutine(tranCor);
    }

    public void SetToLast(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < transitionList.Count)
        {
            lastColor = transitionList[colorIndex];
        }
    }
}
