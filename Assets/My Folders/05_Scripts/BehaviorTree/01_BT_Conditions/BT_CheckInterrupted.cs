using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // 캐릭터가 동작 중단 방해 효과를 받는지 체크하여 결과를 반환
    public class BT_CheckInterrupted : BT_Node
    {
        public override Result Execute()
        {
            return Result.SUCCESS;
        }

        public override void ResetNode()
        {
            
        }
    }
}