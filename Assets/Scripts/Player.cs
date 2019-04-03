using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Range(1, 2)]
    public int initialSize;
    private int size;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool bActive;

    public static bool witchTime;
    public static float witchTimer; 

    private bool bIsDiagonal;

    Rigidbody rb;
    float speed = 100.0f;
    bool isSliding = false;
    float delta = 0.1f;
    public ShakeBehavior shaker;
    AudioSource audioSource;


    private void Awake()
    {

        bIsDiagonal = transform.rotation.eulerAngles.y == 45;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialSize = (int)transform.lossyScale.x;

        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
        GameManager.instance.OnGameStart.AddListener(_OnGameStart);
    }

    // Start is called before the first frame update
    void Start()
    {
        witchTime = false;
        witchTimer = 0.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!bActive)
            return;

        if(witchTime)
        {
            witchTimer -= Time.deltaTime; 

            if(witchTimer <= 0.0f)
            {
                witchTime = false;
                witchTimer = 0.0f; 
            }
        }

        if (!isSliding)
        {
            RaycastHit hit = new RaycastHit();
            Vector3 slideDir = new Vector3(0, 0, 0);

            if (Input.GetKeyDown(KeyCode.A))
            {
                slideDir = -transform.right;
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                slideDir = transform.right;
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                slideDir = transform.forward;
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                slideDir = -transform.forward;
            }

            if (slideDir.magnitude > 0 && Physics.Raycast(transform.position, slideDir, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {

                bool bSwitchOrientation = Mathf.Abs(Vector3.Dot(hit.normal, slideDir)) > 0.9 ? false : true;

                Vector3 translation = hit.point + hit.normal * 0.5f;
                
                if (Vector3.Distance(transform.position, translation) > delta)
                {
                    StartCoroutine(Slide(translation, bSwitchOrientation));
                }
            }

        }
    }

    IEnumerator Slide(Vector3 target, bool bSwitchOrientation)
    {
        isSliding = true;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = bSwitchOrientation && transform.rotation.eulerAngles.y == 0 ? Quaternion.Euler(new Vector3(0, 45, 0)) : Quaternion.identity;
        float slerpVal = bSwitchOrientation ? 0 : 1;
        float slerpRate = 10.0f;

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
            

            yield return null;
        }
        shaker.TriggerShake();
        audioSource.PlayOneShot(audioSource.clip);
        isSliding = false;
    }

    private void _OnGameStart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        bActive = true;
        isSliding = false;
        size = initialSize;
    }

    private void _OnGameOver()
    {
        bActive = false;
    }

    private void UpdateSizeVisual()
    {
        Utilities.SetGlobalScale(transform, Vector3.one * GameManager.instance.unitLength * size);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.layer == (int)Mathf.Log(GameManager.instance.enemyLayer.value, 2) && !witchTime)
        {
            GameManager.instance.OnGameOver.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isSliding && !witchTime)
        {
            witchTime = true;
            witchTimer = 5.0f; 
        }
    }
}
