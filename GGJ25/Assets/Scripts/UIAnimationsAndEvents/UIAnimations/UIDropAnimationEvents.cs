using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIDropAnimationEvents : UIDropAnimation
{
    [SerializeField]
    private UnityEvent eventBeforeRise, eventBeforeDrop, eventAfterRise, eventAfterDrop;

    public override void Drop()
    {
        eventBeforeDrop.Invoke();
        base.Drop();
    }

    public override void Rise()
    {
        eventBeforeRise.Invoke();
        base.Rise();
    }


    protected override IEnumerator MoveYCor()
    {
        yield return base.MoveYCor();
        if (target == 0)
        {
            eventAfterRise.Invoke();
        }
        else if (target == 1f)
        {
            eventAfterDrop.Invoke();
        }
    }
}
