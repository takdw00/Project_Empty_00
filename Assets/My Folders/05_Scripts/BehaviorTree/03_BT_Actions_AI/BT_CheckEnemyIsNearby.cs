using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    //���� �αٿ� ��ġ�ϴ��� Ȯ���Ͽ� ����� SUCCESS, FAILURE�� ��ȯ
    public class BT_CheckEnemyIsNearby : BT_Node
    {
        public override Result Execute()
        {
            Debug.Log("���� ��ó�� ���� �� �����ϴ�.");
            return Result.FAILURE;
        }

        public override void ResetNode()
        {
            
        }
    }
}