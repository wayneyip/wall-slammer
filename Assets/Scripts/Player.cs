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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bActive)
            return;


        if (!isSliding)
        {
            RaycastHit hit;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Physics.Raycast(transform.position, -transform.right, out hit))
                {
                    Vector3 translationPos = hit.point + hit.transform.forward * 0.5f;
                    Vector3 translationNeg = hit.point - hit.transform.forward * 0.5f;

                    Vector3 translation = (translationPos - transform.position).magnitude < (translationNeg - transform.position).magnitude ? translationPos : translationNeg;

                    bool bSwitchOrientation = false;

                    if (Mathf.Abs(Mathf.Abs(hit.transform.rotation.eulerAngles.y) - 45f) < 0.1f)
                    {
                        //Wall is diagonal
                        if (!bIsDiagonal)
                        {
                            //Player was not diagonal
                            bSwitchOrientation = true;
                            bIsDiagonal = true;
                        }
                    }
                    else
                    {
                        if (bIsDiagonal)
                        {
                            bSwitchOrientation = true;
                            bIsDiagonal = false;
                        }
                    }

                    if (Vector3.Distance(transform.position, translation) > delta)
                    {
                        StartCoroutine(Slide(translation, bSwitchOrientation));
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Physics.Raycast(transform.position, transform.right, out hit))
                {
                    Vector3 translationPos = hit.point + hit.transform.forward * 0.5f;
                    Vector3 translationNeg = hit.point - hit.transform.forward * 0.5f;

                    Vector3 translation = (translationPos - transform.position).magnitude < (translationNeg - transform.position).magnitude ? translationPos : translationNeg;

                    bool bSwitchOrientation = false;

                    if (Mathf.Abs(Mathf.Abs(hit.transform.rotation.eulerAngles.y) - 45f) < 0.1f)
                    {
                        //Wall is diagonal
                        if (!bIsDiagonal)
                        {
                            //Player was not diagonal
                            bSwitchOrientation = true;
                            bIsDiagonal = true;
                        }
                    }
                    else
                    {
                        if (bIsDiagonal)
                        {
                            bSwitchOrientation = true;
                            bIsDiagonal = false;
                        }
                    }

                    if (Vector3.Distance(transform.position, translation) > delta)
                    {
                        StartCoroutine(Slide(translation, bSwitchOrientation));
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Vector3 translationPos = hit.point + hit.transform.forward * 0.5f;
                    Vector3 translationNeg = hit.point - hit.transform.forward * 0.5f;

                    Vector3 translation = (translationPos - transform.position).magnitude < (translationNeg - transform.position).magnitude ? translationPos : translationNeg;

                    bool bSwitchOrientation = false;

                    if (Mathf.Abs(Mathf.Abs(hit.transform.rotation.eulerAngles.y) - 45f) < 0.1f)
                    {
                        Debug.Log("Diagonal Wall");
                        //Wall is diagonal
                        if (!bIsDiagonal)
                        {
                            Debug.Log("Player was not diagonal");
                            //Player was not diagonal
                            bSwitchOrientation = true;
                            bIsDiagonal = true;
                        }
                    }
                    else
                    {
                        if (bIsDiagonal)
                        {
                            bSwitchOrientation = true;
                            bIsDiagonal = false;
                        }
                    }

                    if (Vector3.Distance(transform.position, translation) > delta)
                    {
                        StartCoroutine(Slide(translation, bSwitchOrientation));
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (Physics.Raycast(transform.position, -transform.forward, out hit))
                {
                    Vector3 translationPos = hit.point + hit.transform.forward * 0.5f;
                    Vector3 translationNeg = hit.point - hit.transform.forward * 0.5f;

                    Vector3 translation = (translationPos - transform.position).magnitude < (translationNeg - transform.position).magnitude ? translationPos : translationNeg;

                    bool bSwitchOrientation = false;

                    if (Mathf.Abs(Mathf.Abs(hit.transform.rotation.eulerAngles.y) - 45f) < 0.1f)
                    {
                        //Wall is diagonal
                        if (!bIsDiagonal)
                        {
                            //Player was not diagonal
                            bSwitchOrientation = true;
                            bIsDiagonal = true;
                        }
                    }
                    else
                    {
                        if (bIsDiagonal)
                        {
                            bSwitchOrientation = true;
                            bIsDiagonal = false;
                        }
                    }

                    if (Vector3.Distance(transform.position, translation) > delta)
                    {
                        StartCoroutine(Slide(translation, bSwitchOrientation));
                    }
                }
            }
        }
    }

    IEnumerator Slide(Vector3 target, bool bSwitchOrientation)
    {
        Debug.Log(target);
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

}
