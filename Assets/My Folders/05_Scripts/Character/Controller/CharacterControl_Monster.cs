using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl_Monster : CharacterControl
{
    protected override void Start()
    {
        base.Start();
        gameObject.GetComponentInChildren<BTSystem.BehaviorTree>().StartStandardBehavior();
    }

    public override int GetDirectionIndex(Vector3 dir)
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

        //2���� ������ ���� ��ȣ�� ����ϴ�.
        int dirIndex;
        dirIndex = right ? 1 : 0;

        return dirIndex;
    }
}
