﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEnemy : EnemyBase
{

    public enum Direction { X, Z };
    public Direction direction;
    public List<Vector3> dirSequence;
    int dirIndex = 0;


    protected override void Awake()
    {
        base.Awake();
        GameManager.instance.OnGameStart.AddListener(ResetSequence);
    }

    private void LateUpdate()
    {
        if (size > 1)
        {
            Vector3 dir = direction == Direction.X ? new Vector3(1, 0, 0) : new Vector3(0, 0, 1);
            Vector3 normalDir = direction == Direction.X ? new Vector3(0, 0, 1) : new Vector3(1, 0, 0);


            for (int i = 0; i < size; ++i)
            {
                Vector3 rayOrigin = transform.position + ((size - 1) / 2.0f - i) * normalDir;

                Debug.DrawLine(rayOrigin, rayOrigin + 5.0f * dir, Color.green);

            }
        }
    }

    protected override void ObtainNewTargetPosition()
    {
        Vector3 dir = dirSequence[dirIndex];
        dirIndex++;
        if (dirIndex == 5)
        {
            dirIndex = 0;
        }
        Vector3 normalDir = direction == Direction.X ? transform.forward : transform.right;

        float minPositive = float.PositiveInfinity;
        float minNegative = float.PositiveInfinity;

        for (int i = 0; i < size; ++i)
        {
            Vector3 rayOrigin = transform.position + ((size - 1) / 2.0f - i) * normalDir;

            Ray posRay = new Ray(rayOrigin, dir);
            Ray negRay = new Ray(rayOrigin, -dir);

            if (Physics.Raycast(posRay, out RaycastHit posHit, float.PositiveInfinity, GameManager.instance.wallLayer.value))
            {
                minPositive = Mathf.Min((posHit.point - transform.position).magnitude, minPositive);
            }
            else
            {
                //there is no wall in the positive direciton
                Debug.LogError("No wall in the posive direction");
                Destroy(this);
            }

            if (Physics.Raycast(negRay, out RaycastHit negHit, float.PositiveInfinity, GameManager.instance.wallLayer.value))
            {
                minNegative = Mathf.Min((negHit.point - transform.position).magnitude, minNegative);
            }
            else
            {
                //there is no wall in the negative direciton
                Debug.LogError("No wall in negative direction");
                Destroy(this);
            }

        }

        if (minPositive > minNegative)
        {
            if (minPositive <= size / 2.0f * GameManager.instance.unitLength)
            {
                Debug.LogError("No Movement Space");
                Destroy(this);
            }
            else
            {
                movementTargetPosition = transform.position + dir * (minPositive - size / 2.0f * GameManager.instance.unitLength);
                movementDirection = dir;
            }
        }
        else
        {
            if (minNegative <= size / 2.0f * GameManager.instance.unitLength)
            {
                Debug.LogError("No Movement Space");
                Destroy(this);
            }
            else
            {
                movementTargetPosition = transform.position - dir * (minNegative - size / 2.0f * GameManager.instance.unitLength);
                movementDirection = -dir;
            }
        }
    }

    private void ResetSequence()
    {
        dirIndex = 0;
    }


}
