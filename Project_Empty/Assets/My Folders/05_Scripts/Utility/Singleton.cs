using UnityEngine;

//싱글톤으로 사용하고자 하는 클래스 T는 Singleton<T>를 상속받음으로서 싱글톤으로 기능할 수 있음.
//T.Instance로 빠르게 싱글톤 객체에 접근 가능
//상속받은 클래스의 Awake, OnDestroy함수는 오버라이딩 후 base.Awake(), base.OnDestory()를 호출하도록 넣어 줄 것
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate a second instance of singleton class.");
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
