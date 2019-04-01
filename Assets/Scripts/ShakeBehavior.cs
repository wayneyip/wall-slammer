using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehavior : MonoBehaviour
{

  private Transform tf;
  private float shakeDuration = 0.0f;
  private float shakeMagnitude = 0.5f;
  private float dampingSpeed = 2.0f;
  public GameObject player;
  Vector3 initialPosition;

  // Use this for initialization
  void Start()
  {
  }

  private void Awake()
  {
    if (tf == null)
    {
      tf = GetComponent(typeof(Transform)) as Transform;
    }
  }

  private void OnEnable()
  {
    initialPosition = tf.localPosition;
  }

  // Update is called once per frame
  void Update()
  {
    if (shakeDuration > 0)
    {
      Vector2 rand = Random.insideUnitCircle;
      transform.localPosition = initialPosition + new Vector3(rand.x, rand.y) * shakeMagnitude;

      shakeDuration -= Time.deltaTime * dampingSpeed;
    }
    else
    {
      shakeDuration = 0.0f;
      tf.localPosition = initialPosition;
    }
  }

  public void TriggerShake()
  {
    shakeDuration = 0.3f;
  }
}
