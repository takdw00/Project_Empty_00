using UnityEngine;

namespace BTSystem
{
    // 조건식에 해당하는 노드 하나를 연결하여 사용한다. 조건은 SUCCESS나 FAILURE만을 반환해야 한다.
    // 조건식이 SUCCESS면 자식을 실행해 그 결과를 반환하고, FAILURE면 자식을 실행조차 하지 않고 바로 FAILURE를 반환한다.
    // 만약 자식 노드에서 RUNNING 중이었는데 이 데코레이터로 인해 끊겨서 FAILURE로 반환됬다면 자식 노드는 자동으로 리셋된다.
    // 단일 프레임 시퀀스 노드에서 왼쪽 자식 노드에 조건식 노드를 연결해도 비슷한 효과를 누릴 수 있다. 편해보이는 것을 사용하면 된다.
    public class BT_If : BT_Decorator
    {
        [SerializeField] [Tooltip("조건식으로 사용할 액션 노드를 넣을 것")] protected BT_Node conditionNode;

        public override Result Execute()
        {
            Result conditionResult = conditionNode.Execute();
            Debug.Log("[" + conditionNode.GetType() + "](" + conditionNode.DebugText + ") Executed : " + conditionResult.ToString());

            if (conditionResult == Result.SUCCESS)
            {
                Result result = child.Execute();
                Debug.Log("[" + child.GetType() + "](" + child.DebugText + ") Executed : " + result.ToString());

                return result;
            }
            else
            {
                child.ResetNode();
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            base.ResetNode();
            conditionNode.ResetNode();
        }
    }
}