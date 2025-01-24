using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : Cleanable
{
    [SerializeField] private Transform mask;
    [SerializeField, Range(0.5f, 20f)] private float maskScale = 5f;
    [SerializeField, Range(0.5f, 10f)] private float duration = 4f;
    [SerializeField] private Ease ease = Ease.InExpo;

    private Vector3 maskEndScale;
    private Tween scaleTween;

    private bool isClean;
    private float debugSphereSize => maskScale * 0.5f;

    private void Awake()
    {
        if (mask == null) mask = transform;
        ResetCleanable();
        maskEndScale = Vector3.one * maskScale;
    }

    [ContextMenu("Clean")]
    public override void Clean()
    {
        KillTween();
        scaleTween = mask.DOScale(maskEndScale, duration).SetEase(ease);
    }

    [ContextMenu("Reset Clean")]
    public override void ResetCleanable()
    {
        IsClean = false;
        KillTween();
        mask.localScale = Vector3.zero;
    }


    private void KillTween()
    {
        if (scaleTween != null)
        {
            scaleTween.Kill();
            scaleTween = null;
        }
    }

    private void OnDestroy() => KillTween();
    private void OnDisable() => KillTween();

    private void OnDrawGizmos()
    {
        if (mask == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mask.position, debugSphereSize);    
    }
    
}
