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
            
            // 캐릭터의 근처 지점으로 이동하도록 오프셋을 지정합니다. 플레이어 주변 1~2 사이의 거리로 지정됩니다.
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