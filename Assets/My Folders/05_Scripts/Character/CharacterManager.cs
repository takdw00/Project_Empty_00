using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BTSystem;

public class CharacterManager : MonoBehaviour
{
    /// <summary>
    /// 캐릭터에서 소속 된 오브젝트의 모든 스크립트에 연결한다.
    /// 다른 외부 스크립트에서의 접근을 위한 스크립트
    /// </summary>

    

    //in Character Object
    CapsuleCollider characterCollider;
    NavMeshAgent characterNavAgent;
    CharacterControl characterControl;
    CharacterStatus characterStatus;

    //in Graphics Object
    SpriteRenderer spriteRenderer;
    Animator animator;
    CharacterAnimEvents characterAnimEvents;

    //in BT Object 
    BehaviorTree behaviorTree;


    #region Properties
    //in Character Object
    private CapsuleCollider CharacterCollider { get { return characterCollider; } set { characterCollider = value; } }
    public NavMeshAgent CharacterNavAgent { get { return characterNavAgent; } private set { characterNavAgent = value; } }
    private CharacterControl CharacterControl { get { return characterControl; } set { characterControl = value; } }
    public CharacterStatus CharacterStatus { get { return characterStatus; } set { characterStatus = value; } }

    //in Graphics Object
    private SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }
    private Animator Animator { get { return animator; } set { animator = value; } }
    private CharacterAnimEvents CharacterAnimEvents { get { return characterAnimEvents; } set { characterAnimEvents = value; } }

    //in BT Object 
    public BehaviorTree BehaviorTree { get { return behaviorTree; } private set { behaviorTree = value; } }
    #endregion

    private void Awake()
    {
        //in Character Object
        try
        {
            characterCollider = GetComponent<CapsuleCollider>();
            characterNavAgent = GetComponent<NavMeshAgent>();
            characterControl = GetComponent<CharacterControl>();
            characterStatus = GetComponent<CharacterStatus>();
        }
        catch
        {
            Debug.Log(" Failed to get script inside " + gameObject.name + " object.");
        }

        //in Graphics Object
        try
        {
            spriteRenderer = transform.Find("Graphics").GetComponent<SpriteRenderer>();
            animator = transform.Find("Graphics").GetComponent<Animator>();
            characterAnimEvents = transform.Find("Graphics").GetComponent<CharacterAnimEvents>();
        }
        catch
        {
            Debug.Log(" Failed to get script inside " + transform.Find("Graphics").gameObject.name + " object.");
        }

        //in BT Object 
        try
        {
            behaviorTree = transform.Find("BT").GetComponent<BehaviorTree>();
        }
        catch
        {
            Debug.Log(" Failed to get script inside " + transform.Find("BT").gameObject.name + " object.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
