﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [Range(1, 2)]
    public int initialSize;
    private int size;
    public Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool bActive;

    public static bool witchTime;
    public static float witchTimer;
    public static bool inWitchTime; 
    
    public bool allowWitchTime;

    public bool useGravity;

    public static bool isGravitational;
    public bool allowGravity;

    public bool allowDoubleMove;
    public bool hasDoubleMoved;

    private bool bIsDiagonal;

    private bool dead; 

    Rigidbody rb;
    float speed = 100.0f;
    public bool isSliding = false;
    float delta = 0.1f;
    

    //TODO: Fix implementation of getting canvas
    public Canvas witchCanvas;

    private IEnumerator slideCoroutine;


    private void Awake()
    {
        isGravitational = allowGravity;

        bIsDiagonal = transform.rotation.eulerAngles.y == 45;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialSize = (int)transform.lossyScale.x;

        hasDoubleMoved = false;

        useGravity = allowGravity;

        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
        GameManager.instance.OnGameStart.AddListener(_OnGameStart);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!bActive)
            return;

        if(witchTime)
        {
            witchTimer -= Time.deltaTime;
            inWitchTime = true; 

            if(witchTimer <= 0.0f)
            {
                witchTime = false;
                witchTimer = 0.0f;
                witchCanvas.gameObject.SetActive(false);  
            }
        }
        else
        {
            inWitchTime = false; 
        }

        Slide();
    }

    private void Slide()
    {
        if((!allowDoubleMove && isSliding) || (allowDoubleMove && hasDoubleMoved))
        {
            return;
        }

        RaycastHit hit = new RaycastHit();
        Vector3 slideDir = new Vector3(0, 0, 0);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slideDir = -transform.right;
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            slideDir = transform.right;
        }

        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            slideDir = transform.forward;
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            slideDir = -transform.forward;
        }

        LayerMask mask = GameManager.instance.wallLayer.value | GameManager.instance.breakableLayer | GameManager.instance.witchLayer; 
       
        if(allowWitchTime)
        {
            mask = mask | GameManager.instance.enemyLayer.value; 
        }

        if (slideDir.magnitude > 0 && Physics.Raycast(transform.position, slideDir, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
        {

            bool bSwitchOrientation = Mathf.Abs(Vector3.Dot(hit.normal, slideDir)) > 0.9 ? false : true;

            Vector3 translation = hit.point + hit.normal * 0.5f;

            if (Vector3.Distance(transform.position, translation) > delta)
            {
                if (isSliding)
                {
                    hasDoubleMoved = true;
                    StopSliding();
                }

                slideCoroutine = _Slide(translation, bSwitchOrientation, slideDir);
                StartCoroutine(slideCoroutine);
            }
        }
    }

    private void StopSliding()
    {
        StopCoroutine(slideCoroutine);
    }

    IEnumerator _Slide(Vector3 target, bool bSwitchOrientation, Vector3 slideDir)
    {
        if (allowGravity)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
        isSliding = true;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = bSwitchOrientation && transform.rotation.eulerAngles.y == 0 ? Quaternion.Euler(new Vector3(0, 45, 0)) : Quaternion.identity;
        float slerpVal = bSwitchOrientation ? 0 : 1;
        float slerpRate = 10.0f;

        speed = allowDoubleMove && !hasDoubleMoved ? 25.0f : 100.0f;

        while (Vector3.Distance(transform.position, target) > 0 || slerpVal < 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

            if (bSwitchOrientation)
            {
                slerpVal += Time.deltaTime * slerpRate;

                if (slerpVal > 1)
                {
                    slerpVal = 1;
                    transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, slerpVal);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, slerpVal);
                }
            }

            if (GameManager.instance.isGameOver)
            {
                yield break;
            }

            yield return null;
        }

        GameManager.instance.PlayCollisionEffect();
        isSliding = false;

        if (allowGravity)
        {
          rb.velocity = Vector3.zero;
          rb.AddForce(-slideDir * 75.0f);
          rb.useGravity = true;
        }

        hasDoubleMoved = false;
    }

    private void _OnGameStart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        bActive = true;
        isSliding = false;
        hasDoubleMoved = false;
        witchTime = false;
        witchTimer = 0.0f;
        dead = false;
        GetComponent<TrailRenderer>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<ParticleSystem>().gravityModifier = 0;
        GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        isGravitational = useGravity;
        allowGravity = useGravity;
        rb.useGravity = useGravity;
        rb.velocity = Vector3.zero;

        if (witchCanvas != null)
        {
            witchCanvas.gameObject.SetActive(false);
        }

        size = initialSize;
    }

    private void _OnGameOver()
    {
        bActive = false;
        rb.velocity = Vector3.zero;
        if (GetComponent<MeshRenderer>().enabled)
        {
            if (isGravitational)
            {
               GetComponent<ParticleSystem>().gravityModifier = 1;
            }
            GetComponent<ParticleSystem>().Play();
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
    }

    private void UpdateSizeVisual()
    {
        Utilities.SetGlobalScale(transform, Vector3.one * GameManager.instance.unitLength * size);
    }


    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);

        if(collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.enemyLayer.value, 2) && !witchTime)
        {
            dead = true; 
            GameManager.instance.OnGameOver.Invoke();
        }

        if (collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.witchLayer.value, 2) && !witchTime)
        {
            witchTime = true && allowWitchTime;
            witchTimer = 4.0f;
            if (witchCanvas != null)
                witchCanvas.gameObject.SetActive(true);
        }

        if (collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.spikeLayer.value, 2))
        {
            dead = true; 
            GameManager.instance.OnGameOver.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.enemyLayer.value, 2) && !witchTime)
        {
            dead = true;
            GameManager.instance.OnGameOver.Invoke();
        }

        if (collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.spikeLayer.value, 2))
        {
            dead = true;
            GameManager.instance.OnGameOver.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Witch")
    {
      if (isSliding && !witchTime && !dead)
      {
        witchTime = true && allowWitchTime;
        witchTimer = 4.0f;
        if (witchCanvas != null)
          witchCanvas.gameObject.SetActive(true);
      }
    }

    }

    public void SetEnableGravity()
    {
        isGravitational = true;
        allowGravity = true;
    }

    public void SetDisableGravity()
    {
        isGravitational = false;
        allowGravity = false;
    }

    public bool isWitchTime()
    {
        return witchTime && !inWitchTime; 
    }
}
