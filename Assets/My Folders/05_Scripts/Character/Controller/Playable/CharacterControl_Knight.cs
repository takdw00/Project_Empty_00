using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl_Knight : CharacterControl_Playable, ICharacterControl_Guardable
{
    //°¡µå
    public void Guard(Vector3 dir)
    {
        if (!Mathf.Approximately(Vector3.SqrMagnitude(dir), 0.0f))
        {
            SetDirection(dir.normalized);
        }
        animator.SetInteger("Anim", (int)AnimIndex_Basic.GUARD);
        currentBattleReadyTime = 0.0f;
    }
}
