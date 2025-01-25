using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    public KeyCode keyUp, keyRight, keyDown, keyLeft;


    [SerializeField] private float _force = 1f;

    [SerializeField] private float _topSpeed = 10f;

    [SerializeField] private float floatDownForce = 0.1f;
    [SerializeField] private OstManager ostManager;
    [SerializeField] private SfxManager sfxManager;
    [SerializeField] private float collideSfxMinMagnitude = 0.5f;

    private Rigidbody2D rb2d;

    private bool playerMove; 

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        playerMove = false;

        if (Input.GetKey(keyUp))
        {
            CheckAddForce(Vector3.up * (_force));
        }

        if (Input.GetKey(keyRight))
        {
            CheckAddForce(Vector3.right * (_force));
        }

        if (Input.GetKey(keyDown))
        {
            CheckAddForce(Vector3.down * (_force));
        }

        if (Input.GetKey(keyLeft))
        {
            CheckAddForce(Vector3.left * (_force));
        }
    
        if(!playerMove)
        {
            CheckAddForce(Vector3.down * (floatDownForce));
        }

        //update rain ost
        Transform nearestWindow = FindNearestObjectWithTag("Window").transform;
        ostManager.updateDistanceToWindow(Vector3.Distance(transform.position,nearestWindow.position));
 
    }


    void CheckAddForce(Vector3 vec)
    {
        if (Mathf.Abs(rb2d.velocity.magnitude) < _topSpeed)
        {
            rb2d.AddForce(vec, ForceMode2D.Force);
        }

        //playerMove = true;
    }

    //collision sfx
    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.transform.tag == "wall" && rb2d.velocity.magnitude >= collideSfxMinMagnitude) 
            sfxManager.PlaySound(SfxManager.SFX.collide);
    }

    //sorry :3
    GameObject FindNearestObjectWithTag(string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in taggedObjects)
        {
            float distance = Vector3.Distance(obj.transform.position, currentPosition);
            if (distance < minDistance)
            {
                nearestObject = obj;
                minDistance = distance;
            }
        }

        return nearestObject;
    }

}
