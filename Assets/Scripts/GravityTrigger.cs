using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().useGravity = true;
            if(other.GetComponent<Player>() != null)
            {
                other.GetComponent<Player>().SetAllowGravity();
            }
        }
    }
}
