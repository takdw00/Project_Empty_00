using UnityEngine;

namespace BTSystem
{
    // ���ǽĿ� �ش��ϴ� ��� �ϳ��� �����Ͽ� ����Ѵ�. ������ SUCCESS�� FAILURE���� ��ȯ�ؾ� �Ѵ�.
    // ���ǽ��� SUCCESS�� �ڽ��� ������ �� ����� ��ȯ�ϰ�, FAILURE�� �ڽ��� �������� ���� �ʰ� �ٷ� FAILURE�� ��ȯ�Ѵ�.
    // ���� �ڽ� ��忡�� RUNNING ���̾��µ� �� ���ڷ����ͷ� ���� ���ܼ� FAILURE�� ��ȯ��ٸ� �ڽ� ���� �ڵ����� ���µȴ�.
    // ���� ������ ������ ��忡�� ���� �ڽ� ��忡 ���ǽ� ��带 �����ص� ����� ȿ���� ���� �� �ִ�. ���غ��̴� ���� ����ϸ� �ȴ�.
    public class BT_If : BT_Decorator
    {
        [SerializeField] [Tooltip("���ǽ����� ����� �׼� ��带 ���� ��")] protected BT_Node conditionNode;

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