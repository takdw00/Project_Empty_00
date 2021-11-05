using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_Input_Move : BT_Node
    {
        private CharacterControl characterControl;

        protected override void Awake()
        {
            base.Awake();
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }

        public override Result Execute()
        {
            Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            if (Mathf.Approximately(dir.sqrMagnitude, 0.0f))
            {
                characterControl.NavMeshMoveMotionUpdate();
            }
            else 
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    characterControl.Walk(dir.normalized);
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    characterControl.Sprint(dir.normalized);
                }
                else 
                {
                    characterControl.Run(dir.normalized);
                }
            }

            return Result.SUCCESS;
        }

        public override void ResetNode()
        {

        }
    }
}