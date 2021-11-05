using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // 개인 블랙보드
    // 개인이 소유하는 형태의 블랙보드입니다. 개인적인 값들이 저장됩니다.
    // 액션 노드 간 데이터 전달을 위한 통로의 성격도 있습니다.
    // 컴포넌트 형태가 아닙니다. MonoBehavior를 상속받지 않는 일반 클래스 형태입니다.
    public class CharacterBlackboard
    {
        
        // Component Cache
        // BehaviorTree의 InitBlackboard함수를 이용해 캐시합니다.
        public CharacterControl characterControl;

        // Data
        public CampType CampIndex; //적대, 우호를 판별하는 진영의 의미로 캠프라는 단어를 사용하였음.
        public Transform followTarget;// 따라갈 타겟을 나타냅니다.
        public Vector3 followTargetOffset; // 일반적으로 따라갈 위치는 followTarget.position이지만 정확히 그 지점은 아닐 수도 있습니다.
    }
}