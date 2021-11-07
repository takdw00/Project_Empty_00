using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    //적이 인근에 위치하는지 확인하여 결과를 SUCCESS, FAILURE로 반환
    public class BT_CheckEnemyIsNearby : BT_Node
    {
        public float recognitionDistance = 10.0f;

        public override Result Execute()
        {
            // 인지 거리 내 캐릭터들을 배열로 확보
            // 자신과 캠프가 다른 캐릭터들을 리스트로 확보
            // 리스트로 확보된 녀석들 중 가장 가까운 적을 찾음

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
                Debug.Log("적이 근처에 없는 것 같습니다.");
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            
        }
    }
}