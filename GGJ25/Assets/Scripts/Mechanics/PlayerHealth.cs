using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    public float lifeLength = 12f;

    public float lifeTimer = 0f; 

    public bool inSafeZone = false;

    [SerializeField] private ParticleSystem system;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float lightRadiusMultiplier = 2f;
    [SerializeField] private float lightRadiusSpeed = 5f;

    private float maxEmission;

    //Set one of these on start
    public Transform lastSafeZone;

    public SpriteRenderer playerSpriteDisappear;

    public float healLength = 1f;

    public FixableObjectCurve lastCurve;
    private float initialRadius;

    private void Awake()
    {
        initialRadius = playerLight.pointLightOuterRadius;
    }

    [SerializeField] OstManager ostManager;
    [SerializeField] SfxManager sfxManager;

    void Start()
    {
        maxEmission = system.emission.rateOverTime.constantMax;
    }

    void Update()
    {
        if(inSafeZone)
        {
            //ost
            ostManager.SetOst(OstManager.OSTS.happy);
            ostManager.SetHealing(lifeTimer > 0);

            //refill hp
            lifeTimer = Mathf.Clamp(lifeTimer - Time.deltaTime * lifeTimer / healLength, 0f, lifeLength);
            if(lifeTimer < 0.1f) lifeTimer = 0f;

        
            UpdateParticles();
        }
        else
        {
            ostManager.SetOst(OstManager.OSTS.sad);
            lifeTimer = Mathf.Clamp(lifeTimer + Time.deltaTime, 0f, lifeLength);
            UpdateParticles();
            if(lifeTimer >= lifeLength)
            {
                Death();
            }
        }

        playerLight.pointLightOuterRadius = Mathf.Sin(Time.time * lightRadiusSpeed) * lightRadiusMultiplier + initialRadius;
    }

    private void Death()
    {
        //Pop animation? 
        //Respawn
        transform.position = lastSafeZone.position;

        if(lastCurve != null)
        {
            lastCurve.OnTriggerExit2D(GetComponent<Collider2D>());
        }
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
