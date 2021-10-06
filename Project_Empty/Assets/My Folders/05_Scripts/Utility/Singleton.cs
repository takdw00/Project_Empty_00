using UnityEngine;

//�̱������� ����ϰ��� �ϴ� Ŭ���� T�� Singleton<T>�� ��ӹ������μ� �̱������� ����� �� ����.
//T.Instance�� ������ �̱��� ��ü�� ���� ����
//��ӹ��� Ŭ������ Awake, OnDestroy�Լ��� �������̵� �� base.Awake(), base.OnDestory()�� ȣ���ϵ��� �־� �� ��
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
