using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // ĳ���Ͱ� ���� �ߴ� ���� ȿ���� �޴��� üũ�Ͽ� ����� ��ȯ
    public class BT_CheckInterrupted : BT_Node
    {
        public override Result Execute()
        {
            return BT.Blackboard.characterControl.CheckInterrupted() ? Result.SUCCESS : Result.FAILURE;
        }

        public override void ResetNode()
        {
            
        }
    }
}