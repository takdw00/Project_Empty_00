using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//캐릭터의 그래픽 오브젝트에 붙어 애니메이션 이벤트 발생을 총괄합니다.
public class CharacterAnimEvents : MonoBehaviour
{
    public UnityEvent Event_OnActionEnding;
    public UnityEvent Event_OnAttackEnd;
    public UnityEvent Event_OnHitEnd;

    //Ending의 의미는 동작이 끝나기 직전이라는 의미입니다. 주로 선입력을 받는 타이밍을 주는 데 사용됩니다.
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
