using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl_Skeleton : CharacterControl_Monster
{
    public override void NavMeshMoveMotionUpdate()
    {
        float currentNavAgentSpeedSqr = Vector3.SqrMagnitude(navAgent.velocity);

        if (currentNavAgentSpeedSqr < 0.01f)
        {
            lastNavMeshMoveMotion = AnimIndex_Basic.IDLE;
            animator.SetInteger("Anim", (int)AnimIndex_Basic.IDLE);

        }
        else
        {
            lastNavMeshMoveMotion = AnimIndex_Basic.RUN;
            animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);

            if (Time.time >= lastTimeNavmeshMoveMotionUpdated + 0.2f)
            {
                lastTimeNavmeshMoveMotionUpdated = Time.time;
                SetDirection(navAgent.velocity);
            }
        }
    }
}
