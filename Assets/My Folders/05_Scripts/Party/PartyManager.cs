using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTSystem;

public class PartyManager : MonoBehaviour
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


    [SerializeField] CharacterManager []character; // ��ü ĳ���� ����Ʈ

    // ���Ŀ� CharacterManager Ŭ������ ���� �� Ŭ������ ĳ���� ���� ��ũ��Ʈ ���� �����ϵ��� ���� ��
    [SerializeField] CharacterManager partyMember_1; // UI���� 1������ ǥ�� �� ��, �ð����� ���Ǽ��� ���� 1���̶� ǥ��
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

    /*���Ŀ� �Է� �޴� �Ű����� partyMember �� UIâ�� ��ư ��� ������ ������ �Լ��� �����ϵ��� �Ѵ�. */
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
