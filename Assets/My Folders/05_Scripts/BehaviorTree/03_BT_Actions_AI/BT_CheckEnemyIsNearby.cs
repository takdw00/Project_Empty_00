using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    //적이 인근에 위치하는지 확인하여 결과를 SUCCESS, FAILURE로 반환
    public class BT_CheckEnemyIsNearby : BT_Node
    {
        public override Result Execute()
        {
            Debug.Log("적이 근처에 없는 것 같습니다.");
            return Result.FAILURE;
        }

        public override void ResetNode()
        {
            
        }
    }
}