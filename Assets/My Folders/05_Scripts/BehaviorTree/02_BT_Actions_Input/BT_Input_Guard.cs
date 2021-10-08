using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_Input_Guard : BT_Node
    {
        private CharacterControl characterControl;

        protected override void Awake()
        {
            base.Awake();
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }


        public override Result Execute()
        {
            if (Input.GetMouseButton(1))
            {
                Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"),0.0f,Input.GetAxisRaw("Vertical"));
                characterControl.Guard(dir);
                return Result.SUCCESS;
            }
            else 
            {
                return Result.FAILURE;
            }
        }

        public override void ResetNode()
        {
            
        }
    }
}