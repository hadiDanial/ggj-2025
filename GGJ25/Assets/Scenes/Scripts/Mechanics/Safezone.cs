using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider2D))]
public class Safezone : Cleanable
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private List<Cleanable> cleanables;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private UnityEvent<Safezone> onSafezoneTriggered;

    public bool IsUnlocked => isUnlocked;
    public Transform SpawnLocation => spawnLocation;
    private Collider2D safezoneTrigger;

    private void Awake()
    {
        safezoneTrigger = GetComponent<Collider2D>();
        safezoneTrigger.isTrigger = true;
    }

    [ContextMenu("Clean")]
    public override void Clean()
    {
        isUnlocked = true;
        foreach (var cleanable in cleanables)
        {
            cleanable.Clean();
        }
    }

    public override void ResetCleanable()
    {
        isUnlocked = false;
        foreach (var cleanable in cleanables)
        {
            cleanable.ResetCleanable();
        }
    }
    
    public void TriggerSafezone()
    {
        if (!isUnlocked)
        {
            onSafezoneTriggered.Invoke(this);
            Clean();
        }
    }
}
