using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    public bool turnOnGravity;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().useGravity = turnOnGravity;
            if(other.GetComponent<Player>() != null)
            {
                if (turnOnGravity)
                {
                    other.GetComponent<Player>().SetEnableGravity();
                }
                else
                {
                    other.GetComponent<Player>().SetDisableGravity();
                }
                
            }

        }
    }
}
