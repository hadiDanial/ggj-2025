using UnityEngine.Events;
using UnityEngine;


[RequireComponent(typeof(UIGrowAnimation))]
public class UIGrowAnimationEvents : MonoBehaviour
{
    public UnityEvent onGrowEvent;

    private void Start()
    {
        GetComponent<UIGrowAnimation>().SetEventOnGrow(onGrowEvent);
    }

}
