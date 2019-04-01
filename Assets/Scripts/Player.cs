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


    Rigidbody rb;
    float speed = 100.0f;
    bool isSliding = false;
    float delta = 0.1f;
    public ShakeBehavior shaker;
    AudioSource audioSource;


    private void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bActive)
            return;


        if (!isSliding)
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

}
