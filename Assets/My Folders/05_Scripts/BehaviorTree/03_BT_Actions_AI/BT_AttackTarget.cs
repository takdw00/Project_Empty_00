using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_AttackTarget : BT_Node
    {
        public float attackRange = 1.0f;

        public override Result Execute()
        {
            if (BT.Blackboard.attackTarget == null)
            {
                return Result.FAILURE;
            }
            else 
            {
                if (BT.Blackboard.characterControl.IsAttacking)
                {
                    return Result.RUNNING;
                }
                else if (BT.Blackboard.attackTarget == null
                    || Vector3.Distance(BT.Blackboard.attackTarget.transform.position, transform.position) > attackRange
                    )
                {
                    return Result.FAILURE;
                }
                else 
                {
                    BT.Blackboard.characterControl.Attack((BT.Blackboard.attackTarget.transform.position - transform.position).normalized);
                    return Result.RUNNING;
                }

                
                
            }
        }

        public override void ResetNode()
        {
            
        }
    }
}