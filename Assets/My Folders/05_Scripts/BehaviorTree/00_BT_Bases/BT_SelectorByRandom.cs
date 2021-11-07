using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    public class BT_SelectorByRandom : BT_Composite
    {
        [System.Serializable]
        public struct ChildInfo 
        {
            public float probability;
            public bool isExecuted;
        };

        // 반드시 Children과 수가 같아야 한다.
        // 실수의 크기의 합이 꼭 1일 필요는 없다. 각 실수의 비율에 따라 확률이 결정된다.
        [Tooltip("반드시 children과 같은 개수만큼 생성할 것")]
        public ChildInfo[] childrenInfo;

        protected int currentChildIndex;
        protected bool nodeSelected;

        // 이 함수를 통해 외부에서 확률을 변동시킴
        public void SetProbability(int index, float value) 
        {
            if (index >= 0 && index < childrenInfo.Length)
            {
                childrenInfo[index].probability = value;
            }
            else 
            {
                Debug.LogError("Index : " + index + " is Out of Array Length 0 ~ " + childrenInfo.Length);
            }
        }

        private void ResetExecuted() 
        {
            for (int i = 0; i < childrenInfo.Length; i++)
            {
                childrenInfo[i].isExecuted = false;
            }
        }

        protected virtual void SelectByRandom() 
        {
            List<int> includedIndexes = new List<int>();

            float total = 0f;
            for (int i = 0; i < childrenInfo.Length; i++)
            {
                if (childrenInfo[i].isExecuted == false) 
                {
                    includedIndexes.Add(i);
                    total += childrenInfo[i].probability;
                }
            }

            float randomValue = Random.Range(0f, total);

            float boundary = 0f;
            for (int i = 0; i < includedIndexes.Count-1; i++)
            {
                boundary += childrenInfo[includedIndexes[i]].probability;
                if (randomValue < boundary) 
                {
                    currentChildIndex = includedIndexes[i];
                    break;
                }

                // 끝 원소 바로 전까지 검사했는데 아직 당첨되지 않았다면 그 다음 원소(제일 끝 원소)로 자동 당첨
                if (i == includedIndexes.Count - 2) 
                {
                    currentChildIndex = includedIndexes[i + 1];
                }
            }

        }

        public override Result Execute()
        {
            Result result;

            while (!nodeSelected) 
            {
                SelectByRandom();
                result = children[currentChildIndex].Execute();
                Debug.Log("[" + children[currentChildIndex].GetType() + "](" + children[currentChildIndex].DebugText + ") Executed : " + result.ToString());
                childrenInfo[currentChildIndex].isExecuted = true;

                if (result == Result.RUNNING)
                {
                    nodeSelected = true;
                    return Result.RUNNING;
                }
                else if (result == Result.SUCCESS)
                {
                    currentChildIndex = 0;
                    nodeSelected = false;
                    ResetExecuted();
                    return Result.SUCCESS;
                }
                else
                {
                    bool completed = true;
                    for (int i = 0; i < childrenInfo.Length; i++)
                    {
                        if (childrenInfo[i].isExecuted == false) 
                        {
                            completed = false;
                        }
                    }

                    if (completed)
                    {
                        currentChildIndex = 0;
                        nodeSelected = false;
                        ResetExecuted();
                        return Result.FAILURE;
                    }
                }
            }

            //node Selected and RUNNING
            result = children[currentChildIndex].Execute();

            if (result == Result.RUNNING)
            {
                nodeSelected = true;
                return Result.RUNNING;
            }
            else if (result == Result.SUCCESS)
            {
                currentChildIndex = 0;
                nodeSelected = false;
                ResetExecuted();
                return Result.SUCCESS;
            }
            else
            {
                currentChildIndex = 0;
                nodeSelected = false;
                ResetExecuted();
                return Result.FAILURE;
            }


        }

        public override void ResetNode()
        {
            base.ResetNode();
            ResetExecuted();
            currentChildIndex = 0;
            nodeSelected = false;
        }
    }
}