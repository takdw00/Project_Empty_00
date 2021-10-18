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

        [SerializeField] [TextArea] [Tooltip("�� ��Ҵ� ���� �����մϴ�. (�θ� ��� ����) (�ڽ� ��ȣ) �� �Ʒ��ٿ� (�ڽ��� ����) �� �Ʒ��ٿ� (����)")] private string Description;

        public abstract Result Execute();
        public abstract void ResetNode();

        protected virtual void Awake()
        {
            BT = GetComponent<BehaviorTree>();
        }
    }
}