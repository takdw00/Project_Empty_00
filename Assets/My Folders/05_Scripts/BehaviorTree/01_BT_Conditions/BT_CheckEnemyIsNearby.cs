using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    //���� �αٿ� ��ġ�ϴ��� Ȯ���Ͽ� ����� SUCCESS, FAILURE�� ��ȯ
    public class BT_CheckEnemyIsNearby : BT_Node
    {
        public float recognitionDistance = 10.0f;

        public override Result Execute()
        {
            // ���� �Ÿ� �� ĳ���͵��� �迭�� Ȯ��
            // �ڽŰ� ķ���� �ٸ� ĳ���͵��� ����Ʈ�� Ȯ��
            // ����Ʈ�� Ȯ���� �༮�� �� ���� ����� ���� ã��

            //BT.Blackboard.CampIndex

            Collider[] characters = Physics.OverlapSphere(transform.position, recognitionDistance, LayerMask.GetMask("CHARACTER"));
            List<CharacterManager> enemies = new List<CharacterManager>();
            foreach (Collider c in characters) 
            {
                CharacterManager cm = c.GetComponent<CharacterManager>();
                if (cm.CharacterStatus.Camp != BT.Blackboard.CampIndex) 
                {
                    enemies.Add(cm);
                }
            }

            if (enemies.Count > 0) 
            {
                BT.Blackboard.attackTargets = enemies.ToArray();
                return Result.SUCCESS;
            }
            else 
            {
                Debug.Log("���� ��ó�� ���� �� �����ϴ�.");
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            
        }
    }
}