using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICharacterControl_Dodgeable 
{
    public void Dodge(Vector3 dir);
}

public interface ICharacterControl_Guardable
{
    public void Guard(Vector3 dir);
}

public interface ICharacterControl_Parriable 
{
    //public void Parry();
}