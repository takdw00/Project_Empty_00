using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_SelectNearestEnemyTarget : BT_Node
    {
        public float attackRange = 1.0f;

        public override Result Execute()
        {
            if (BT.Blackboard.attackTargets.Length > 0)
            {
                float minDistance = Mathf.Infinity;
                CharacterManager target = null;

                foreach (CharacterManager c in BT.Blackboard.attackTargets)
                {
                    float distance = Vector3.Distance(transform.position, c.transform.position);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        target = c;
                    }
                }

                if (target != null) 
                {
                    Vector3 baseOffset = (transform.position - target.transform.position).normalized * (attackRange - 1.0f);

                    Vector2 offset;
                    do
                    {
                        offset = 1.0f * Random.insideUnitCircle;
                    } while (offset.sqrMagnitude < 0.5f);

                    BT.Blackboard.followTarget = target.transform;
                    BT.Blackboard.followTargetOffset = new Vector3(baseOffset.x + offset.x, 0.0f, baseOffset.z + offset.y);
                    BT.Blackboard.attackTarget = target;
                }

                return Result.SUCCESS;
            }
            else 
            {
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            
        }
    }
}