using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimEvents : MonoBehaviour
{
    private CharacterControl characterControl;

    private void Awake()
    {
        characterControl = transform.parent.GetComponent<CharacterControl>();
    }

    public void OnAttackEnd() 
    {
        characterControl.OnAttackEnd();
    }
}
