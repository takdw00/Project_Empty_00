using UnityEngine;

namespace BTSystem
{
    public class BT_Inverter : BT_Decorator
    {
        public override Result Execute()
        {
            Result result = child.Execute();
            Debug.Log("[" + child.GetType() + "] Executed : " + result.ToString());

            if (result != Result.RUNNING) 
            {
                return (result == Result.SUCCESS) ? Result.FAILURE : Result.SUCCESS;
            }

            return Result.RUNNING;
        }
    }
}