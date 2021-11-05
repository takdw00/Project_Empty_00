using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTSystem;



// ĳ������ �����ӿ� ���� �Լ����� ����
// �ִϸ����͸� �����̴� �۾��� ��� ���⼭ ����
// BT���� �� ĳ������Ʈ�ѿ� ����� �־� ĳ���͸� �����ϵ��� ��
// �ִϸ��̼ǿ��� ����ϴ� �ִϸ��̼� �̺�Ʈ �Լ��� �� Ŭ���� �� �Ϻ� �Լ����� �̿��ϵ��� ��

public class CharacterControl : MonoBehaviour
{
    protected delegate void VoidDelegate();

    //�ִϸ����Ϳ��� ����ϴ� Anim �Ķ���Ϳ� �־��ָ� �ش� �ִϸ��̼� �����. Basic�� ��κ� ȣȯ�Ǵ� �ִϸ��̼� ��ȣ��. �߰��� �ʿ��� ����� �� �Ļ� Ŭ������ ����
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
    protected NavMeshAgent navAgent; //��� ĳ���ʹ� �׺�޽� ��� �������� �����ϰ�, Ư�� ������ ��� ���������� ��Ȱ��ȭ
    protected Animator animator; //��� ĳ���ʹ� �׷����� ����ϴ� �ڽ� ������Ʈ���� �ִϸ����͸� ����

    //ĳ���� ���� ������Ʈ
    protected CharacterStatus status;
    
    //ĳ���Ͱ� �ٶ󺸴� ���� ����
    protected Vector3 lastFacingDirection; //ĳ���Ͱ� ���������� �̵��ϰų� �ٶ�ô� ����. 360�� ������ ������ �����̴�.
    protected bool lastFacedRight; //ĳ���Ͱ� ���������� �ٶ󺸴� ������ ������ ������ ������ �����̴�.(currentDirection���δ� ������ �Ұ��ϱ� ����)

    //���� ���� �ܺο��� ���� �����ϵ��� ��
    public Vector3 LastFacingDirection { get { return lastFacingDirection; } }
    public bool LastFacedRight { get { return lastFacedRight; } }
    //���⿡ ���� 4���� ���� ������ �ε��� ����� ���� �Լ�
    public virtual int GetDirectionIndex(Vector3 dir)
    {
        bool right = lastFacedRight;

        //�ݴ� �������� ������ ��ȯ�� ������ ���� ������ �ø��ϴ� ������ �ϴ� �����Դϴ�. ���� ���, ���� �Ʒ��θ� �̵��ϸ� ���� ������ �����մϴ�.
        if (lastFacedRight && lastFacingDirection.x < -0.1f)
        {
            right = false;
        }
        else if (!lastFacedRight && lastFacingDirection.x > 0.1f)
        {
            right = true;
        }

        //4���� ������ ���� ��ȣ�� ����ϴ�.
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


    //�׺�޽� ��� AI �̵� �� �ִϸ��̼� ������Ʈ �ֱ⿡ ������ �Ӵϴ�.
    protected float lastTimeNavmeshMoveMotionUpdated;
    protected AnimIndex_Basic lastNavMeshMoveMotion;

    //Attack ���� ���� ����
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
    //ĳ���� �׼��� ��κ� ���۵��� �ִϸ����� �Ķ���� �������� ����մϴ�.
    

    //������ �������� Direction�� �����ϰ� �ִϸ������� Direction �Ķ���͸� �����մϴ�.
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


    //���� �Ǵ� ���� �̵��� ���� �ִϸ��̼� ó���� �� �� �ֽ��ϴ�. �����Ǿ� �ִ��� navAgent�� ���� �ӵ��� ��ȭ�ϸ� �װͿ� ���� �̵� ó���� �̷�����ϴ�.
    //�׺�޽��� ���� �̵��ϴ� ĳ������ �ִϸ��̼� ó�� ����� �˴ϴ�.
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

    

    //��ǲ���� �̵��ϴ� ���Դϴ�.
    protected void Move(Vector3 dir, float speed)
    {
        navAgent.isStopped = true;
        navAgent.velocity = (dir * speed);
    }

    //���� �������� ĳ������ �ȱ� �ӵ��� �̵��մϴ�.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.walkSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.WALK);
    }

    //���� �������� ĳ������ �޸��� �ӵ��� �̵��մϴ�.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);
    }

    //���� �������� ĳ������ �������� �ӵ��� �̵��մϴ�.
    //������ Run�� ������ ������, ���¹̳� �Ҹ�, �̵��ӵ� ó��, �ִϸ��̼� ���� ���� ���߿� �����ؾ� �մϴ�.
    public void Sprint(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.sprintSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex_Basic.RUN);
    }

    //BT�� NavMesh�� ������� ���������� �޸���� �̵��մϴ�.
    public virtual BTSystem.Result MoveByNavmesh(Vector3 dest)
    {
        NavMeshPath path = new NavMeshPath();
        VoidDelegate CalculateNewPath =
            delegate
            {
                navAgent.CalculatePath(dest, path);
            };


        //�н��� ���ٸ�, �Ǵ� �������� �н��� �̹� �ִµ� ���� ������ Ÿ�� ��ġ�� ���� ������ �� �������� �Ӱ�ġ �̻� �ٸ��ٸ�
        if (!navAgent.hasPath || Vector3.SqrMagnitude(navAgent.destination - dest) > 1.0f)
        {
            //�� �н� ��길 �� �� �� ���¸� ����
            CalculateNewPath();

            //�н��� ���¿� ���� �ٸ� ó��
            switch (path.status)
            {
                //������ �� �ִ� �������� �н���� ������Ʈ�� ���ο� �н��� ���󰡰� ��
                case NavMeshPathStatus.PathComplete:
                    navAgent.SetPath(path);
                    if (navAgent.remainingDistance <= navAgent.stoppingDistance) 
                    {
                        //�� �н� ����. ���� �Ϸ�.
                        StopNavMeshMove();
                        NavMeshMoveMotionUpdate();
                        return Result.SUCCESS;
                    }
                    //�� �н� ����. �̵� ��.
                    navAgent.speed = status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio;
                    navAgent.isStopped = false;
                    NavMeshMoveMotionUpdate();
                    return Result.RUNNING;

                //������ ���̰ų� ��Ȱ��ȭ�� ���̶�� ������Ʈ�� �н��� �ʱ�ȭ�ϰ� �������� ����
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    StopNavMeshMove();
                    NavMeshMoveMotionUpdate();
                    //�� �н� ������. �̵� ����
                    return Result.FAILURE;
            }


        }
        else //�������� �н��� �ְ�, �� ������ �ٸ����� �ʴٸ�
        {
            //�н��� ��ȿ���� �˻�
            switch (navAgent.pathStatus)
            {
                case NavMeshPathStatus.PathComplete:
                    if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                    {
                        //���� �н� ����. ���� �Ϸ�.
                        StopNavMeshMove();
                        NavMeshMoveMotionUpdate();
                        return Result.SUCCESS;
                    }
                    else 
                    {
                        //���� �н� ����. �̵� ��.
                        NavMeshMoveMotionUpdate();
                        return Result.RUNNING;
                    }
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    //���� �н� ������. �̵� ����
                    StopNavMeshMove();
                    NavMeshMoveMotionUpdate();
                    return Result.FAILURE;
            }
        }

        return Result.FAILURE;
    }

    //NavAgent�� �̵��� �����ϰ� �ʱ�ȭ�մϴ�.
    public void StopNavMeshMove()
    {
        navAgent.destination = transform.position;
        navAgent.ResetPath();
        navAgent.isStopped = true;
    }

    //�׼��� ����� ������ �߻��� �ִϸ��̼� �̺�Ʈ�� �־��� ����
    protected virtual void OnActionEnd() 
    {
        animator.SetInteger("Anim", (int)AnimIndex_Basic.IDLE);
    }

    //������ �� �Ļ� Ŭ�������� �ٸ��� �����մϴ�. �׽�Ʈ�� ���� �⺻���� ����� �ҵ� �����Դϴ�.
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

    //���� ��� ���� �� �ִϸ��̼� �̺�Ʈ �ݹ��Դϴ�.
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

    //���� ��
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
