using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafezoneCollider : MonoBehaviour
{

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.inSafeZone = true;
            hp.lastSafeZone = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth hp))
        {
            hp.inSafeZone = false;
            hp.lastSafeZone = transform;
        }
    }
}
