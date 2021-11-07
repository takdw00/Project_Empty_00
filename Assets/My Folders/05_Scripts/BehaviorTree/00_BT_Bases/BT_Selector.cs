using UnityEngine;

namespace BTSystem
{
    // �ڽ� ���� �� �ϳ��� RUNNING�� ��ȯ�ϸ� ��带 ������ ������ �����Ѵ�.
    // �� ���, ������ �ൿ�� ��� �����ϴ� ���� FAILURE�� �߻��� �� Selector�� ������ ������ ����.
    // ��, ���� �ൿ �� �ϳ��� �ൿ�� ��� ������ �� �����ؾ� Selector�� �����̴�.
    // �ݴ�� �����غ��� �� SUCCESS�� ������ �ʴ��� Selector�� �̹� �ൿ�� �����Ͽ� �����ϸ鼭 RUNNING�� ��ȯ�� ���̴�.
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