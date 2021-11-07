using UnityEngine;

namespace BTSystem
{
    // 루트 재탐색 간 프레임 대기 없이 시퀀스를 작동하고 싶을 때 사용한다.
    // SUCCESS, FAILURE가 바로바로 나오는 액션을 동시에 실행하기에 유용할 것이다.
    // 그러나 RUNNING이 나오면 그 행동이 SUCCESS나 FAILURE를 반환할 때까지 매 프레임 실행되며 다음 노드가 대기된다.
    public class BT_SequenceAtOneFrame : BT_Composite
    {
        protected bool isRunning;
        protected int runningChildIndex;

        public override Result Execute()
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (isRunning) 
                {
                    i = runningChildIndex;
                }

                Result result = children[i].Execute();
                Debug.Log("[" + children[i].GetType() + "](" + children[i].DebugText + ") Executed : " + result.ToString());

                if (result == Result.RUNNING)
                {
                    isRunning = true;
                    runningChildIndex = i;
                    return Result.RUNNING;
                }
                else if (result == Result.SUCCESS)
                {
                    isRunning = false;
                    runningChildIndex = 0;
                }
                else
                {
                    isRunning = false;
                    runningChildIndex = 0;
                    return Result.FAILURE;
                }
            }

            return Result.SUCCESS;
        }

        public override void ResetNode()
        {
            base.ResetNode();
            isRunning = false;
            runningChildIndex = 0;
        }
    }
}