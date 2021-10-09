using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] private string character_Name; //캐릭터 이름

    public Experience experience;
    public AbilityStats abilityStats;
    public BattleStats battleStats;
    public BattleStats adjustedBattleStats;
}


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

    public float health_Point; //HP(생명력)
    public float stamina_Point; //SP(지구력)
    public float mind_Point; //MP(정신력)
    public float maximum_Weight; //최대 무게

    public float move_Speed; //이동속도
    public float physical_Attack_Power; //물리 공격력
    public float magic_Attack_Power; //마법 공격력
    public float physical_Defense_Power; //물리 방어력
    public float magic_Defense_Power; //마법 방어력
    public float critical_Rate; //크리티컬 확률
    public float critical_Attack_Resistance; //크리티컬 저항력
    public float block; //가드율

    public float attack_Width_Range; //공격범위(폭) - 필요 없을 수도 있음
    public float attack_Length_Range; //공격범위(길이) - 필요 없을 수도 있음
    public float sight; //시야(캐릭터가 볼 수 있는 거리) - 필요 없을 수도 있음


    //test
    public float walkSpeed;
    public float runSpeed;
    //
} 