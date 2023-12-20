using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    public bool canAddScore = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeadZone") {
            Destroy(gameObject);
        }

        
    }

}
