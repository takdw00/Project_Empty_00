using UnityEngine;

namespace BTSystem
{
    public abstract class BT_Decorator : BT_Node
    {
        [SerializeField] protected BT_Node child;

        public override void ResetNode()
        {
            child.ResetNode();
        }
    }
}