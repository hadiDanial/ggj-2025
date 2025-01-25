using System;
using UnityEngine;

namespace Mechanics
{
    public class BubbleCluster : MonoBehaviour
    {
        [Header("Cluster Settings")]
        [SerializeField] private int clusterSize = 5;
        
        [Header("Cluster Timers")]
        [SerializeField] private float timeToPop = 1.2f;
        [SerializeField] private float timeToRestore = 0.8f;
        [SerializeField] private LayerMask safezoneLayer;
        [SerializeField] private CircleCollider2D bubbleCollider;
        
        private float timer;
        private bool wasInSafezone;
        private int currentSize;
        private void Awake()
        {
            wasInSafezone = IsInSafezone();
            currentSize = clusterSize;
        }

        private bool IsInSafezone()
        {
            return Physics2D.OverlapCircle(transform.position, bubbleCollider.radius, safezoneLayer);    
        }

        private void Update()
        {
            bool isInSafeZone = IsInSafezone();
            if (isInSafeZone != wasInSafezone)
            {
                timer = 0;
            }
            timer += Time.deltaTime;
            if (isInSafeZone)
            {
                if(timer >= timeToRestore)
                {
                    RestoreBubble();
                    timer = 0;
                }
            }
            else
            {
                if (timer >= timeToPop)
                {
                    PopBubble();
                    timer = 0;
                }
            }
            wasInSafezone = isInSafeZone;
        }

        private void PopBubble()
        {
            Debug.Log("POP");
            currentSize--;
        }

        private void RestoreBubble()
        {
            Debug.Log("Restore");
            currentSize++;
        }

        private void OnDrawGizmos()
        {
            if(bubbleCollider == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, bubbleCollider.radius);
        }
    }
}