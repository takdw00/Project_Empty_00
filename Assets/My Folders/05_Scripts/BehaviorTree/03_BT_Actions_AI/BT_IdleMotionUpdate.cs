using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_IdleMotionUpdate : BT_Node
    {
        public override Result Execute()
        {
            BT.Blackboard.characterControl.IdleAndMoveMotionUpdate();
            return Result.SUCCESS;
        }

        public override void ResetNode()
        {
            
        }
    }
}