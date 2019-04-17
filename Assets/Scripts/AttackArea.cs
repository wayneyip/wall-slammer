using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackArea : MonoBehaviour
{
    public Vector3 attackDirection;

    public bool isStaying;

    public bool isDetecting;

    public UnityEvent OnTriggered;

    public bool selected;

    private void Awake()
    {
        GameManager.instance.OnGameStart.AddListener(_OnGameStart);
        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isDetecting)
            return;

        if (isStaying)
        {
            if (!selected)
            {
                OnTriggered.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isStaying = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isStaying = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isStaying = false;
    }

    private void _OnGameStart()
    {
        selected = false;
        isStaying = false;
    }

    private void _OnGameOver()
    {

    }

}
