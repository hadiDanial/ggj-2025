using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    public KeyCode keyUp, keyRight, keyDown, keyLeft;

    [SerializeField] private float _force = 1f;

    [SerializeField] private float _topSpeed = 10f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            CheckAddForce(Vector3.up * (_force * -1f));
        }

        if (Input.GetKey(keyLeft))
        {
            CheckAddForce(Vector3.right * (_force * -1f));
        }
    

 
    }


    void CheckAddForce(Vector3 vec)
    {
        if (Mathf.Abs(rb2d.velocity.magnitude) < _topSpeed)
        {
            rb2d.AddForce(vec);
        }
    }

}
