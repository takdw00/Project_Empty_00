using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl_Monster : CharacterControl
{
    public override int GetDirectionIndex(Vector3 dir)
    {
        bool right = lastFacedRight;

        //반대 방향으로 방향을 전환할 때에만 기존 방향을 플립하는 것으로 하는 판정입니다. 예를 들어, 위나 아래로만 이동하면 기존 방향을 유지합니다.
        if (lastFacedRight && lastFacingDirection.x < -0.1f)
        {
            right = false;
        }
        else if (!lastFacedRight && lastFacingDirection.x > 0.1f)
        {
            right = true;
        }

        //2분할 기준의 방향 번호를 얻습니다.
        int dirIndex;
        dirIndex = right ? 1 : 0;

        return dirIndex;
    }
}
