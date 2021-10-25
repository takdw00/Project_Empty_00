using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTSystem;



// 캐릭터의 움직임에 관한 함수들을 정의
// 애니메이터를 움직이는 작업도 모두 여기서 진행
// BT에서 이 캐릭터컨트롤에 명령을 주어 캐릭터를 조작하도록 함
// 애니메이션에서 사용하는 애니메이션 이벤트 함수도 이 클래스 내 일부 함수들을 이용하도록 함

public class CharacterControl : MonoBehaviour
{
    private delegate void VoidDelegate();

    //애니메이터에서 사용하는 Anim 파라미터에 넣어주면 해당 애니메이션 재상
    public enum AnimIndex 
    {
        IDLE = 0,
        WALK,
        RUN,
        GUARD,
        ATTACK,
        READY
    }

    //Components Cache
    protected NavMeshAgent navAgent;
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

    //IdleAndMove Variable
    public float readyToIdleTime = 5.0f;
    private float currentReadyTime;
    private float lastTimeIdleMotionUpdated;//AI 이동 시 애니메이션 업데이트 주기에 제한을 둡니다.
    private float lastTimeIdleMotionDirUpdated;
    private AnimIndex currentNavmeshMoveMotion;


    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        behaviorTree = GetComponentInChildren<BehaviorTree>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        status = GetComponent<CharacterStatus>();
    }

    protected virtual void Start() 
    {
        currentReadyTime = readyToIdleTime;
    }

    protected virtual void Update()
    {
        ComboAttackTimeUpdate();
        ReadyTimeUpdate();
    }

    #region Character Actions
    //캐릭터 액션의 대부분 동작들은 애니메이터 파라미터 조정까지 담당합니다.

    //방향에 따른 Direction 파라미터의 기댓값을 얻습니다.
    public int CalculateDirection(Vector3 dir) 
    {
        bool right = isFacingRight;

        //반대 방향으로 방향을 전환할 때에만 플립합니다. 예를 들어, 위나 아래로만 이동하면 기존 방향을 유지합니다.
        if (isFacingRight && currentDirection.x < -0.1f)
        {
            right = false;
        }
        else if (!isFacingRight && currentDirection.x > 0.1f)
        {
            right = true;
        }

        //애니메이션에서 사용할 방향을 지정합니다.
        int dirIndex;
        if (dir.z > 0.1f)
        {
            dirIndex = right ? 3 : 2;
        }
        else
        {
            dirIndex = right ? 1 : 0;
        }

        return dirIndex;
    }

    //Direction을 조정하고 애니메이터의 Direction 파라미터를 갱신합니다.
    protected void SetDirection(Vector3 dir) 
    {
        currentDirection = dir;
        int dirIndex = CalculateDirection(dir);

        if (dirIndex == 1 || dirIndex == 3)
        {
            isFacingRight = true;
        }
        else 
        {
            isFacingRight = false;
        }

        animator.SetInteger("Direction", dirIndex);
    }


    //주로 무브 로직에서 움직이고 있지 않다면 IDLE 또는 READY를 골라 애니메이션으로 출력하는 역할을 합니다.
    //정지 또는 작은 이동에 대한 애니메이션 처리를 할 수 있습니다. 정지되어 있더라도 navAgent에 의해 속도가 변화하면 그것에 맞춰 이동 처리가 이루어집니다.
    //네브메쉬를 통해 이동하는 캐릭터의 애니메이션 처리 결과가 됩니다.
    public void IdleAndMoveMotionUpdate() 
    {
        float speedSqr = Vector3.SqrMagnitude(navAgent.velocity);
        float moveSpeed = status.adjustedBattleStats.move_Speed;
        float walkSpeed = moveSpeed * status.adjustedBattleStats.walkSpeedRatio;
        float runSpeed = moveSpeed * status.adjustedBattleStats.runSpeedRatio;
        float walkRunBoundSpeed = (walkSpeed + runSpeed) / 2.0f;


        if (speedSqr < 0.01f)
        {
            currentNavmeshMoveMotion = AnimIndex.IDLE;

            if (currentReadyTime >= readyToIdleTime)
            {
                animator.SetInteger("Anim", (int)AnimIndex.IDLE);
            }
            else
            {
                animator.SetInteger("Anim", (int)AnimIndex.READY);
            }
        }
        else 
        {
            if (Time.time >= lastTimeIdleMotionUpdated + 0.2f)
            {
                if (speedSqr < walkRunBoundSpeed * walkRunBoundSpeed && currentNavmeshMoveMotion != AnimIndex.WALK)
                {
                    lastTimeIdleMotionUpdated = Time.time;
                    currentNavmeshMoveMotion = AnimIndex.WALK;
                    animator.SetInteger("Anim", (int)AnimIndex.WALK);

                }
                else if(currentNavmeshMoveMotion != AnimIndex.RUN)
                {
                    lastTimeIdleMotionUpdated = Time.time;
                    currentNavmeshMoveMotion = AnimIndex.RUN;
                    animator.SetInteger("Anim", (int)AnimIndex.RUN);
                }
            }

            if (Time.time >= lastTimeIdleMotionDirUpdated + 0.2f) 
            {
                lastTimeIdleMotionDirUpdated = Time.time;
                SetDirection(navAgent.velocity);
            }
        }
    }

    private void ReadyTimeUpdate() 
    {
        currentReadyTime += Time.deltaTime;
    }

    //Move
    //실제 이동하는 논리입니다.
    private void Move(Vector3 dir, float speed)
    {
        navAgent.isStopped = true;
        navAgent.velocity = (dir * speed);
    }

    //지정 방향으로 캐릭터의 걷기 속도로 이동합니다.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.walkSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.WALK);
    }

    //지정 방향으로 캐릭터의 달리기 속도로 이동합니다.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.RUN);
    }

    //지정 방향으로 캐릭터의 전력질주 속도로 이동합니다.
    //지금은 Run과 내용이 같으며, 스태미나 소모, 이동속도 처리, 애니메이션 가속 등을 나중에 구현해야 합니다.
    public void Sprint(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.sprintSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.RUN);
    }

    //BT와 NavMesh를 기반으로 목적지까지 달리기로 이동합니다.
    public BTSystem.Result MoveByNavmesh(Vector3 dest)
    {
        NavMeshPath path = new NavMeshPath();
        VoidDelegate CalculateNewPath =
            delegate
            {
                navAgent.CalculatePath(dest, path);
            };


        //패스가 없다면, 또는 정상적인 패스가 이미 있는데 현재 지정한 타겟 위치와 과거 지정해 둔 목적지가 임계치 이상 다르다면
        if (!navAgent.hasPath || Vector3.SqrMagnitude(navAgent.destination - dest) > 1.0f)
        {
            //새 패스 계산만 한 후 그 상태를 저장
            CalculateNewPath();

            //패스의 상태에 따라 다른 처리
            switch (path.status)
            {
                //도달할 수 있는 정상적인 패스라면 에이전트가 새로운 패스로 따라가게 함
                case NavMeshPathStatus.PathComplete:
                    navAgent.SetPath(path);
                    if (navAgent.remainingDistance <= navAgent.stoppingDistance) 
                    {
                        StopNavMeshMove();
                        IdleAndMoveMotionUpdate();
                        Debug.Log("새 패스를 계산했는데 정상이고 도착했어요.");
                        return Result.SUCCESS;
                    }
                    navAgent.speed = status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio;
                    navAgent.isStopped = false;
                    IdleAndMoveMotionUpdate();
                    Debug.Log("새 패스를 계산했는데 정상이고 이동 중이에요.");
                    return Result.RUNNING;

                //끊어진 길이거나 비활성화된 길이라면 에이전트의 패스를 초기화하고 움직임을 멈춤
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    StopNavMeshMove();
                    IdleAndMoveMotionUpdate();
                    Debug.Log("새 패스를 계산했는데 갈 수 없는 비정상이라 멈췄어요.");
                    return Result.FAILURE;
            }


        }
        else //정상적인 패스가 있고, 또 기존과 다르지도 않다면
        {
            //패스의 유효성을 검사
            switch (navAgent.pathStatus)
            {
                case NavMeshPathStatus.PathComplete:
                    if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                    {
                        StopNavMeshMove();
                        IdleAndMoveMotionUpdate();
                        Debug.Log("기존 패스를 검사했는데 정상이고 도착했어요.");
                        return Result.SUCCESS;
                    }
                    else 
                    {
                        IdleAndMoveMotionUpdate();
                        Debug.Log("기존 패스를 검사했는데 정상이고 이동 중이에요.");
                        return Result.RUNNING;
                    }
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    Debug.Log("기존 패스를 검사했는데 비정상이 되서 멈췄어요.");
                    StopNavMeshMove();
                    IdleAndMoveMotionUpdate();
                    return Result.FAILURE;
            }
        }

        return Result.FAILURE;
    }

    //NavAgent의 이동을 정지하고 초기화합니다.
    public void StopNavMeshMove()
    {
        navAgent.destination = transform.position;
        navAgent.ResetPath();
        navAgent.isStopped = true;
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

    //공격 모션 종료 직전 애니메이션 이벤트 콜백입니다.(다른 액션의 선입력을 받을 수 있는 상태)
    public void OnAttackEnding() 
    {

    }

    //공격 모션 종료 시 애니메이션 이벤트 콜백입니다.
    public void OnAttackEnd() 
    {
        animator.SetInteger("Anim", (int)AnimIndex.IDLE);
        //nextAttackCombo++;
        timeSinceAttackEnd = 0.0f;
        isAttacking = false;
        currentReadyTime = 0.0f;
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
        currentReadyTime = 0.0f;
    }

    //패리
    public void Parry() 
    {

    }

    #endregion

}
