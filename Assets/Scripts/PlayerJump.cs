using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Physics Component")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject landedOnPlatform;
    [SerializeField] private float forceX, forceY;

    private float tresholdX = 7f;
    private float tresholdY = 14f;

    [Header("Animation")]
    [SerializeField] private Animator anim;
    [SerializeField] private bool setPower, didJump, alreadyJump;

    [Header("Input Manager")]
    [SerializeField] private PlayerInput playerInput;

    [Header("HUD")]
    [SerializeField] private Slider powerBar;
    private float powerBarTreshold = 10f;
    private float powerBarValue = 0f;

    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;

    
    [Header("Events")]
    public UnityEvent OnJump;
    public UnityEvent OnSetPowerJump;
    public UnityEvent OnLanded;

    private void Start()
    {
        InitializeJumpBar();
    }

    private void InitializeJumpBar()
    {
        powerBar.minValue = 0;
        powerBar.maxValue = 10f;
        powerBar.value = powerBarValue;
    }

    private void Update()
    {
        
        SetPower();
    }

    

    private void SetPower() {
        if (setPower) {
            forceX += tresholdX * Time.deltaTime;
            forceY += tresholdY * Time.deltaTime;

            if (forceX > 6.5f) {
                forceX = 6.5f;
            }

            if (forceY > 13.5f)
            {
                forceY = 13.5f;
            }

            OnSetPowerJump?.Invoke();
        }
    }
    public void SetPower(bool setPower) {
        this.setPower = setPower;

        if (!setPower) {
            Jump();
        }

    }


    private void Jump()
    {
        rb.velocity = new Vector3(forceX, forceY, 0);
        forceX = forceY = 0f;

        didJump = true;

        OnJump?.Invoke();

        powerBarValue = 0f;
        powerBar.value = powerBarValue;
        
    }

    public void EventAnimation() {
        anim.SetBool("Jump", didJump);
    }

    public void EventSound(AudioClip clip) {
        audioSource.clip = null;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void EventUpdateJumpBar() {
        powerBarValue += powerBarTreshold * Time.deltaTime;
        powerBar.value = powerBarValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (didJump) {
            didJump = false;

            OnLanded?.Invoke();

            if (other.TryGetComponent<Platform>(out Platform _platform))
            {
                if (_platform.canAddScore)
                {
                    _platform.canAddScore = false;
                    

                    GameManager.Instance.AddScore(1);
                    GameManager.Instance.CreateNewPlatformAndLerp(other.transform.position.x);
                }

                landedOnPlatform = null;
                landedOnPlatform = _platform.gameObject;
                //landedOnPlatform.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        

        if (other.tag == "DeadZone")
        {
            Destroy(gameObject);
            GameManager.Instance.GameOver();
        }


    }

    
}
