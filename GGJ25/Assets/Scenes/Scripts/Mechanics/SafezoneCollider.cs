using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafezoneCollider : MonoBehaviour
{
    public Transform overrideSpawn;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.inSafeZone = true;
            hp.lastSafeZone = transform;
            if(overrideSpawn != null)
            {
                hp.lastSafeZone = overrideSpawn;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.inSafeZone = false;
            hp.lastSafeZone = transform;
                        if(overrideSpawn != null)
            {
                hp.lastSafeZone = overrideSpawn;
            }
        }
    }
}
