using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;
using System.Threading;

public class Player : MonoBehaviour
{
    
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] AudioClip[] listDeathSounds;
    [SerializeField] Animator dustAnimator;
    [SerializeField] AudioClip landingSFX;
    [SerializeField] AudioClip firstStep;
    [SerializeField] AudioClip secondStep;

    private float ShakeAmplitude = 1.2f;     //Cinemachine Noise Profile Parameter
    private float ShakeFrequency = 2.0f;     //Cinemachine Noise Profile Parameter

    public float LandingShakeDuration = 0.3f;      //Time the Camera Shake effect will last
    public float LandingShakeAmplitude = 1.2f;     //Cinemachine Noise Profile Parameter
    public float LandingShakeFrequency = 2.0f;     //Cinemachine Noise Profile Parameter

    public float runShakeAmplitude;
    public float runShakeFrequency;
    public float runShakeDuration;

    public float ShakeElapsedTime = 0f;

    //Cinemachine Shake Virtual Camera Run
    public CinemachineVirtualCamera VirtualCameraRun;
    private CinemachineBasicMultiChannelPerlin runVirtualCameraNoise;

    //Cinemachine Shake Virtual Camera Idle
    public CinemachineVirtualCamera VirtualCameraIdle;
    private CinemachineBasicMultiChannelPerlin idleVirtualCameraNoise;


    Animator myAnimator;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCapsuleCollider2D;
    BoxCollider2D myBoxCollider2D;
    float gravityAtStart;
    float moveSpeed = 6f;
    int health;
    private float timer;
    private float timer1;
    private float timer2;


    public bool stop = false;

    //Boolean
    bool inTheAir = false;
    bool isAlive = true;
    bool isOnALadder = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        gravityAtStart = myRigidBody.gravityScale / 2;

        //Get Virtual Camera Noise Profile
        if (VirtualCameraRun != null)
        {
            runVirtualCameraNoise = VirtualCameraRun.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }

        if(VirtualCameraIdle != null)
        {
            idleVirtualCameraNoise = VirtualCameraIdle.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        if (stop)
        {
            myAnimator.SetBool("PlayerStop", true);
            dustAnimator.SetBool("PlayerRunning", false);
            myRigidBody.velocity = new Vector2(0, 0);
            ShakeElapsedTime = 0f;
            CameraShake();
            return;
        }
        myAnimator.SetBool("PlayerStop", false);

        Move();
        Jump();
        Flying();
        FlipSprite();
        ClimbLadder();
        Die();
        Interact();
        CameraShake();
    }

    public bool Interact()
    {
        return Input.GetButtonDown("Interact");
    }

    public void SpeakingCameraTriggered(bool trigger)
    {
        myAnimator.SetBool("SentenceTrigger", trigger);
    }

    private void Flying()
    {
        if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && !isOnALadder)
        {
            inTheAir = true;
            timer1 = Time.time;
        }
        else
        {
            inTheAir = false;
            timer2 = Time.time;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inTheAir && !myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            timer = timer1 - timer2;
            Debug.Log(timer);
            if (timer > 1.1f) { timer = 5f; }
            else if (timer > 0.8f) { timer = 1f; }
            else if (timer > 0.5f) { timer = 0.5f; }
            else { timer = 0.2f; }
            dustAnimator.SetTrigger("Landing");
            AudioSource.PlayClipAtPoint(landingSFX, Camera.main.transform.position);
            TriggerLandingCameraShake();
        }
        if(!myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && isOnALadder)
        {
            timer = .4f;
            dustAnimator.SetTrigger("Landing");
            AudioSource.PlayClipAtPoint(landingSFX, Camera.main.transform.position);
            TriggerLandingCameraShake();
            
        }
    }
    private void Die()
    {
        if(myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")))
        {
            health = FindObjectOfType<GameSession>().GetLives();
            StartCoroutine(DeathAnimation(health));
            isAlive = false;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            myAnimator.SetTrigger("Dying");
            if(!GetComponent<AudioSource>().mute)
            {
                AudioSource.PlayClipAtPoint(listDeathSounds[UnityEngine.Random.Range(0, listDeathSounds.Length - 1)], Camera.main.transform.position);
            }
            
        }
    }

    IEnumerator DeathAnimation(int health)
    {
        if(health < 1)
        {
            Time.timeScale = 0.2f;
        }
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1f;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }


    private void CameraShake()
    {
        //For Virtual Camera Running

        //If the Cinemachine component is not set, avoid update
        if(VirtualCameraRun != null && runVirtualCameraNoise != null)
        {
            //If Camera Shake effect is still playing
            if(ShakeElapsedTime > 0)
            {
                //Set Cinemachine Camera Noise Parameter
                runVirtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                runVirtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                //Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                //If Camera Shake effect is over, reset variables
                runVirtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
                timer = 0;
            }
        }

        //For Virtual Camera Idle

        //If the Cinemachine component is not set, avoid update
        if (VirtualCameraIdle != null && idleVirtualCameraNoise != null)
        {
            //If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                //Set Cinemachine Camera Noise Parameter
                idleVirtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                idleVirtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                //Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                //If Camera Shake effect is over, reset variables
                idleVirtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
                timer = 0;
            }
        }
    }

    private void TriggerRunCameraShake()
    {
        //Trigger by animation
        ShakeElapsedTime = runShakeDuration;
        ShakeAmplitude = runShakeAmplitude;
        ShakeFrequency = runShakeFrequency;
    }

    private void TriggerLandingCameraShake()
    {
        ShakeElapsedTime = LandingShakeDuration;
        ShakeAmplitude = LandingShakeAmplitude * timer;
        ShakeFrequency = LandingShakeFrequency * timer;
    }

    private void Move()
    {
        float controlThrow = Input.GetAxisRaw("Horizontal"); // value between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
        if(myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            dustAnimator.SetBool("PlayerRunning", playerHasHorizontalSpeed); 
        }
        else { dustAnimator.SetBool("PlayerRunning", false); }
        
    }

    private void FristStep()
    {
        if(!inTheAir && !GetComponent<AudioSource>().mute)
        {
            AudioSource.PlayClipAtPoint(firstStep, Camera.main.transform.position);
        }
        
    }

    private void SecondStep()
    {
        if(!inTheAir && !GetComponent<AudioSource>().mute)
        {
            AudioSource.PlayClipAtPoint(secondStep, Camera.main.transform.position);
        }
    }

    private void Jump()
    {
        if(inTheAir) { return; }
        if (isOnALadder) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
            myAnimator.SetTrigger("Jumping");
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        isOnALadder = myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder"));

        if (!isOnALadder) 
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityAtStart;
            myAnimator.SetBool("On Ladder", false);
            return; 
        }

        float controlThrow = Input.GetAxisRaw("Vertical"); // value between -1 and +1
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * moveSpeed);
        myRigidBody.velocity = playerVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
      
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
        myRigidBody.gravityScale = 0f;
        myAnimator.SetBool("On Ladder", isOnALadder);
    }
}
