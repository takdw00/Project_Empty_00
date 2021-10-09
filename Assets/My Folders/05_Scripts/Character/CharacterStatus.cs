using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] private string character_Name; //ĳ���� �̸�

    public Experience experience;
    public AbilityStats abilityStats;
    public BattleStats battleStats;
    public BattleStats adjustedBattleStats;
}


[System.Serializable]
public struct Experience
{    ///����ġ
    public float character_Experience; //ĳ���� ����ġ

    public float exp_Vigor; //ü�� ����ġ
    public float exp_Endurance; //������ ����ġ
    public float exp_Attunement; //���߷� ����ġ
    public float exp_Strength; //�� ����ġ
    public float exp_Intelligence; //���� ����ġ
    public float exp_Dexterity; //�ⷮ ����ġ
    public float exp_Luck; //�� ����ġ
    public float exp_Charisma; //�ŷ� ����ġ
}


[System.Serializable]
public struct AbilityStats 
{
    public float vigor; //ü��
    public float endurance; //������
    public float attunement; //���߷�
    public float strength; //��
    public float intelligence; //����
    public float dexterity; //�ⷮ
    public float luck; //��
    public float charisma; //�ŷ�
}

[System.Serializable]
public struct BattleStats
{

    public float health_Point; //HP(�����)
    public float stamina_Point; //SP(������)
    public float mind_Point; //MP(���ŷ�)
    public float maximum_Weight; //�ִ� ����

    public float move_Speed; //�̵��ӵ�
    public float physical_Attack_Power; //���� ���ݷ�
    public float magic_Attack_Power; //���� ���ݷ�
    public float physical_Defense_Power; //���� ����
    public float magic_Defense_Power; //���� ����
    public float critical_Rate; //ũ��Ƽ�� Ȯ��
    public float critical_Attack_Resistance; //ũ��Ƽ�� ���׷�
    public float block; //������

    public float attack_Width_Range; //���ݹ���(��) - �ʿ� ���� ���� ����
    public float attack_Length_Range; //���ݹ���(����) - �ʿ� ���� ���� ����
    public float sight; //�þ�(ĳ���Ͱ� �� �� �ִ� �Ÿ�) - �ʿ� ���� ���� ����


    //test
    public float walkSpeed;
    public float runSpeed;
    //
} 