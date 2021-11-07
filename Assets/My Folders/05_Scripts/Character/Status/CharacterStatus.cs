using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStatus : MonoBehaviour
{
    protected CharacterManager character;

    [SerializeField] protected string character_Name; //캐릭터 이름

    [SerializeField] protected CampType camp; //캐릭터 진영, 초기값 설정 시 그대로 적용
    public CampType Camp { get { return camp; } }

    public BattleStats battleStats;
    public BattleStats adjustedBattleStats;

    protected float health_Point; //HP(생명력)
    protected float stamina_Point; //SP(지구력)
    protected float mind_Point; //MP(정신력)
    public float HP { get { return health_Point; } }
    public float SP { get { return health_Point; } }
    public float MP { get { return health_Point; } }

    //이벤트 리스트(이곳에 접근해서 추가하고자 하는 메소드 추가)
    public UnityEventFloat OnDamaged; //최종적으로 받은 데미지를 매개변수로
    public UnityEventFloat OnGroggyDamaged;
    public UnityEvent OnDeath;
    public UnityEventFloat OnHPIncreased; //최종적으로 증가한 체력량을 매개변수로
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

    //지정한 스탯을 안전하게 증가, 감소 처리하고 증가, 감소에 따른 이벤트를 발생시킵니다.
    protected void AddPoint(float amount, ref float point, float max, UnityEventFloat increaseCallback, UnityEventFloat decreaseCallback)
    {
        if (amount >= 0.0f)
        {
            //증가할 때에는 최댓값을 넘을 시 증가량을 조정
            float resultAmount = max < amount + point ? 0.0f : amount;
            point = Mathf.Clamp(point + resultAmount, 0.0f, max);

            //실제 증가한 양을 기반으로 이벤트 실행
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

    //데미지 계산 공식에 의해 받을 데미지를 결정
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

    //데미지와 그로기 데미지를 동시에 받음
    public void TakeHitDamage(float damage, float groggyDamage) 
    {
        TakeDamage(damage);
        TakeGroggyDamage(groggyDamage);
    }

    //데미지 받기 명령(수치적으로. 피격 모션 처리는 다른 곳에서.)
    public void TakeDamage(float damage) 
    {
        float resultDamage = CalculateDamage(damage);

        OnDamaged.Invoke(resultDamage);
    }

    //기절 데미지 받기 명령
    public void TakeGroggyDamage(float damage)
    {
        float resultDamage = CalculateDamage(damage);

        OnGroggyDamaged.Invoke(resultDamage);
    }

    public void Death() 
    {
        OnDeath.Invoke();
    }

    //캠프를 변경하고자 할 때 이 함수를 사용
    public void ChangeCamp(CampType _camp) 
    {
        camp = _camp;
        character.BehaviorTree.Blackboard.CampIndex = _camp;
    }
}

//우호, 적대 판별의 의미로 사용하는 Camp. 무분별하게 늘이지 말 것.
public enum CampType { PLAYER, MONSTER, OTHER }

[System.Serializable]
public struct Experience
{    ///경험치
    public float character_Experience; //캐릭터 경험치

    public float exp_Vigor; //체력 경험치
    public float exp_Endurance; //지구력 경험치
    public float exp_Attunement; //집중력 경험치
    public float exp_Strength; //힘 경험치
    public float exp_Intelligence; //지력 경험치
    public float exp_Dexterity; //기량 경험치
    public float exp_Luck; //운 경험치
    public float exp_Charisma; //매력 경험치
}


[System.Serializable]
public struct AbilityStats 
{
    public float vigor; //체력
    public float endurance; //지구력
    public float attunement; //집중력
    public float strength; //힘
    public float intelligence; //지력
    public float dexterity; //기량
    public float luck; //운
    public float charisma; //매력
}

[System.Serializable]
public struct BattleStats
{
    public float max_health_Point; //최대 HP
    public float max_stamina_Point; //최대 SP
    public float max_mind_Point; // 최대 MP
    public float max_Weight; //최대 무게
    public float move_Speed; //이동속도
    public float physical_Attack_Power; //물리 공격력
    public float magic_Attack_Power; //마법 공격력
    public float physical_Defense_Power; //물리 방어력
    public float magic_Defense_Power; //마법 방어력
    public float critical_Rate; //크리티컬 확률
    public float critical_Attack_Resistance; //크리티컬 저항력
    public float block_Rate; //가드율

    //이동속도에 곱해지는 배율
    public float walkSpeedRatio;
    public float runSpeedRatio;
    public float sprintSpeedRatio;
} 