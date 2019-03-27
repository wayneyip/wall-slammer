using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedEnemy : EnemyBase
{

    public enum Direction { X, Z };
    public Direction direction;


    protected override void ObtainNewTargetPosition()
    {
        Vector3 dir = direction == Direction.X ? new Vector3(-1, 0, 0) : new Vector3(0, 0, 1);

        Ray rayPos = new Ray(transform.position, dir);
        Ray rayNeg = new Ray(transform.position, -dir);

        if(Physics.Raycast(rayPos, out RaycastHit hitPos, float.PositiveInfinity, GameManager.instance.wallLayer.value)){

            if((hitPos.point - transform.position).magnitude < 1.0f)
            {
                if (Physics.Raycast(rayNeg, out RaycastHit hitNeg, float.PositiveInfinity, GameManager.instance.wallLayer.value))
                {
                    Debug.Log(hitPos.point + " " + hitNeg.point);
                    movementTargetPosition = hitNeg.point + dir / 2.0f;
                    movementDirection = -dir;
                }
                else
                {
                    //there is no wall in the negative direciton
                    Debug.LogError("No wall in negative direction");
                    Destroy(this);
                }
            }
            else
            {
                movementTargetPosition = hitPos.point - dir / 2.0f;
                movementDirection = dir;
            }

        }
        else
        {
            //there is no wall in the positive direciton
            Debug.LogError("No wall in positive direction");
            Destroy(this);
        }
    }
}
