using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTSystem;

public class PartyManager : MonoBehaviour
{


    /*
         PartyManager
        -파티에 소속된 캐릭터들을 가지고 있음
        -캐릭터 영입
        -전투 행동 지침 선택할 수 있음
        -플레이어가 최우선 명령 가능 (공격, 방어, 서포트, 집합)
        현재 조작하는 캐릭터 정보를 가짐, 캐릭터의 Input, AI 조작상태를 변경


        주요기능
        -전체 캐릭터 리스트를 가진다.
        -파티 맴버를 가지고 있는다.
        -전체 캐릭터 리스트에서 파티 맴버를 선택 할 수 있다.
        -파티 맴버에 관해 파티 맴버에게 필요한 설정등을 할 수 있다. (전투방침 같은거)

     */


    [SerializeField] CharacterManager []character; // 전체 캐릭터 리스트

    // 이후에 CharacterManager 클래스를 만들어서 그 클래스로 캐릭터 관련 스크립트 접근 가능하도록 만들 것
    [SerializeField] CharacterManager partyMember_1; // UI에서 1번으로 표시 할 것, 시각적인 편의성을 위해 1번이라 표시
    [SerializeField] CharacterManager partyMember_2;
    [SerializeField] CharacterManager partyMember_3;
    [SerializeField] CharacterManager partyMember_4;

    // Start is called before the first frame update
    void Start()
    {
        #region Test Code

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Test Code
        if(Input.GetKey(KeyCode.P))
        {
            partyMember_1.BehaviorTree.StartBehavior(BehaviorMode.INPUT);
        }
        if (Input.GetKey(KeyCode.O))
        {
            partyMember_1.BehaviorTree.StartBehavior(BehaviorMode.STANDARD);
        }
        #endregion
    }

    #region Inspector Test

    /*이후에 입력 받는 매개변수 partyMember 는 UI창의 버튼 등에서 정보를 가져와 함수를 실행하도록 한다. */
    [ContextMenu("StartMode_Input")]
    void ConvertInputBehavior(/*CharacterManager partyMember*/)
    {
        partyMember_1.BehaviorTree.StartBehavior(BehaviorMode.INPUT);
        //partyMember.BehaviorTree.StartBehavior(BehaviorMode.INPUT);
    }
    [ContextMenu("StartMode_Standard")]
    void ConvertStandardBehavior(/*CharacterManager partyMember*/)
    {
        partyMember_1.BehaviorTree.StartBehavior(BehaviorMode.STANDARD);
        //partyMember.BehaviorTree.StartBehavior(BehaviorMode.STANDARD);
    }
    #endregion

}
