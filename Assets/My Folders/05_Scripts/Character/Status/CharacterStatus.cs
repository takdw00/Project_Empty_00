using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStatus : MonoBehaviour
{
    protected CharacterManager character;

    [SerializeField] protected string character_Name; //ĳ���� �̸�

    [SerializeField] protected CampType camp; //ĳ���� ����, �ʱⰪ ���� �� �״�� ����
    public CampType Camp { get { return camp; } }

    public BattleStats battleStats;
    public BattleStats adjustedBattleStats;

    protected float health_Point; //HP(�����)
    protected float stamina_Point; //SP(������)
    protected float mind_Point; //MP(���ŷ�)
    public float HP { get { return health_Point; } }
    public float SP { get { return health_Point; } }
    public float MP { get { return health_Point; } }

    //�̺�Ʈ ����Ʈ(�̰��� �����ؼ� �߰��ϰ��� �ϴ� �޼ҵ� �߰�)
    public UnityEventFloat OnDamaged; //���������� ���� �������� �Ű�������
    public UnityEventFloat OnGroggyDamaged;
    public UnityEvent OnDeath;
    public UnityEventFloat OnHPIncreased; //���������� ������ ü�·��� �Ű�������
    public UnityEventFloat OnSPIncreased;
    public UnityEventFloat OnMPIncreased;
    public UnityEventFloat OnHPDecreased;
    public UnityEventFloat OnSPDecreased;
    public UnityEventFloat OnMPDecreased;

    protected virtual void Awake() 
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start() 
    {
        ChangeCamp(camp);
    }

    //������ ������ �����ϰ� ����, ���� ó���ϰ� ����, ���ҿ� ���� �̺�Ʈ�� �߻���ŵ�ϴ�.
    protected void AddPoint(float amount, ref float point, float max, UnityEventFloat increaseCallback, UnityEventFloat decreaseCallback)
    {
        if (amount >= 0.0f)
        {
            //������ ������ �ִ��� ���� �� �������� ����
            float resultAmount = max < amount + point ? 0.0f : amount;
            point = Mathf.Clamp(point + resultAmount, 0.0f, max);

            //���� ������ ���� ������� �̺�Ʈ ����
            increaseCallback.Invoke(resultAmount);
        }
        else 
        {
            point = Mathf.Clamp(point + amount, 0.0f, max);
            decreaseCallback.Invoke(amount);
        }
    }

    public void AddHP(float amount) 
    {
        AddPoint(amount, ref health_Point, adjustedBattleStats.max_health_Point, OnHPIncreased, OnHPDecreased);
    }

    public void AddSP(float amount) 
    {
        AddPoint(amount, ref stamina_Point, adjustedBattleStats.max_stamina_Point, OnSPIncreased, OnSPDecreased);
    }

    public void AddMP(float amount) 
    {
        AddPoint(amount, ref mind_Point, adjustedBattleStats.max_mind_Point, OnMPIncreased, OnMPDecreased);
    }

    //������ ��� ���Ŀ� ���� ���� �������� ����
    protected float CalculateDamage(float damage) 
    {
        float result = damage;
        return result;
    }

    protected float CalculateGroggyDamage(float damage) 
    {
        float result = damage;
        return result;
    }

    //�������� �׷α� �������� ���ÿ� ����
    public void TakeHitDamage(float damage, float groggyDamage) 
    {
        TakeDamage(damage);
        TakeGroggyDamage(groggyDamage);
    }

    //������ �ޱ� ���(��ġ������. �ǰ� ��� ó���� �ٸ� ������.)
    public void TakeDamage(float damage) 
    {
        float resultDamage = CalculateDamage(damage);

        OnDamaged.Invoke(resultDamage);
    }

    //���� ������ �ޱ� ���
    public void TakeGroggyDamage(float damage)
    {
        float resultDamage = CalculateDamage(damage);

        OnGroggyDamaged.Invoke(resultDamage);
    }

    public void Death() 
    {
        OnDeath.Invoke();
    }

    //ķ���� �����ϰ��� �� �� �� �Լ��� ���
    public void ChangeCamp(CampType _camp) 
    {
        camp = _camp;
        character.BehaviorTree.Blackboard.CampIndex = _camp;
    }
}

//��ȣ, ���� �Ǻ��� �ǹ̷� ����ϴ� Camp. ���к��ϰ� ������ �� ��.
public enum CampType { PLAYER, MONSTER, OTHER }

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
    public float max_health_Point; //�ִ� HP
    public float max_stamina_Point; //�ִ� SP
    public float max_mind_Point; // �ִ� MP
    public float max_Weight; //�ִ� ����
    public float move_Speed; //�̵��ӵ�
    public float physical_Attack_Power; //���� ���ݷ�
    public float magic_Attack_Power; //���� ���ݷ�
    public float physical_Defense_Power; //���� ����
    public float magic_Defense_Power; //���� ����
    public float critical_Rate; //ũ��Ƽ�� Ȯ��
    public float critical_Attack_Resistance; //ũ��Ƽ�� ���׷�
    public float block_Rate; //������

    //�̵��ӵ��� �������� ����
    public float walkSpeedRatio;
    public float runSpeedRatio;
    public float sprintSpeedRatio;
} 