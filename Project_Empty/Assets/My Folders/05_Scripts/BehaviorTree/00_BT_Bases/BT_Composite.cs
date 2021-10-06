using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public abstract class BT_Composite : BT_Node
    {
        [SerializeField] protected List<BT_Node> children;

        public override void ResetNode()
        {
            foreach (BT_Node n in children) 
            {
                n.ResetNode();
            }
        }
    }
}