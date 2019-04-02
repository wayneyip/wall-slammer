using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedEnemy : EnemyBase
{

    public enum Direction { X, Z };
    public Direction direction;


    private void LateUpdate()
    {
        if(size > 1)
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
        Vector3 dir = direction == Direction.X ? transform.right : transform.forward;
        Vector3 normalDir = direction == Direction.X ? transform.forward : transform.right;

        //Ray rayPos = new Ray(transform.position, dir);
        //Ray rayNeg = new Ray(transform.position, -dir);

        //if(Physics.Raycast(rayPos, out RaycastHit hitPos, float.PositiveInfinity, GameManager.instance.wallLayer.value)){

        //    if((hitPos.point - transform.position).magnitude < 1.0f)
        //    {
        //        if (Physics.Raycast(rayNeg, out RaycastHit hitNeg, float.PositiveInfinity, GameManager.instance.wallLayer.value))
        //        {
        //            //Debug.Log(hitPos.point + " " + hitNeg.point);
        //            movementTargetPosition = hitNeg.point + dir / 2.0f;
        //            movementDirection = -dir;
        //        }
        //        else
        //        {
        //            //there is no wall in the negative direciton
        //            Debug.LogError("No wall in negative direction");
        //            Destroy(this);
        //        }
        //    }
        //    else
        //    {
        //        movementTargetPosition = hitPos.point - dir / 2.0f;
        //        movementDirection = dir;
        //    }

        //}
        //else
        //{
        //    //there is no wall in the positive direciton
        //    Debug.LogError("No wall in positive direction");
        //    Destroy(this);
        //}

        float minPositive = float.PositiveInfinity;
        float minNegative = float.PositiveInfinity;

        for(int i = 0; i < size; ++i)
        {
            Vector3 rayOrigin = transform.position + ((size - 1) / 2.0f - i) * normalDir;

            Ray posRay = new Ray(rayOrigin, dir);
            Ray negRay = new Ray(rayOrigin, -dir);

            if(Physics.Raycast(posRay, out RaycastHit posHit, float.PositiveInfinity, GameManager.instance.wallLayer.value))
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
            if(minPositive <= size / 2.0f * GameManager.instance.unitLength)
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
}
