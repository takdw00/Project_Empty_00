using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // 플레이어 옆에 있는지 검사합니다.
    public class BT_CheckOnNextToPlayer : BT_Node
    {
        public float distance = 3.0f;

        public override Result Execute()
        {
            if (Vector3.SqrMagnitude(BT.Blackboard.characterControl.transform.position - GlobalBlackboard.Instance.playerTransform.position) < distance * distance) 
            {
                return Result.SUCCESS;
            }

            Debug.Log("플레이어 옆에 이미 있습니다.");
            return Result.FAILURE;
        }

        public override void ResetNode()
        {

        }
    }
}