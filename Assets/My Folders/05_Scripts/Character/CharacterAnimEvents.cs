using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//캐릭터의 그래픽 오브젝트에 붙어 애니메이션 이벤트 발생을 총괄합니다.
public class CharacterAnimEvents : MonoBehaviour
{
    public UnityEvent Event_OnActionEnding;
    public UnityEvent Event_OnAttackEnd;

    public void OnAttackEnd() 
    {
        Event_OnAttackEnd.Invoke();
    }
}
