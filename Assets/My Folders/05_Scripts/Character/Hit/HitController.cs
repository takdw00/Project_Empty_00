using UnityEngine;

// �� �� ��������� Ÿ���� ������ ����ϴ� Ŭ�����Դϴ�.
// Ÿ���� ������ �����ϴ� ���Դϴ�.
// ������ Ÿ���� �޴� ó���� �� ��ũ��Ʈ�� �����ϴ�.(characterControl�� ��Ʈ ��� ���, characterStatus�� ������ ���� ���)
public class HitController : MonoBehaviour
{
    public UnityEventFloatFloat OnWeakHit;
    public UnityEventFloatFloat OnNormalHit;
    public UnityEventFloatFloat OnStrongHit;

    //��ǿ� ������ ���� �ʴ� �ſ� ���� Ÿ���Դϴ�.
    public void TakeWeakHit(float damage, float groggyDamage) 
    {
        OnWeakHit.Invoke(damage, groggyDamage);
    }

    //�ǰ� ����� �ִ� ����� Ÿ���Դϴ�.
    public void TakeNormalHit(float damage, float groggyDamage) 
    {
        OnNormalHit.Invoke(damage, groggyDamage);
    }

    //���� �ǰ� ����� �ִ� ������ Ÿ���Դϴ�.
    public void TakeStrongHit(float damage, float groggyDamage) 
    {
        OnStrongHit.Invoke(damage, groggyDamage);
    }


}
