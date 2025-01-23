using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class UIButtonClickEffect : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{


    public bool active = true;

    [Header("Delay on click:")]

    [SerializeField]
    private float clickDelay = 0f;

    [SerializeField]
    public int disableButtonsDepth = 0;

    [SerializeField]
    public bool redoGetButtons = false;



    private UnityEvent clickEvent;

    private Button button;

    private WaitForSeconds clickDelayWait;

    [Header("Blink effect:")]
    //Blinking might not work with an animator

    //Blink text bool
    [SerializeField]
    private bool blinkText;
    //Blink image bool
    [SerializeField]
    private bool blinkImage;
    //Blink curve? Repeat blink amounts?
    [SerializeField]
    private float blinkStep = 0.25f;
    [SerializeField]
    private int blinkTimes = 2; //Blink blink! Amounts of blinks per blinkLength

    private Text blinkTextComponent;

    private Image blinkImageComponent;

    private Button[] disableButtons;

    private IEnumerator textCor, imageCor;

    [SerializeField]
    private UnityEvent eventOnDelay;

    void Start()
    {
        button = GetComponent<Button>();
        clickDelayWait = new WaitForSeconds(clickDelay);
    }

    public void OnEnable()
    {
        //Reset on OnEnable
        if(clickEvent != null)
        {
            clickEvent.RemoveAllListeners();
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        if(!active)
        {
            return;
        }
        if(clickDelay > 0)
        {
            //clickEvent = button.onClick;
            button.enabled = false; //Possibly?
            
            StopCoroutine("DelayClick");
            StartCoroutine(DelayClick());
        }
        DoBlink();
    }

    public void ActivateEffect()
    {
        active = true;
        OnPointerDown(null);
    }

    public void OnPointerUp(PointerEventData data)
    {

    }

    public void OnPointerClick(PointerEventData data)
    {

    }

    //Coroutine:
    private IEnumerator DelayClick()
    {
        if(eventOnDelay != null)
        {
            eventOnDelay.Invoke();
        }

        //Disable "brother buttons"
        if (disableButtonsDepth > 0)
        {
            SetBrotherButtons(false);
        }
        if(clickDelayWait == null)
        {
            clickDelayWait = new WaitForSeconds(clickDelay); 
        }
        yield return clickDelayWait;

        button.enabled = true;
        button.onClick.Invoke();
        //Debug.Log("Return event");


        //Enable parents
        if (disableButtonsDepth > 0)
        {
            SetBrotherButtons(true);
        }
        clickDelayWait = null;
    }

    private void SetBrotherButtons(bool setButtonsEnabled)
    {
        if(disableButtons == null || (setButtonsEnabled && redoGetButtons))
        {
            Transform tran = GetParentAtIndex();
            //Debug.Log("Parent tran: " + tran.name);
            disableButtons = tran.GetComponentsInChildren<Button>(true);
        }
        //Debug.Log("Disable buttons: " + disableButtons.Length+ " / Set buttons: "+ setButtonsEnabled.ToString());
        foreach(Button setButton in disableButtons)
        {
            if(setButton != button)
            {
                //setButton.interactable = setButtonsEnabled;
                setButton.enabled = setButtonsEnabled;
                //Debug.Log("Disabling: " + setButton.gameObject.name.ToString()+" button: "+ setButton.enabled.ToString());
            }
        }
    }

    private Transform GetParentAtIndex()
    {
        int counter = 0;
        Transform checkTransform = transform;
        while(counter < disableButtonsDepth && checkTransform.parent != null)
        {
            checkTransform = checkTransform.parent;
            counter++;
        }
        return checkTransform;
    }

    [ContextMenu("Blink", false)]
    public void DoBlink()
    {
        if (blinkText)
        {
            if(blinkTextComponent == null)
            {
                blinkTextComponent = GetComponentInChildren<Text>();
            }
            if(textCor == null)
            {
                textCor = BlinkCorText();
                StartCoroutine(textCor);
            }
        }
        if (blinkImage)
        {
            if (blinkImageComponent == null)
            {
                blinkImageComponent = GetComponent<Image>();
            }
            if(imageCor == null)
            {
                imageCor = BlinkCorImage();
                StartCoroutine(imageCor);
            }
        }

    }

    private IEnumerator BlinkCorText()
        //Maybe use sine function to interploate between these? Could be linear too
    {
        //Debug.Log("Blink text");
        if(blinkTimes > 0)
        {
            //Debug.Log("Blink text 2");
            Color positiveCol = new Color(blinkTextComponent.color.r, blinkTextComponent.color.g, blinkTextComponent.color.b, 1f);
            Color negativeCol = new Color(blinkTextComponent.color.r, blinkTextComponent.color.g, blinkTextComponent.color.b, 0f);
            int blinkCounter = 0;
            float subCounter = 0;
            bool descend = true;
            while(blinkCounter < blinkTimes * 2)
            {
                yield return null;
                subCounter += Time.deltaTime;
                //Debug.Log("- Subcounter: " + subCounter);
                if (descend)
                {
                    blinkTextComponent.color = Color.Lerp(positiveCol, negativeCol, subCounter / blinkStep);
                }
                else
                {
                    blinkTextComponent.color = Color.Lerp(negativeCol, positiveCol, subCounter / blinkStep);
                }
                if (subCounter >= blinkStep)
                {
                    blinkCounter++;
                    descend = !descend;
                    subCounter = 0;
                    //Debug.Log("* Counter: "+blinkCounter+" descend: "+descend.ToString());
                }
            }
            blinkTextComponent.color = positiveCol;
        }
        imageCor = null;
    }

    private IEnumerator BlinkCorImage()
    //Maybe use sine function to interploate between these? Could be linear too
    {
        if (blinkTimes > 0)
        {
            Color positiveCol = new Color(blinkImageComponent.color.r, blinkImageComponent.color.g, blinkImageComponent.color.b, 1f);
            Color negativeCol = new Color(blinkImageComponent.color.r, blinkImageComponent.color.g, blinkImageComponent.color.b, 0f);
            int blinkCounter = 0;
            float subCounter = 0;
            bool descend = true;
            while (blinkCounter < blinkTimes * 2)
            {
                yield return null;
                subCounter += Time.deltaTime;
                if (descend)
                {
                    blinkImageComponent.color = Color.Lerp(positiveCol, negativeCol, subCounter / blinkStep);
                }
                else
                {
                    blinkImageComponent.color = Color.Lerp(negativeCol, positiveCol, subCounter / blinkStep);
                }
                if (subCounter >= blinkStep)
                {
                    blinkCounter++;
                    descend = !descend;
                }
            }
            blinkImageComponent.color = positiveCol;
        }
        textCor = null;
    }
}
