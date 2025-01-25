using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Animations;
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

    enum PLAYER_HP_STATES{
        natural,
        pop,
        respawn,
    }
    private PLAYER_HP_STATES state = PLAYER_HP_STATES.natural;

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
    [SerializeField] private Animator animator;


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
            ostManager.SetHealing(lifeTimer > lifeLength);

            //refill hp
            lifeTimer = Mathf.Clamp(lifeTimer - Time.deltaTime * lifeTimer / healLength, 0f, lifeLength);
            if(lifeTimer < 0.1f) lifeTimer = 0f;

        
            UpdateParticles();
        }
        else //if(state == PLAYER_HP_STATES.natural)
        {
            ostManager.SetOst(OstManager.OSTS.sad);
            lifeTimer = Mathf.Clamp(lifeTimer + Time.deltaTime, 0f, lifeLength);
            UpdateParticles();
            if(lifeTimer >= lifeLength)
            {
                animator.SetBool("dead",true);
                state = PLAYER_HP_STATES.pop;
            }
        }

        playerLight.pointLightOuterRadius = Mathf.Sin(Time.time * lightRadiusSpeed) * lightRadiusMultiplier + initialRadius;
    }

    private void Death()
    {
        //moved to respawn.
    }

    private void Respawn()
    {
        //Respawn
        transform.position = lastSafeZone.position;
        lifeTimer = 0f;

        if(lastCurve != null)
        {
            lastCurve.OnTriggerExit2D(GetComponent<Collider2D>());
        }
    }

    public void OnAnimationEnd(){
        Debug.Log("animation ended during " + state);
        switch(state){
            case PLAYER_HP_STATES.natural: return; break;

            case PLAYER_HP_STATES.pop: 
                animator.SetBool("dead",false);
                state = PLAYER_HP_STATES.respawn;
                Respawn();
            break;
            case PLAYER_HP_STATES.respawn: 
                // animator.SetBool("dead",false);
                state = PLAYER_HP_STATES.natural;
            break;
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
