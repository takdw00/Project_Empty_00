using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTSystem;

public class PartyManager : Singleton<PartyManager>
{
    /*
         PartyManager
        -��Ƽ�� �Ҽӵ� ĳ���͵��� ������ ����
        -ĳ���� ����
        -���� �ൿ ��ħ ������ �� ����
        -�÷��̾ �ֿ켱 ��� ���� (����, ���, ����Ʈ, ����)
        ���� �����ϴ� ĳ���� ������ ����, ĳ������ Input, AI ���ۻ��¸� ����


        �ֿ���
        -��ü ĳ���� ����Ʈ�� ������.
        -��Ƽ �ɹ��� ������ �ִ´�.
        -��ü ĳ���� ����Ʈ���� ��Ƽ �ɹ��� ���� �� �� �ִ�.
        -��Ƽ �ɹ��� ���� ��Ƽ �ɹ����� �ʿ��� �������� �� �� �ִ�. (������ħ ������)


     */

    public CharacterManager[] partyMembers;

    private void Start()
    {
        ChangePlayerCharacter(0);
    }

    //������ �ε����� ��Ƽ ����� �÷��̾ �ǵ��� ��ü
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
