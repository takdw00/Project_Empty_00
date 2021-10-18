using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTSystem
{
    public class BT_MoveToTarget : BT_Node
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override Result Execute()
        {
            if (BT.Blackboard.followTarget == null)
            {
                return Result.FAILURE;
            }
            else 
            {
                return BT.Blackboard.characterControl.MoveByNavmesh(BT.Blackboard.followTarget.position + BT.Blackboard.followTargetOffset);
            }
        }

        public override void ResetNode()
        {
            BT.Blackboard.characterControl.StopNavMeshMove();
        }
    }
}