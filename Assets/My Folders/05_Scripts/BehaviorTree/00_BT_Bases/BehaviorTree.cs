using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // �ٸ� ������ �̿��ϰ��� �� ��� Using BTSystem; ���� �̿��Ѵ�.
    // ĳ������ �߽� ������Ʈ�� �ڽ� ���ӿ�����Ʈ�� BehaviorTree�� ��� BT���� ��带 �� �ٿ��� ����Ѵ�.
    // BehaviorTree�� ���� �� ���� ��Ʈ ��带 ������. 0�� ��Ʈ�� �÷��̾� ��ǲ���� ���۵Ǵ� ����̸�, �̿� ���� AI���̴�.
    // �� ��� ���ϴ� Ʈ���� ��ü�ϴ� �۾��� ���� ������ �ڽ��� �ʱ�ȭ�ϴ� �޼��� ResetNode�� �����Լ��� ������.

    public enum Result { SUCCESS, RUNNING, FAILURE };
    public enum BehaviorMode { INPUT, STANDARD }

    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField] [Tooltip("�ܼ� ǥ�ÿ�")] private BehaviorMode currentMode;
        [SerializeField] private BT_Node[] rootNodes;

        public Dictionary<string, object> Blackboard { get; set; }
        
        private bool isBehaviorStarted;
        private int currentBehaviorIndex;

        private void Awake()
        {
            isBehaviorStarted = false;
            currentBehaviorIndex = 1;
        }

        private void Start()
        {
            StartBehavior(BehaviorMode.STANDARD);
        }

        private void Update()
        {
            if (isBehaviorStarted) 
            {
                BT_Node nodeToExecute = rootNodes[currentBehaviorIndex];
                Result result = nodeToExecute.Execute();
                Debug.Log("[" + nodeToExecute.GetType() + "] Executed : " + result.ToString());
            }
            
        }


        #region Inspector Test
        [ContextMenu("StartMode_Input")]
        public void StartInputBehavior() 
        {
            StartBehavior(BehaviorMode.INPUT);
        }

        [ContextMenu("StartMode_Standard")]
        public void StartStandardBehavior() 
        {
            StartBehavior(BehaviorMode.STANDARD);
        }

        #endregion


        //���� �ε����� �ൿ Ʈ�� ������ �����ϴ� �Լ��̴�.
        public void StartBehavior(BehaviorMode mode)
        {
            int behaviorIndex = (int)mode;

            //�̹� ���� ���� ���� �ൿ Ʈ���� ������ ��쿡�� �������� �ʴ´�.(�ƹ� �ϵ� �Ͼ�� �ʴ´�.)
            if (!isBehaviorStarted || currentBehaviorIndex != behaviorIndex) 
            {
                rootNodes[currentBehaviorIndex].ResetNode();
            }

            currentBehaviorIndex = behaviorIndex;
            currentMode = mode;
            isBehaviorStarted = true;
        }

        //�ൿ Ʈ�� ������ �����.
        [ContextMenu("StopBehavior")]
        public void StopBehavior() 
        {
            rootNodes[currentBehaviorIndex].ResetNode();
            isBehaviorStarted = false;
        }
    }
}