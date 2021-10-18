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

        [SerializeField] [TextArea] [Tooltip("각 요소는 생략 가능합니다. (부모 노드 제목) (자식 번호) 그 아랫줄에 (자신의 제목) 그 아랫줄에 (설명)")] private string Description;

        public abstract Result Execute();
        public abstract void ResetNode();

        protected virtual void Awake()
        {
            BT = GetComponent<BehaviorTree>();
        }
    }
}