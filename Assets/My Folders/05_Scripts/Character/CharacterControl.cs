using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTSystem;

public class CharacterControl : MonoBehaviour
{
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

    }

    #region Character Actions
    //캐릭터 액션의 대부분 동작들은 애니메이터 파라미터 조정까지 담당합니다.

    //Direction을 조정하고 애니메이터의 Direction 파라미터를 갱신합니다.
    protected void SetDirection(Vector3 dir) 
    {
        currentDirection = dir;
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
    }

    //지정 방향으로 캐릭터의 달리기 속도로 이동합니다.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.runSpeed);
    }

    //NavMesh를 기반으로 목적지까지 달리기로 이동합니다.
    public bool MoveByNavmesh(Vector3 dest)
    {
        if (navAgent.SetDestination(dest))
        {
            navAgent.speed = status.adjustedBattleStats.runSpeed;
            navAgent.isStopped = false;
            return true;
        }

        return false;
    }

    //NavMesh 기반 이동을 정지합니다.
    public void StopNavMeshMove()
    {
        navAgent.isStopped = true;
    }

    //공격은 각 파생 클래스마다 다르게 구현합니다. 테스트를 위해 기본형은 기사의 소드 공격입니다.
    public virtual void Attack() 
    {

    }

    //가드
    public void Guard() 
    {

    }

    //패리
    public void Parry() 
    {

    }

    #endregion

}
