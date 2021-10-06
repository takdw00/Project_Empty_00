using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_Input_Move : BT_Node
    {
        [Tooltip("캐릭터 이동 속도에 이 값을 곱해 적용됩니다.")] public float speedMultiply;
        private CharacterControl characterControl;

        protected override void Awake()
        {
            base.Awake();
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }

        public override Result Execute()
        {
            bool inputIsWalk = Input.GetKey(KeyCode.LeftControl);
            Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical")).normalized;

            if (inputIsWalk)
            {
                characterControl.Walk(inputDirection);
            }
            else 
            {
                characterControl.Run(inputDirection);
            }
            

            return Result.SUCCESS;
        }

        public override void ResetNode()
        {

        }
    }
}