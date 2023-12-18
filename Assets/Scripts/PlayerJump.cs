using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Slider powerBar;

    private float forceX, forceY;

    private float tresholdX = 7f;
    private float tresholdY = 14f;

    private bool setPower, didJump;

    private float powerBarTreshold = 10f;
    private float powerBarValue = 0f;

    private void Start()
    {
        powerBar.minValue = 0;
        powerBar.maxValue = 10f;
        powerBar.value = powerBarValue;
    }

    private void Update()
    {
        SetPower();

        //Debug.Log("velocity : " +rb.velocity.y);
    }

    public void OnSetPower(InputAction.CallbackContext context) {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                //Debug.Log("started");
                break;
            case InputActionPhase.Performed:
                //Debug.Log("performed");
                SetPower(true);
                break;
            case InputActionPhase.Canceled:
                //Debug.Log("cancel");
                SetPower(false);
                break;
        }   
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

            powerBarValue += powerBarTreshold * Time.deltaTime;
            powerBar.value = powerBarValue;
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

        anim.SetBool("Jump", didJump);

        powerBarValue = 0f;
        powerBar.value = powerBarValue;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (didJump) {
            didJump = false;

            anim.SetBool("Jump", didJump);

            if (other.TryGetComponent<Platform>(out Platform _platform))
            {
                if (_platform.canAddScore)
                {
                    _platform.canAddScore = false;

                    GameManager.Instance.AddScore(1);
                    GameManager.Instance.CreateNewPlatformAndLerp(other.transform.position.x);
                }
            }
        }

        
    }
}
