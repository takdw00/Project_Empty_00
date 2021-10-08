using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTSystem;

public class CharacterControl : MonoBehaviour
{
    public enum AnimIndex 
    {
        IDLE = 0,
        WALK,
        RUN,
        GUARD,
        ATTACK
    }

    //Components Cache
    protected NavMeshAgent navAgent;
    protected Rigidbody myRigidbody;
    protected BehaviorTree behaviorTree;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected CharacterStatus status;

    //현재 캐릭터가 바라보고 있는 방향
    protected Vector3 currentDirection;
    public Vector3 CurrentDirection { get { return currentDirection; } }
    protected bool isFacingRight;

    //Attack Variable
    private float timeSinceAttackEnd;
    private int nextAttackCombo;
    private bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } }


    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        myRigidbody = GetComponent<Rigidbody>();
        behaviorTree = GetComponentInChildren<BehaviorTree>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        status = GetComponent<CharacterStatus>();
    }

    protected virtual void Start() 
    {

    }

    protected virtual void Update()
    {
        ComboAttackTimeUpdate();
    }

    #region Character Actions
    //캐릭터 액션의 대부분 동작들은 애니메이터 파라미터 조정까지 담당합니다.

    //Direction을 조정하고 애니메이터의 Direction 파라미터를 갱신합니다.
    protected void SetDirection(Vector3 dir) 
    {
        currentDirection = dir;

        //반대 방향으로 방향을 전환할 때에만 플립합니다. 예를 들어, 위나 아래로만 이동하면 기존 방향을 유지합니다.
        if (isFacingRight && currentDirection.x < -0.1f)
        {
            isFacingRight = false;
        }
        else if(!isFacingRight && currentDirection.x > 0.1f)
        {
            isFacingRight = true;
        }

        //애니메이션에서 사용할 방향을 지정합니다.
        int dirIndex;
        if (currentDirection.z > 0.1f)
        {
            dirIndex = isFacingRight ? 3 : 2;
        }
        else 
        {
            dirIndex = isFacingRight ? 1 : 0;
        }


        animator.SetInteger("Direction", dirIndex);

    }


    //가만히 서 있는 동작을 수행합니다.
    public void Idle() 
    {
        animator.SetInteger("Anim", (int)AnimIndex.IDLE);
    }

    //Move
    //실제 이동하는 논리입니다.
    private void Move(Vector3 dir, float speed)
    {
        Vector3 targetPos = myRigidbody.position + (dir * speed * Time.deltaTime);
        myRigidbody.MovePosition(targetPos);
    }

    //지정 방향으로 캐릭터의 걷기 속도로 이동합니다.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.walkSpeed);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.WALK);
    }

    //지정 방향으로 캐릭터의 달리기 속도로 이동합니다.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.runSpeed);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.RUN);
    }

    //NavMesh를 기반으로 목적지까지 달리기로 이동합니다.
    public bool MoveByNavmesh(Vector3 dest)
    {
        if (navAgent.SetDestination(dest))
        {
            navAgent.speed = status.adjustedBattleStats.runSpeed;
            navAgent.isStopped = false;
            SetDirection(navAgent.desiredVelocity.normalized);
            animator.SetInteger("Anim", (int)AnimIndex.RUN);
            return true;
        }

        return false;
    }

    //NavMesh 기반 이동을 정지합니다.
    public void StopNavMeshMove()
    {
        navAgent.isStopped = true;
        animator.SetInteger("Anim", (int)AnimIndex.IDLE);
    }

    //공격은 각 파생 클래스마다 다르게 구현합니다. 테스트를 위해 기본형은 기사의 소드 공격입니다.
    public virtual void Attack(Vector3 dir) 
    {
        if (!isAttacking) 
        {
            if (!Mathf.Approximately(Vector3.SqrMagnitude(dir), 0.0f))
            {
                SetDirection(dir.normalized);
            }

            animator.SetInteger("Anim", (int)AnimIndex.ATTACK);
            animator.SetInteger("Combo", nextAttackCombo);
            isAttacking = true;
        }
    }

    public void OnAttackEnding() 
    {

    }

    //공격 모션 종료 시 콜백입니다.
    public void OnAttackEnd() 
    {
        animator.SetInteger("Anim", (int)AnimIndex.IDLE);
        //nextAttackCombo++;
        timeSinceAttackEnd = 0.0f;
        isAttacking = false;
    }

    //공격 후
    private void ComboAttackTimeUpdate() 
    {
        if (!isAttacking) 
        {
            timeSinceAttackEnd += Time.deltaTime;
        }
        if (timeSinceAttackEnd >= 0.2f) 
        {
            timeSinceAttackEnd = 0.0f;
            nextAttackCombo = 0;
        }
    }

    //가드
    public void Guard(Vector3 dir) 
    {
        if (!Mathf.Approximately(Vector3.SqrMagnitude(dir), 0.0f)) 
        {
            SetDirection(dir.normalized);
        }
        animator.SetInteger("Anim", (int)AnimIndex.GUARD);
    }

    //패리
    public void Parry() 
    {

    }

    #endregion

}
