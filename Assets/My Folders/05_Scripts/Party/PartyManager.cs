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


    // ���Ŀ� CharacterManager Ŭ������ ���� �� Ŭ������ ĳ���� ���� ��ũ��Ʈ ���� �����ϵ��� ���� ��
    [SerializeField] GameObject partyMember_0;
    [SerializeField] GameObject partyMember_1;
    [SerializeField] GameObject partyMember_2;
    [SerializeField] GameObject partyMember_3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Inspector Test

    /*���Ŀ� �Է� �޴� �Ű����� partyMember �� UIâ�� ��ư ��� ������ ������ �Լ��� �����ϵ��� �Ѵ�. */
    [ContextMenu("StartMode_Input")]
    void ConvertInputBehavior(GameObject partyMember)
    {
        partyMember.transform.Find("BT").GetComponent<BehaviorTree>().StartBehavior(BehaviorMode.INPUT);
    }
    [ContextMenu("StartMode_Standard")]
    void ConvertStandardBehavior(GameObject partyMember)
    {
        partyMember.transform.Find("BT").GetComponent<BehaviorTree>().StartBehavior(BehaviorMode.STANDARD);
    }
    #endregion

}
