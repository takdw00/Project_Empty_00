using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTSystem
{
    public class BT_MoveToTarget : BT_Node
    {
        public Transform target;
        private CharacterControl characterControl;

        protected override void Awake()
        {
            base.Awake();
            characterControl = transform.parent.GetComponent<CharacterControl>();

        }

        public override Result Execute()
        {
            return characterControl.MoveByNavmesh(target.position);
        }

        public override void ResetNode()
        {
            characterControl.StopNavMeshMove();
        }
    }
}