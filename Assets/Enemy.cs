using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float timer;
    Rigidbody rb; 

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
        float force = 1000.0f;

        if (timer >= 1.0f)
        {
            int direction = Random.Range(1, 4);

            if (direction == 1)
            {
                rb.AddForce(Vector3.right * force);
            }
            else if (direction == 2)
            {
                rb.AddForce(Vector3.left * force);
            }
            else if (direction == 3)
            {
                rb.AddForce(Vector3.forward * force);
            }
            else if (direction == 4)
            {
                rb.AddForce(Vector3.back * force);
            }
            timer = 0.0f;
        }
    }
}
