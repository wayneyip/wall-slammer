using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  Rigidbody rb;
  float speed = 100.0f;
  bool isSliding = false;
  float delta = 0.1f;
  public ShakeBehavior shaker;
  AudioSource audioSource;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (! isSliding)
    {
      RaycastHit hit;
      if (Input.GetKey(KeyCode.A))
      {
        if (Physics.Raycast(transform.position, Vector3.left, out hit))
        {
          Vector3 translation = new Vector3(hit.transform.position.x + 1, transform.position.y, transform.position.z);

          if (Vector3.Distance(transform.position, translation) > delta)
          {
            StartCoroutine(Slide(translation));
          }
        }
      }

      else if (Input.GetKey(KeyCode.D))
      {
        if (Physics.Raycast(transform.position, Vector3.right, out hit))
        {
          Vector3 translation = new Vector3(hit.transform.position.x - 1, transform.position.y, transform.position.z);

          if (Vector3.Distance(transform.position, translation) > delta)
          {
            StartCoroutine(Slide(translation));
          }
        }
      }

      else if (Input.GetKey(KeyCode.W))
      {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
          Vector3 translation = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z - 1);

          if (Vector3.Distance(transform.position, translation) > delta)
          {
            StartCoroutine(Slide(translation));
          }
        }
      }

      else if (Input.GetKey(KeyCode.S))
      {
        if (Physics.Raycast(transform.position, Vector3.back, out hit))
        {
          Vector3 translation = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z + 1);

          if (Vector3.Distance(transform.position, translation) > delta)
          {
            StartCoroutine(Slide(translation));
          }
        }
      }
    }
  }

  IEnumerator Slide(Vector3 target)
  {
    isSliding = true;
    while (Vector3.Distance(transform.position, target) > 0)
    {
      transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
      yield return null;
    }
    shaker.TriggerShake();
    audioSource.PlayOneShot(audioSource.clip);
    isSliding = false;
  }
}
