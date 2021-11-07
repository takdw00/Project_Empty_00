using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//ĳ������ �׷��� ������Ʈ�� �پ� �ִϸ��̼� �̺�Ʈ �߻��� �Ѱ��մϴ�.
public class CharacterAnimEvents : MonoBehaviour
{
    public UnityEvent Event_OnActionEnding;
    public UnityEvent Event_OnAttackEnd;
    public UnityEvent Event_OnHitEnd;

    //Ending�� �ǹ̴� ������ ������ �����̶�� �ǹ��Դϴ�. �ַ� ���Է��� �޴� Ÿ�̹��� �ִ� �� ���˴ϴ�.
    public void OnActionEnding() 
    {
        Event_OnActionEnding.Invoke();
    }
    
    public void OnAttackEnd() 
    {
        Event_OnAttackEnd.Invoke();
    }

    public void OnHitEnd() 
    {
        Event_OnHitEnd.Invoke();
    }
}
