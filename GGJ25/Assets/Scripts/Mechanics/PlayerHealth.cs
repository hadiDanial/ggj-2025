using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float lifeLength = 12f;

    private float lifeTimer = 0f; 

    public bool inSafeZone = false;

    [SerializeField]
    private ParticleSystem system;

    private float maxEmission;

    //Set one of these on start
    public Transform lastSafeZone;

    public SpriteRenderer playerSpriteDisappear;

    public float healLength = 1f;

    void Start()
    {
        maxEmission = system.emission.rateOverTime.constantMax;

    }

    void Update()
    {
        if(inSafeZone)
        {
            lifeTimer = Mathf.Clamp(lifeTimer - Time.deltaTime * lifeTimer / healLength, 0f, lifeLength);
            UpdateParticles();
        }
        else
        {
            lifeTimer = Mathf.Clamp(lifeTimer + Time.deltaTime, 0f, lifeLength);
            UpdateParticles();
            if(lifeTimer >= lifeLength)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        //Pop animation? 
        //Respawn
        transform.position = lastSafeZone.position;
    }


    private void UpdateParticles()
    {
        var emit = system.emission.rateOverTime;
        emit.constantMax = maxEmission * (1f-(lifeTimer/lifeLength));
        var emitSys = system.emission;
        emitSys.rateOverTime = emit;

        var color = playerSpriteDisappear.color;
        color.a = 1f-(lifeTimer/lifeLength);
        playerSpriteDisappear.color = color;
    }
}
