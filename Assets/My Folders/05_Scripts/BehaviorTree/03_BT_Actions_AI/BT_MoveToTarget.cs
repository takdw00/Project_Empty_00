using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTSystem
{
    public class BT_MoveToTarget : BT_Node
    {
        public Transform target;
        public float stoppingDistance;
        public float destinationUpdateDistance;


        private CharacterControl characterControl;
        
        private bool moveStarted = false;
        private Vector3 oldDestination;

        protected override void Awake()
        {
            base.Awake();
            characterControl = transform.parent.GetComponent<CharacterControl>();

        }

        public override Result Execute()
        {
            if (target == null) 
            {
                return Result.FAILURE;
            }

            if (CheckArrived())
            {
                characterControl.StopNavMeshMove();
                moveStarted = false;
                return Result.SUCCESS;
            }

            if (!moveStarted || Vector3.SqrMagnitude(oldDestination - target.position) >= destinationUpdateDistance * destinationUpdateDistance)
            {
                if (characterControl.MoveByNavmesh(target.position) == false)
                {
                    return Result.FAILURE;
                }
                else 
                {
                    oldDestination = target.position;
                    moveStarted = true;
                }
            }

            return Result.RUNNING;
        }

        public override void ResetNode()
        {
            characterControl.StopNavMeshMove();
            moveStarted = false;
        }


        private bool CheckArrived() 
        {
            return Vector3.SqrMagnitude(transform.position - target.position) <= stoppingDistance * stoppingDistance;
        }
    }
}