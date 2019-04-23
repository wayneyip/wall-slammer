using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float attackInterval;
    public float activationTime;
    public float moveSpeed;

    public AttackArea[] attackAreas;
    public Vector3 attackDirection;
    private bool playerDetectionEnabled;

    private float activationTimer;
    private float attackTimer;

    public IEnumerator slideCoroutine;
    public IEnumerator activationCoroutine;

    public bool isSliding;
    public bool isActivating;

    private Color initialColor;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool bActive;

    private Rigidbody rb;
    private MeshRenderer mr;

  
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
        GameManager.instance.OnGameStart.AddListener(_OnGameStart);

        rb = GetComponent<Rigidbody>();

        mr = GetComponent<MeshRenderer>();

        initialColor = mr.material.color;

        foreach (AttackArea aa in attackAreas)
        {
            aa.OnTriggered.AddListener(() => OnPlayerDetected(aa.attackDirection, aa));
        }

        playerDetectionEnabled = false;

    }


    private void Update()
    {

    }

    private void EnablePlayerDetection()
    {
        foreach (AttackArea aa in attackAreas)
        {
            aa.isDetecting = true;
            aa.isStaying = false;
            aa.selected = false;
        }

        playerDetectionEnabled = true;
    }

    private void DisablePlayerDetection()
    {
        foreach (AttackArea aa in attackAreas)
        {
            aa.isDetecting = false;
            aa.isStaying = false;
            aa.selected = false;
        }

        playerDetectionEnabled = false;
    }

    private void OnPlayerDetected(Vector3 direction, AttackArea aa)
    {
        Debug.Log("OnPlayerDetected()");

        foreach (AttackArea a in attackAreas)
        {
            a.selected = false;
        }

        aa.selected = true;

        attackDirection = direction;

        if (!isActivating)
        {
            OnBeginActivation();
        }
    }

    private void OnBeginActivation()
    {
        Debug.Log("OnBeginActivation()");

        isActivating = true;

        activationCoroutine = _Activate();
        StartCoroutine(activationCoroutine);

    }

    private void OnEndActivation()
    {
        Debug.Log("OnEndActivation()");

        isActivating = false;

        DisablePlayerDetection();

        OnBeginAttack();
    }

    private void OnBeginAttack()
    {
        Debug.Log("Begin Attack");

      

        if (Physics.Raycast(transform.position, attackDirection, out RaycastHit hit, Mathf.Infinity, GameManager.instance.wallLayer.value, QueryTriggerInteraction.Ignore))
        {

            Vector3 translation = hit.point + hit.normal;

            if (Vector3.Distance(transform.position, translation) > 0.01f)
            {

                slideCoroutine = _Slide(translation, attackDirection);
                StartCoroutine(slideCoroutine);
            }
            else
            {
                OnEndAttack();
            }
        }
        else
        {
            OnEndAttack();
        }
    }

    public void OnEndAttack()
    {

        Debug.Log("Attack Finished");

        mr.material.color = initialColor;
        rb.velocity = Vector3.zero;

        if (Player.isGravitational)
        {
          rb.useGravity = true;
        }
        EnablePlayerDetection();
    }

    IEnumerator _Activate()
    {
        float activationVal = 0.0f;

        while(activationVal < 1.0f)
        {
            activationVal += Time.deltaTime;

            if(activationVal > 1.0f)
            {
                activationVal = 1.0f;
            }

            Color c = mr.material.color;
            Color.RGBToHSV(c, out float h, out float s, out float v);
            s = Mathf.Lerp(0.0f, 1.0f, activationVal);

            mr.material.color = Color.HSVToRGB(h, s, v);

            if (GameManager.instance.isGameOver)
            {
                yield break;
            }

            yield return null;
        }

        OnEndActivation();
    }

    IEnumerator _Slide(Vector3 target, Vector3 slideDir)
    {

        isSliding = true;
        float speed = moveSpeed;

        if (Player.isGravitational)
        {
          rb.useGravity = false;
        }
        while (Vector3.Distance(transform.position, target) > 0 )
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

            if (GameManager.instance.isGameOver)
            {
                yield break;
            }

            yield return null;
        }

        GameManager.instance.PlayCollisionEffect();

        isSliding = false;

        OnEndAttack();
    }


    private void _OnGameStart()
    {
        activationTimer = activationTime;
        attackTimer = attackInterval;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        mr.material.color = initialColor;

        rb.useGravity = Player.isGravitational;
        rb.velocity = Vector3.zero;

        bActive = true;

        isActivating = false;
        isSliding = false;

        EnablePlayerDetection();
    }

    private void _OnGameOver()
    {
        bActive = false;
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.instance.playerGO && !Player.witchTime)
        {
            GameManager.instance.OnGameOver.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == GameManager.instance.playerGO && !Player.witchTime)
        {
            GameManager.instance.OnGameOver.Invoke();
        }
    }

}
