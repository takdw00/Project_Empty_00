using UnityEngine;

namespace BTSystem
{
    public class BT_Sequence : BT_Composite
    {
        protected int currentChildIndex;

        public override Result Execute()
        {
            Result result = children[currentChildIndex].Execute();
            Debug.Log("[" + children[currentChildIndex].GetType() + "](" + children[currentChildIndex].DebugText + ") Executed : " + result.ToString());

            if (result == Result.RUNNING)
            {
                return Result.RUNNING;
            }
            else if (result == Result.SUCCESS)
            {
                currentChildIndex++;

                if (currentChildIndex == children.Count)
                {
                    currentChildIndex = 0;
                    return Result.SUCCESS;
                }

                return Result.RUNNING;
            }
            else
            {
                currentChildIndex = 0;
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            base.ResetNode();
            currentChildIndex = 0;
        }
    }
}