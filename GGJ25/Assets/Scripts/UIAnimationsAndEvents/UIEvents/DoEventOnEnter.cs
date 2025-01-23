using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoEventOnEnter : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private UnityEvent pointerEnterEvent;

    [SerializeField]
    private float cancelDelay = 0.0f;

    private float lastEnter;

    private IEnumerator cor;

    //private Button button;

    void Start()
    {
        //button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //if(button != null && !button.interactable)
        //{
        //    return;
        //}

        if(cancelDelay == 0f)
        {
            if(pointerEnterEvent != null)
            {
                pointerEnterEvent.Invoke();
            }
        }
        cor = DoEventDelayed();
        lastEnter = Time.time;
        StartCoroutine(cor);
        
    }


    public void OnPointerExit(PointerEventData data)
    {
        if(Time.time < lastEnter + cancelDelay)
        {
            StopCoroutine(cor);
        }
    }

    private IEnumerator DoEventDelayed()
    {
        float counter = 0;
        while(counter < cancelDelay)
        {
            yield return null;
            counter += Time.deltaTime;
        }
        if (pointerEnterEvent != null)
        {
            pointerEnterEvent.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnPointerEnter(null);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnPointerExit(null);
    }


}
