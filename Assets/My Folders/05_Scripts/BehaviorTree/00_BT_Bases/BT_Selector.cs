using UnityEngine;

namespace BTSystem
{
    // 자식 노드들 중 하나가 RUNNING을 반환하면 노드를 선택한 것으로 간주한다.
    // 이 경우, 선택한 행동을 계속 수행하는 도중 FAILURE가 발생할 시 Selector는 실패한 것으로 본다.
    // 즉, 여러 행동 중 하나의 행동을 골라 완전히 다 수행해야 Selector의 성공이다.
    // 반대로 생각해보면 꼭 SUCCESS가 나오지 않더라도 Selector는 이미 행동을 선택하여 실행하면서 RUNNING을 반환할 것이다.
    public class BT_Selector : BT_Composite
    {
        protected int currentChildIndex;
        protected bool nodeSelected;

        public override Result Execute()
        {
            Result result = children[currentChildIndex].Execute();
            Debug.Log("[" + children[currentChildIndex].GetType() + "](" + children[currentChildIndex].DebugText + ") Executed : " + result.ToString());

            if (result == Result.RUNNING)
            {
                nodeSelected = true;
                return Result.RUNNING;
            }
            else if (result == Result.SUCCESS)
            {
                currentChildIndex = 0;
                nodeSelected = false;
                return Result.SUCCESS;
            }
            else
            {
                if (nodeSelected)
                {
                    currentChildIndex = 0;
                    nodeSelected = false;
                    return Result.FAILURE;
                }

                currentChildIndex++;

                if (currentChildIndex == children.Count) 
                {
                    currentChildIndex = 0;
                    nodeSelected = false;
                    return Result.FAILURE;
                }

                return Result.RUNNING;
            }
        }

        public override void ResetNode()
        {
            base.ResetNode();
            currentChildIndex = 0;
            nodeSelected = false;
        }
    }
}