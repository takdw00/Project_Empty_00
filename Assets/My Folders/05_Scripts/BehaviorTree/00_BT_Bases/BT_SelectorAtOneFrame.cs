using UnityEngine;

namespace BTSystem
{
    // ��Ʈ ��Ž�� �� ������ ��� ���� �����͸� �۵��ϰ� ���� �� ����Ѵ�.
    // SUCCESS, FAILURE�� �ٷιٷ� ������ �׼��� ��� �����ϱ⿡ ������ ���̴�.(��ǥ������ ��ǲ�� ���� �ൿ ����)
    // �׷��� RUNNING�� ������ �� �ൿ�� SUCCESS�� FAILURE�� ��ȯ�� ������ �� ������ ����Ǹ� ���� ��尡 ���ȴ�.
    public class BT_SelectorAtOneFrame : BT_Composite
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
                    return Result.SUCCESS;
                }
                else
                {
                    isRunning = false;
                    runningChildIndex = 0;
                }
            }

            return Result.FAILURE;
        }

        public override void ResetNode()
        {
            base.ResetNode();
            isRunning = false;
            runningChildIndex = 0;
        }
    }
}