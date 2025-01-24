using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _force = 1f;
    [SerializeField] private float _topSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CheckAddForce(transform.up * _force);
        }

        if (Input.GetKey(KeyCode.A))
        {
            CheckAddForce(transform.right * (_force * -1.0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            CheckAddForce(transform.right * _force);
        }
    }

    void CheckAddForce(Vector3 vec)
    {
        if (Math.Abs(_rigidbody.velocity.x) < _topSpeed)
        {
            _rigidbody.AddForce(vec);
        }
    }
}