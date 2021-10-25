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
    private delegate void VoidDelegate();

    //�ִϸ����Ϳ��� ����ϴ� Anim �Ķ���Ϳ� �־��ָ� �ش� �ִϸ��̼� ���
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

    //���� ĳ���Ͱ� �ٶ󺸰� �ִ� ����
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
    private float lastTimeIdleMotionUpdated;//AI �̵� �� �ִϸ��̼� ������Ʈ �ֱ⿡ ������ �Ӵϴ�.
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
    //ĳ���� �׼��� ��κ� ���۵��� �ִϸ����� �Ķ���� �������� ����մϴ�.

    //���⿡ ���� Direction �Ķ������ ����� ����ϴ�.
    public int CalculateDirection(Vector3 dir) 
    {
        bool right = isFacingRight;

        //�ݴ� �������� ������ ��ȯ�� ������ �ø��մϴ�. ���� ���, ���� �Ʒ��θ� �̵��ϸ� ���� ������ �����մϴ�.
        if (isFacingRight && currentDirection.x < -0.1f)
        {
            right = false;
        }
        else if (!isFacingRight && currentDirection.x > 0.1f)
        {
            right = true;
        }

        //�ִϸ��̼ǿ��� ����� ������ �����մϴ�.
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

    //Direction�� �����ϰ� �ִϸ������� Direction �Ķ���͸� �����մϴ�.
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


    //�ַ� ���� �������� �����̰� ���� �ʴٸ� IDLE �Ǵ� READY�� ��� �ִϸ��̼����� ����ϴ� ������ �մϴ�.
    //���� �Ǵ� ���� �̵��� ���� �ִϸ��̼� ó���� �� �� �ֽ��ϴ�. �����Ǿ� �ִ��� navAgent�� ���� �ӵ��� ��ȭ�ϸ� �װͿ� ���� �̵� ó���� �̷�����ϴ�.
    //�׺�޽��� ���� �̵��ϴ� ĳ������ �ִϸ��̼� ó�� ����� �˴ϴ�.
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
    //���� �̵��ϴ� ���Դϴ�.
    private void Move(Vector3 dir, float speed)
    {
        navAgent.isStopped = true;
        navAgent.velocity = (dir * speed);
    }

    //���� �������� ĳ������ �ȱ� �ӵ��� �̵��մϴ�.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.walkSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.WALK);
    }

    //���� �������� ĳ������ �޸��� �ӵ��� �̵��մϴ�.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.RUN);
    }

    //���� �������� ĳ������ �������� �ӵ��� �̵��մϴ�.
    //������ Run�� ������ ������, ���¹̳� �Ҹ�, �̵��ӵ� ó��, �ִϸ��̼� ���� ���� ���߿� �����ؾ� �մϴ�.
    public void Sprint(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.sprintSpeedRatio);
        SetDirection(dir);
        animator.SetInteger("Anim", (int)AnimIndex.RUN);
    }

    //BT�� NavMesh�� ������� ���������� �޸���� �̵��մϴ�.
    public BTSystem.Result MoveByNavmesh(Vector3 dest)
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
                        StopNavMeshMove();
                        IdleAndMoveMotionUpdate();
                        Debug.Log("�� �н��� ����ߴµ� �����̰� �����߾��.");
                        return Result.SUCCESS;
                    }
                    navAgent.speed = status.adjustedBattleStats.move_Speed * status.adjustedBattleStats.runSpeedRatio;
                    navAgent.isStopped = false;
                    IdleAndMoveMotionUpdate();
                    Debug.Log("�� �н��� ����ߴµ� �����̰� �̵� ���̿���.");
                    return Result.RUNNING;

                //������ ���̰ų� ��Ȱ��ȭ�� ���̶�� ������Ʈ�� �н��� �ʱ�ȭ�ϰ� �������� ����
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    StopNavMeshMove();
                    IdleAndMoveMotionUpdate();
                    Debug.Log("�� �н��� ����ߴµ� �� �� ���� �������̶� ������.");
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
                        StopNavMeshMove();
                        IdleAndMoveMotionUpdate();
                        Debug.Log("���� �н��� �˻��ߴµ� �����̰� �����߾��.");
                        return Result.SUCCESS;
                    }
                    else 
                    {
                        IdleAndMoveMotionUpdate();
                        Debug.Log("���� �н��� �˻��ߴµ� �����̰� �̵� ���̿���.");
                        return Result.RUNNING;
                    }
                case NavMeshPathStatus.PathPartial:
                case NavMeshPathStatus.PathInvalid:
                    Debug.Log("���� �н��� �˻��ߴµ� �������� �Ǽ� ������.");
                    StopNavMeshMove();
                    IdleAndMoveMotionUpdate();
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

    //������ �� �Ļ� Ŭ�������� �ٸ��� �����մϴ�. �׽�Ʈ�� ���� �⺻���� ����� �ҵ� �����Դϴ�.
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

    //���� ��� ���� ���� �ִϸ��̼� �̺�Ʈ �ݹ��Դϴ�.(�ٸ� �׼��� ���Է��� ���� �� �ִ� ����)
    public void OnAttackEnding() 
    {

    }

    //���� ��� ���� �� �ִϸ��̼� �̺�Ʈ �ݹ��Դϴ�.
    public void OnAttackEnd() 
    {
        animator.SetInteger("Anim", (int)AnimIndex.IDLE);
        //nextAttackCombo++;
        timeSinceAttackEnd = 0.0f;
        isAttacking = false;
        currentReadyTime = 0.0f;
    }

    //���� ��
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

    //����
    public void Guard(Vector3 dir) 
    {
        if (!Mathf.Approximately(Vector3.SqrMagnitude(dir), 0.0f)) 
        {
            SetDirection(dir.normalized);
        }
        animator.SetInteger("Anim", (int)AnimIndex.GUARD);
        currentReadyTime = 0.0f;
    }

    //�и�
    public void Parry() 
    {

    }

    #endregion

}
