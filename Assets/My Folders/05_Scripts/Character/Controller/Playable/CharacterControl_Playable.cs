using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Playable 캐릭터의 특징
// 방향이 4방향 체계이다.
// 인풋에 의한 조작 함수를 가진다.

public class CharacterControl_Playable : CharacterControl, ICharacterControl_Dodgeable
{
    //IdleAndMove Variable
    public float battleReadyToIdleTime = 5.0f;
    protected float currentBattleReadyTime;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        currentBattleReadyTime = battleReadyToIdleTime;
    }

    protected override void Update()
    {
        base.Update();
        ComboAttackTimeUpdate();
        ReadyTimeUpdate();
    }

    protected void ResetReadyTime() 
    {
        currentBattleReadyTime = 0.0f;
    }

    private void ReadyTimeUpdate()
    {
        currentBattleReadyTime += Time.deltaTime;
    }

    //플레이어블은 Ready 자세 개념까지 들어감.
    public override void NavMeshMoveMotionUpdate()
    {
        float currentNavAgentSpeedSqr = Vector3.SqrMagnitude(navAgent.velocity);
        float moveSpeed = status.adjustedBattleStats.move_Speed;
        float walkSpeed = moveSpeed * status.adjustedBattleStats.walkSpeedRatio;
        float runSpeed = moveSpeed * status.adjustedBattleStats.runSpeedRatio;
        float walkRunBoundSpeed = (walkSpeed + runSpeed) / 2.0f;


        if (currentNavAgentSpeedSqr < 0.01f)
        {
            lastNavMeshMoveMotion = AnimIndex_Basic.IDLE;
            if (currentBattleReadyTime >= battleReadyToIdleTime)
            {
                animator.SetInteger("Anim", (int)AnimIndex_Basic.IDLE);
            }
            else
            {
                animator.SetInteger("Anim", (int)AnimIndex_Basic.READY);
            }

        }
        else
        {
            if (Time.time >= lastTimeNavmeshMoveMotionUpdated + 0.2f)
            {
                if (currentNavAgentSpeedSqr < walkRunBoundSpeed * walkRunBoundSpeed && lastNavMeshMoveMotion != AnimIndex_Basic.WALK)
                {
                    lastTimeNavmeshMoveMotionUpdated = Time.time;
                    lastNavMeshMoveMotion = AnimIndex_Basic.WALK;
                    animator.SetInteger("Anim", (int)AnimIndex_Basic.WALK);
                    SetDirection(navAgent.velocity);

                }
                else if (lastNavMeshMoveMotion != AnimIndex_Basic.RUN)
                {
                    lastTimeNavmeshMoveMotionUpdated = Time.time;
                    lastNavMeshMoveMotion = AnimIndex_Basic.RUN;
                    animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);
                    SetDirection(navAgent.velocity);
                }
                else 
                {
                    lastTimeNavmeshMoveMotionUpdated = Time.time;
                    SetDirection(navAgent.velocity);
                }
            }
        }
    }

    public virtual void Dodge(Vector3 dir) 
    {

    }

    public override void OnAttackEnd()
    {
        base.OnAttackEnd();
        OnActionEnd();
    }
    protected override void OnActionEnd()
    {
        base.OnActionEnd();
        ResetReadyTime();
    }
}
