using UnityEngine;

// 적 및 위협요소의 타격의 수용을 담당하는 클래스입니다.
// 타격의 강도를 결정하는 곳입니다.
// 지정한 타격을 받는 처리를 각 스크립트로 보냅니다.(characterControl에 히트 모션 명령, characterStatus에 데미지 수용 명령)
public class HitController : MonoBehaviour
{
    public UnityEventFloatFloat OnWeakHit;
    public UnityEventFloatFloat OnNormalHit;
    public UnityEventFloatFloat OnStrongHit;

    //모션에 영향을 주지 않는 매우 약한 타격입니다.
    public void TakeWeakHit(float damage, float groggyDamage) 
    {
        OnWeakHit.Invoke(damage, groggyDamage);
    }

    //피격 모션을 주는 통상의 타격입니다.
    public void TakeNormalHit(float damage, float groggyDamage) 
    {
        OnNormalHit.Invoke(damage, groggyDamage);
    }

    //강한 피격 모션을 주는 강력한 타격입니다.
    public void TakeStrongHit(float damage, float groggyDamage) 
    {
        OnStrongHit.Invoke(damage, groggyDamage);
    }


}
