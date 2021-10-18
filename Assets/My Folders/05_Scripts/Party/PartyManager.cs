using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTSystem;

public class PartyManager : Singleton<PartyManager>
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

    public CharacterManager[] partyMembers;

    private void Start()
    {
        ChangePlayerCharacter(0);
    }

    //지정한 인덱스의 파티 멤버를 플레이어가 되도록 교체
    public void ChangePlayerCharacter(int memberIndex) 
    {
        for (int i = 0; i < 4; i++)
        {
            if (partyMembers[i] == null) 
            {
                continue;
            }

            if (i == memberIndex)
            {
                GlobalBlackboard.Instance.playerTransform = partyMembers[i].transform;
                partyMembers[i].BehaviorTree.StartBehavior(BehaviorMode.INPUT);
                partyMembers[i].CharacterNavAgent.avoidancePriority = 49;
            }
            else 
            {
                partyMembers[i].BehaviorTree.StartBehavior(BehaviorMode.STANDARD);
                partyMembers[i].CharacterNavAgent.avoidancePriority = 50;
            }
        }
    }

    #region Inspector Test

    [ContextMenu("SetPartyMember0ToPlayer")]
    public void SetPartyMember0ToPlayer() 
    {
        ChangePlayerCharacter(0);
    }

    [ContextMenu("SetPartyMember1ToPlayer")]
    public void SetPartyMember1ToPlayer()
    {
        ChangePlayerCharacter(1);
    }

    [ContextMenu("SetPartyMember2ToPlayer")]
    public void SetPartyMember2ToPlayer()
    {
        ChangePlayerCharacter(2);
    }

    [ContextMenu("SetPartyMember3ToPlayer")]
    public void SetPartyMember3ToPlayer()
    {
        ChangePlayerCharacter(3);
    }

    #endregion

}
