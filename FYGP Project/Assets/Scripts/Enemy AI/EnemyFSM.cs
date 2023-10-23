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

    [Header("Components")]
    public LayerMask playerMask;
    public string playerObjectName = "Player(Clone)";
    public NavMeshAgent navMesh;
    public Transform attackPoint;
    public Slider healthBarUI;

    [Header("Common Variables")]
    [Range(0, 200)] public float health;
    [Range(0, 100)] public float loudness;
    [Range(0, 100)] public float damage;
    [Range(0, 10)] public float sightRange;
    [Range(0, 5)] public float attackRange;
    [Range(0, 1)] public float attackRate;
    [Range(0, 10)] public float detectionRange;
    [Range(0, 1)] public float minStepInterval;
    [Range(0, 5)] public int numberHealth;
    [Range(0, 20)] public int numberMovement;

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

    private void Awake()
    {
        detectionModule = new E_Detection(this);
        movementModule = new E_Movement(this);
        attackModule = new E_Attack(this);
        healthModule = new E_Health(this);

        if (originPos == null) originPos = transform.Find("Pos");
        if (attackPoint == null) attackPoint = transform.Find("AttackPoint");
        if (healthBarUI == null) healthBarUI = transform.Find("HealthBar_Canvas/Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (!healthModule.IsDead())
        {
            detectionModule.DetectionUpdater();
            movementModule.MovementUpdater();
            healthModule.HealthUpdater();
            attackModule.AttackUpdater();
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
