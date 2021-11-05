using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTSystem
{
    // ���� ������
    // ������ �����ϴ� ������ �������Դϴ�. �������� ������ ����˴ϴ�.
    // �׼� ��� �� ������ ������ ���� ����� ���ݵ� �ֽ��ϴ�.
    // ������Ʈ ���°� �ƴմϴ�. MonoBehavior�� ��ӹ��� �ʴ� �Ϲ� Ŭ���� �����Դϴ�.
    public class CharacterBlackboard
    {
        
        // Component Cache
        // BehaviorTree�� InitBlackboard�Լ��� �̿��� ĳ���մϴ�.
        public CharacterControl characterControl;

        // Data
        public CampType CampIndex; //����, ��ȣ�� �Ǻ��ϴ� ������ �ǹ̷� ķ����� �ܾ ����Ͽ���.
        public Transform followTarget;// ���� Ÿ���� ��Ÿ���ϴ�.
        public Vector3 followTargetOffset; // �Ϲ������� ���� ��ġ�� followTarget.position������ ��Ȯ�� �� ������ �ƴ� ���� �ֽ��ϴ�.
    }
}