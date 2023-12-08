using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    [Header("Modules")]
    public E_Detection detectionModule;
    public E_Movement movementModule;
    public E_Attack attackModule;
    public E_Health healthModule;
    public E_Animation animationModule;

    [Header("Components")]
    public Animator anim;
    public NavMeshAgent navMesh;
    public Transform attackPoint;
    public Slider healthBarUI;
    public LayerMask playerMask;
    public GameObject[] bloodEffectPrefabs;
    public List<string> playerObjectNames = new List<string>();


    [Header("Common Variables")]
    [Range(0, 200)] public float maxHealth;
    [Range(0, 200)] public float health;
    [Range(0, 10)] public float speed;
    [Range(0, 100)] public float loudness;
    [Range(0, 100)] public float damage;
    [Range(0, 10)] public float sightRange;
    [Range(0, 5)] public float attackRange;
    [Range(0, 1)] public float attackRate;
    [Range(0, 10)] public float detectionRange;
    [Range(0, 1)] public float minStepInterval;
    [Range(0, 5)] public int numberHealth;
    [Range(0, 20)] public int numberMovement;

    //new
    [Range(0, 100)] public int moneyLoot;


    //HIDDEN VARIABLES
    [HideInInspector] public List<Transform> playerTransforms = new List<Transform>();
    [HideInInspector] public Transform originPos;

    [HideInInspector] public float nextAttackTime = 0f;
    [HideInInspector] public float disToPlayer;
    [HideInInspector] public float timeSinceLastStep;
    [HideInInspector] public float lookAtSpeed;

    [HideInInspector] public bool canHit;
    [HideInInspector] public bool canSee;
    [HideInInspector] public bool ready;
    [HideInInspector] public bool IsMove;
    
    private bool gaveCurrency = false;

    private CurrencyHandler currencyHandler;

    private void Awake()
    {
        detectionModule = new E_Detection(this);
        movementModule = new E_Movement(this);
        attackModule = new E_Attack(this);
        healthModule = new E_Health(this);
        animationModule = new E_Animation(this);

        if (originPos == null) originPos = transform.Find("Pos");
        if (playerObjectNames == null) playerObjectNames = new List<string>();
        if (attackPoint == null) attackPoint = transform.Find("AttackPoint");
        if (healthBarUI == null) healthBarUI = transform.Find("HealthBar_Canvas/Slider").GetComponent<Slider>();

        //added
        currencyHandler = FindObjectOfType<CurrencyHandler>();
    }

    private void Update()
    {
        if (!healthModule.IsDead())
        {
            detectionModule.DetectionUpdater();
            movementModule.MovementUpdater();
            healthModule.HealthUpdater();
            attackModule.AttackUpdater();
            animationModule.StateUpdater();
        }
        else
        {
            //added 

            if (currencyHandler != null && !gaveCurrency)
            {
                gaveCurrency = true;
                currencyHandler.AddMoney(moneyLoot);
            }
            //

            AudioManager.instance.PlayAudios("Zombie Death");
            Destroy(gameObject, 0.1f);
        }
    }
    
    public Transform GetClosestPlayer()
    {
        return detectionModule.GetClosestPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        //SIGHT RANGE
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        //HEARING RANGE
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loudness);

        //ATTACK RANGE
        Gizmos.color = Color.red;
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        //DETECTION RANGE
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
