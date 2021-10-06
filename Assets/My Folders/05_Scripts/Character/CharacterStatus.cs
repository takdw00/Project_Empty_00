using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public AbilityStats abilityStats;
    public BattleStats battleStats;
    public BattleStats adjustedBattleStats;
}


[System.Serializable]
public struct AbilityStats 
{

}

[System.Serializable]
public struct BattleStats
{
    public float walkSpeed;
    public float runSpeed;
}