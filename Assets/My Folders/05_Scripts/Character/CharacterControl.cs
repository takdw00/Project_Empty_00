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

    //���� ĳ���Ͱ� �ٶ󺸰� �ִ� ����
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
    //ĳ���� �׼��� ��κ� ���۵��� �ִϸ����� �Ķ���� �������� ����մϴ�.

    //Direction�� �����ϰ� �ִϸ������� Direction �Ķ���͸� �����մϴ�.
    protected void SetDirection(Vector3 dir) 
    {
        currentDirection = dir;
    }


    //Move
    //���� �̵��ϴ� ���Դϴ�.
    private void Move(Vector3 dir, float speed)
    {
        Vector3 targetPos = myRigidbody.position + (dir * speed * Time.deltaTime);
        myRigidbody.MovePosition(targetPos);
    }

    //���� �������� ĳ������ �ȱ� �ӵ��� �̵��մϴ�.
    public void Walk(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.walkSpeed);
    }

    //���� �������� ĳ������ �޸��� �ӵ��� �̵��մϴ�.
    public void Run(Vector3 dir) 
    {
        Move(dir, status.adjustedBattleStats.runSpeed);
    }

    //NavMesh�� ������� ���������� �޸���� �̵��մϴ�.
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

    //NavMesh ��� �̵��� �����մϴ�.
    public void StopNavMeshMove()
    {
        navAgent.isStopped = true;
    }

    //������ �� �Ļ� Ŭ�������� �ٸ��� �����մϴ�. �׽�Ʈ�� ���� �⺻���� ����� �ҵ� �����Դϴ�.
    public virtual void Attack() 
    {

    }

    //����
    public void Guard() 
    {

    }

    //�и�
    public void Parry() 
    {

    }

    #endregion

}
