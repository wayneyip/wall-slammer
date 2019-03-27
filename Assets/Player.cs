using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    float force = 2000.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
           if (Input.GetKey(KeyCode.A)) {
            if(Physics.Raycast(transform.position, Vector3.left, out hit))
            {
                Vector3 translation = new Vector3(hit.transform.position.x + 1, transform.position.y, transform.position.z);

                transform.position = translation;
            }
            }

            else if (Input.GetKey(KeyCode.D)) {
            if (Physics.Raycast(transform.position, Vector3.right, out hit))
            {
                Vector3 translation = new Vector3(hit.transform.position.x - 1, transform.position.y, transform.position.z);

                transform.position = translation;
            }
        }

            else if (Input.GetKey(KeyCode.W)) {
            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                Vector3 translation = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z - 1);

                transform.position = translation;
            }
        }

            else if (Input.GetKey(KeyCode.S)) {
            if (Physics.Raycast(transform.position, Vector3.back, out hit))
            {
                Vector3 translation = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z + 1);

                transform.position = translation;
            }
        }
          
    }
}
