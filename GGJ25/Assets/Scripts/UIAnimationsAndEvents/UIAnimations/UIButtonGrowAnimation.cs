using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(UIGrowAnimation))]
public class UIButtonGrowAnimation : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private UIGrowAnimation growAnim;

    private Button button;

    [SerializeField]
    private bool confirmInput; //Confirms Input that the user presses the left click button

    void Start()
    {
        if(growAnim == null)
        {
            growAnim = GetComponent<UIGrowAnimation>();
        }
        button = GetComponent<Button>();
    }
    /*
    void OnValidate()
    {
        if (growAnim == null)
        {
            growAnim = GetComponent<UIGrowAnimation>();
        }
    }
    */

    public void OnMouseEnter()
    {
        OnPointerEnter(null);
    }

    public void OnMouseExit()
    {
        OnPointerExit(null);
    }

    public void OnMouseDown()
    {
        OnPointerDown(null);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(button == null || button.interactable)
        {
            growAnim.Grow();
        }

    }

    public void OnPointerExit(PointerEventData data)
    {
        if (growAnim.changeSizeObj.transform.localScale != Vector3.one)
        {
            growAnim.Shrink();
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if ((button == null || button.interactable) && (!confirmInput || Input.GetMouseButtonDown(0)))
        {
            growAnim.Grow();
        }
        /*
        if (growAnim.transform.localScale != Vector3.one)
        {
            growAnim.Shrink();
        }
        */
    }

    private void OnDisable()
    {
        if(growAnim != null)
        {
            growAnim.Cancel();
        }
    }



    public void OnSelect(BaseEventData eventData)
    {
        if (button == null || button.interactable)
        {
            if (growAnim == null)
            {
                growAnim = GetComponent<UIGrowAnimation>();
            }
            growAnim.Grow();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (growAnim.changeSizeObj.transform.localScale != Vector3.one)
        {
            growAnim.Shrink();
        }
    }
}
