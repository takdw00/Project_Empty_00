using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // �÷��̾� ���� �ִ��� �˻��մϴ�.
    public class BT_CheckOnNextToPlayer : BT_Node
    {
        public float distance = 3.0f;

        public override Result Execute()
        {
            if (Vector3.SqrMagnitude(BT.Blackboard.characterControl.transform.position - GlobalBlackboard.Instance.playerTransform.position) < distance * distance) 
            {
                return Result.SUCCESS;
            }

            Debug.Log("�÷��̾� ���� �̹� �ֽ��ϴ�.");
            return Result.FAILURE;
        }

        public override void ResetNode()
        {

        }
    }
}