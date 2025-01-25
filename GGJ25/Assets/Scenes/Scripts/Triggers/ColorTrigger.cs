using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _Bubble;

    [SerializeField] private GameObject _BackGround;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag.Equals("TriggerAreaLeft"))
        {
            _BackGround.GetComponent<SpriteRenderer>().material.color = Color.grey;
        }
        else
        {
            _BackGround.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        }
    }
}