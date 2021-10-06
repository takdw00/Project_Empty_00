using UnityEngine;

namespace BTSystem
{
    public abstract class BT_Node : MonoBehaviour
    {
        private BehaviorTree behaviorTree;
        public BehaviorTree BT
        {
            get { return behaviorTree; }
            private set { behaviorTree = value; }
        }

        [SerializeField] [TextArea] [Tooltip("어떤 설명이든 상관없습니다. 노드 간 구분이 가능하도록만 적어주세요.")] private string Description;

        public abstract Result Execute();
        public abstract void ResetNode();

        protected virtual void Awake()
        {
            BT = GetComponent<BehaviorTree>();
        }
    }
}