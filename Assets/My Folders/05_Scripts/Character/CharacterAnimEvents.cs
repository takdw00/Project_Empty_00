using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//ĳ������ �׷��� ������Ʈ�� �پ� �ִϸ��̼� �̺�Ʈ �߻��� �Ѱ��մϴ�.
public class CharacterAnimEvents : MonoBehaviour
{
    public UnityEvent Event_OnActionEnding;
    public UnityEvent Event_OnAttackEnd;

    public void OnAttackEnd() 
    {
        Event_OnAttackEnd.Invoke();
    }
}
