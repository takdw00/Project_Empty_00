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
    protected delegate void VoidDelegate();

    //애니메이터에서 사용하는 Anim 파라미터에 넣어주면 해당 애니메이션 재생함. Basic은 대부분 호환되는 애니메이션 번호임. 추가로 필요한 모션은 각 파생 클래스에 정의
    public enum AnimIndex_Basic
    {
        IDLE = 0,
        WALK = 1,
        RUN = 2,
        GUARD = 3,
        ATTACK = 4,
        READY = 5,
        HIT = 6,
        DEATH = 7
    }

    //Components Cache
    protected NavMeshAgent navAgent; //모든 캐릭터는 네브메쉬 기반 움직임을 수행하고, 특수 몬스터의 경우 예외적으로 비활성화
    protected Animator animator; //모든 캐릭터는 그래픽을 담당하는 자식 오브젝트에서 애니메이터를 가짐

    //캐릭터 정보 컴포넌트
    protected CharacterStatus status;
    
    //캐릭터가 바라보는 방향 저장
    protected Vector3 lastFacingDirection; //캐릭터가 마지막으로 이동하거나 바라봤던 방향. 360도 전방위 가능한 방향이다.
    protected bool lastFacedRight; //캐릭터가 마지막으로 바라보는 방향이 좌인지 우인지 저장한 방향이다.(currentDirection으로는 추출이 불가하기 때문)

    //방향 정보 외부에서 접근 가능하도록 함
    public Vector3 LastFacingDirection { get { return lastFacingDirection; } }
    public bool LastFacedRight { get { return lastFacedRight; } }
    //방향에 따른 4방향 기준 판정된 인덱스 기댓값을 얻어내는 함수
    public virtual int GetDirectionIndex(Vector3 dir)
    {
        bool right = lastFacedRight;

        //반대 방향으로 방향을 전환할 때에만 기존 방향을 플립하는 것으로 하는 판정입니다. 예를 들어, 위나 아래로만 이동하면 기존 방향을 유지합니다.
        if (lastFacedRight && lastFacingDirection.x < -0.1f)
        {
            right = false;
        }
        else if (!lastFacedRight && lastFacingDirection.x > 0.1f)
        {
            right = true;
        }

        //4분할 기준의 방향 번호를 얻습니다.
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


    //네브메쉬 기반 AI 이동 시 애니메이션 업데이트 주기에 제한을 둡니다.
    protected float lastTimeNavmeshMoveMotionUpdated;
    protected AnimIndex_Basic lastNavMeshMoveMotion;

    //Attack 관련 변수 저장
    [SerializeField] protected int maxComboAttackCount = 1;
    protected float timeSinceAttackEnd;
    protected int nextAttackCombo;
    protected bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } }

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
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
    

    //지정한 방향으로 Direction을 조정하고 애니메이터의 Direction 파라미터를 갱신합니다.
    protected virtual void SetDirection(Vector3 dir) 
    {
        lastFacingDirection = dir;

        int dirIndex = GetDirectionIndex(dir);

        if (dirIndex == 1 || dirIndex == 3)
        {
            lastFacedRight = true;
        }
        else 
        {
            lastFacedRight = false;
        }

        animator.SetInteger("Direction", dirIndex);
    }


    //정지 또는 작은 이동에 대한 애니메이션 처리를 할 수 있습니다. 정지되어 있더라도 navAgent에 의해 속도가 변화하면 그것에 맞춰 이동 처리가 이루어집니다.
    //네브메쉬를 통해 이동하는 캐릭터의 애니메이션 처리 결과가 됩니다.
    public virtual void NavMeshMoveMotionUpdate() 
    {
        float currentNavAgentSpeedSqr = Vector3.SqrMagnitude(navAgent.velocity);
        float moveSpeed = status.adjustedBattleStats.move_Speed;
        float walkSpeed = moveSpeed * status.adjustedBattleStats.walkSpeedRatio;
        float runSpeed = moveSpeed * status.adjustedBattleStats.runSpeedRatio;
        float walkRunBoundSpeed = (walkSpeed + runSpeed) / 2.0f;


        if (currentNavAgentSpeedSqr < 0.01f)
        {
            lastNavMeshMoveMotion = AnimIndex_Basic.IDLE;
            animator.SetInteger("Anim", (int)AnimIndex_Basic.IDLE);
            
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

    

    //인풋으로 이동하는 논리입니다.
    protected void Move(Vector3 dir, float speed)
    {
        navAgent.isStopped = true;
        navAgent.velocity = (dir * speed);
    }

    //지정 방향으로 캐릭터의 걷기 속도로 이동합니다.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.walkSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.WALK);
    }

    //지정 방향으로 캐릭터의 달리기 속도로 이동합니다.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);
    }

    //지정 방향으로 캐릭터의 전력질주 속도로 이동합니다.
    //지금은 Run과 내용이 같으며, 스태미나 소모, 이동속도 처리, 애니메이션 가속 등을 나중에 구현해야 합니다.
    public void Sprint(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.sprintSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);
    }

    //BT와 NavMesh를 기반으로 목적지까지 달리기로 이동합니다.
    public virtual BTSystem.Result MoveByNavmesh(Vector3 dest)
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
                        //새 패스 정상. 도착 완료.
                        StopNavMeshMove();
                        NavMeshMoveMotionUpdate();
                        return Result.SUCCESS;
                    }
                    //새 패스 정상. 이동 중.
                    navAgent.speed = status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio;
                    navAgent.isStopped = false;
                    NavMeshMoveMotionUpdate();
                    return Result.RUNNING;

                //끊어진 길이거나 비활성화된 길이라면 에이전트의 패스를 초기화하고 움직임을 멈춤
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    StopNavMeshMove();
                    NavMeshMoveMotionUpdate();
                    //새 패스 비정상. 이동 정지
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
                        //기존 패스 정상. 도착 완료.
                        StopNavMeshMove();
                        NavMeshMoveMotionUpdate();
                        return Result.SUCCESS;
                    }
                    else 
                    {
                        //기존 패스 정상. 이동 중.
                        NavMeshMoveMotionUpdate();
                        return Result.RUNNING;
                    }
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    //기존 패스 비정상. 이동 정지
                    StopNavMeshMove();
                    NavMeshMoveMotionUpdate();
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

    //액션이 종료된 시점에 발생할 애니메이션 이벤트로 넣어줄 내용
    protected virtual void OnActionEnd() 
    {
        animator.SetInteger("Anim", (int)AnimIndex_Basic.IDLE);
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

            animator.SetInteger("Anim", (int)AnimIndex_Basic.ATTACK);
            animator.SetInteger("Combo", nextAttackCombo);
            isAttacking = true;
        }
    }

    //공격 모션 종료 시 애니메이션 이벤트 콜백입니다.
    public virtual void OnAttackEnd() 
    {
        timeSinceAttackEnd = 0.0f;
        UpdateToNextComboAttack();
        OnActionEnd();
        isAttacking = false;
    }

    protected void UpdateToNextComboAttack() 
    {
        if (nextAttackCombo + 1 >= maxComboAttackCount)
        {
            nextAttackCombo = 0;
        }
        else 
        {
            nextAttackCombo++;
        }
    }

    //공격 후
    protected void ComboAttackTimeUpdate() 
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

    #endregion

}
