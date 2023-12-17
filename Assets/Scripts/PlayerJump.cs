using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float forceX, forceY;
    private float tresholdX = 7f;
    private float tresholdY = 14f;

    public bool setPower, didJump;

    private void Update()
    {
        SetPower();

        
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (didJump) {
            didJump = false;

            if (other.tag == "Platform") {
                Debug.Log("Landed on platform after jumping");
            }
        }
    }
}
