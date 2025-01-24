using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Safezone : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private List<Cleanable> cleanables;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private UnityEvent<Safezone> OnSafezoneTriggered;

    public bool IsUnlocked => isUnlocked;
    public Transform SpawnLocation => spawnLocation;
    private Collider2D safezoneTrigger;

    private void Awake()
    {
        safezoneTrigger = GetComponent<Collider2D>();
    }

    [ContextMenu("Clean")]
    private void CleanAll()
    {
        isUnlocked = true;
        foreach (var cleanable in cleanables)
        {
            cleanable.Clean();
        }
        OnSafezoneTriggered.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
