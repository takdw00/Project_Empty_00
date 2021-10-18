using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_SetFollowTargetToPlayer : BT_Node
    {
        public override Result Execute()
        {
            BT.Blackboard.followTarget = GlobalBlackboard.Instance.playerTransform;
            
            // ĳ������ ��ó �������� �̵��ϵ��� �������� �����մϴ�. �÷��̾� �ֺ� 1~2 ������ �Ÿ��� �����˴ϴ�.
            Vector2 offset;
            do
            {
                offset = 2.0f * Random.insideUnitCircle;
            } while (offset.sqrMagnitude < 1.0f);

            BT.Blackboard.followTargetOffset = new Vector3(offset.x, 0.0f, offset.y);

            return Result.SUCCESS;
        }

        public override void ResetNode()
        {
            
        }
    }
}