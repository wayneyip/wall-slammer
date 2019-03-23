using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float timer;
    Rigidbody rb;
    float force = 2000.0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        rb.AddForce(Vector3.back * (50f)); 

        if(timer >= 0.5f)
        {
            timer = 0.0f;

            if (Input.GetKey(KeyCode.A)) {
                rb.AddForce(Vector3.left * force);
            }
            else if (Input.GetKey(KeyCode.D)) {
                rb.AddForce(Vector3.right * force);
            }
            else if (Input.GetKey(KeyCode.W)) {
                rb.AddForce(Vector3.forward * force);
            }
            else if (Input.GetKey(KeyCode.S)) {
                rb.AddForce(Vector3.back * force);
            }
        }
    }
}
