using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // 다른 곳에서 이용하고자 할 경우 Using BTSystem; 으로 이용한다.
    // 캐릭터의 중심 오브젝트의 자식 게임오브젝트에 BehaviorTree와 모든 BT관련 노드를 다 붙여서 사용한다.
    // BehaviorTree는 보통 두 개의 루트 노드를 가진다. 0번 루트는 플레이어 인풋으로 조작되는 방식이며, 이외 노드는 AI들이다.
    // 이 경우 원하는 트리를 교체하는 작업을 위해 노드들은 자신을 초기화하는 메서드 ResetNode를 가상함수로 가진다.

    public enum Result { SUCCESS, RUNNING, FAILURE };
    public enum BehaviorMode { INPUT, STANDARD }

    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField] [Tooltip("단순 표시용")] private BehaviorMode currentMode;
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


        //지정 인덱스의 행동 트리 동작을 시작하는 함수이다.
        public void StartBehavior(BehaviorMode mode)
        {
            int behaviorIndex = (int)mode;

            //이미 실행 중인 같은 행동 트리를 시작할 경우에는 리셋하지 않는다.(아무 일도 일어나지 않는다.)
            if (!isBehaviorStarted || currentBehaviorIndex != behaviorIndex) 
            {
                rootNodes[currentBehaviorIndex].ResetNode();
            }

            currentBehaviorIndex = behaviorIndex;
            currentMode = mode;
            isBehaviorStarted = true;
        }

        //행동 트리 동작을 멈춘다.
        [ContextMenu("StopBehavior")]
        public void StopBehavior() 
        {
            rootNodes[currentBehaviorIndex].ResetNode();
            isBehaviorStarted = false;
        }
    }
}