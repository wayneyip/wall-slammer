﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Range(1, 2)]
    public int initialSize;
    protected int size;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool bActive;

    public float moveInterval;
    public float moveSpeed;
    protected float moveTimer;

    protected bool isMoving;
    protected Vector3 movementTargetPosition;
    protected Vector3 movementDirection;
    protected Vector3 positionBeforeMovement;
    protected float targetVectorLength;

    public static int sensorSampleRate = 4;


    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        initialSize = (int)transform.lossyScale.x;

        GameManager.instance.OnGameOver.AddListener(_OnGameOver);
        GameManager.instance.OnGameStart.AddListener(_OnGameStart);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!bActive)
            return;

        bool canMove = false;
        if(moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            if(moveTimer <= 0)
            {
                moveTimer = 0;
                canMove = true;
            }
        }

        if (isMoving)
        {
            transform.position += moveSpeed * movementDirection * Time.deltaTime;
            if ((transform.position - positionBeforeMovement).magnitude >= targetVectorLength - 0.01f)
            {
                transform.position = movementTargetPosition;
                OnMoveFinished();
            }
        }
        else if (canMove)
        {
            ObtainNewTargetPosition();
            OnMoveStarted();
        }
    }


    private void OnMoveStarted()
    {
        //Debug.Log("Move Started. Target Position: " + movementTargetPosition);
        isMoving = true;
        positionBeforeMovement = transform.position;
        targetVectorLength = (movementTargetPosition - transform.position).magnitude;
    }

    private void OnMoveFinished()
    {
        //Debug.Log("Move Finished");
        moveTimer = moveInterval;
        isMoving = false;
    }

    //Set movementTargetPosition and movementDirection for the next move
    protected abstract void ObtainNewTargetPosition();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.instance.playerGO)
        {
            GameManager.instance.OnGameOver.Invoke();
        }
    }

    private void _OnGameStart()
    {
        moveTimer = moveInterval;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        bActive = true;
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