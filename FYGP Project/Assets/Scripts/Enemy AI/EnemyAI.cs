using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI States")]
    [Tooltip("Check if the AI is within a given range for the AI to cahse the player"),
    SerializeField] private Movement movementState;
    [HideInInspector] public int numberMovement;

    [Tooltip("Checks if the AI meets the requirements to be destroyed"),
    Space(5), SerializeField] private DestroyedState destroyState;
    [HideInInspector] public int numberDestroy;

    [Tooltip("Check if the UI Health Bar should be hidden depending on the state"),
    Space(5), SerializeField] private HealthBar healthBarState;
    [HideInInspector] public int numberHealth;

    [Tooltip("Checks if the AI is ready to attack the player depending on the range from the player"),
    Space(5), SerializeField] private AttackingState attackingState;
    [HideInInspector] public int numberAttacks;

    /// <summary>
    /// ENUM STATES
    /// </summary>

    public enum Movement
    {
        Walk, Run
    };

    public enum HealthBar
    {
        Hide, Hidden
    };

    public enum DestroyedState
    {
        Enable, Disable
    };

    public enum AttackingState
    {
        Enable, Disable
    };

    /// <summary>
    /// KEY COMPONENTS
    /// </summary>

    private List<Transform> playerTransforms;
    private Transform originPos;
    private NavMeshAgent navMesh;
    private Slider healthBarUI;

    /// <summary>
    /// AI VARIABLES
    /// </summary>

    [Header("AI Stats"),
    Tooltip("The player's layermask"),
    SerializeField] private LayerMask playerMask;

    [Tooltip("The name of the player's gameobject"),
    SerializeField] private string playerObjectName;

    [Tooltip("The origin point of the zombie's attacking point"),
    SerializeField] private Transform attackPoint;

    [Space(25), Range(0, 200), Tooltip("The enemy's health value")]
    public float health;

    [Range(0, 100), Tooltip("The requirement to trigger the enemy to switch movement states")]
    public float loudness;

    [Range(0, 100), Tooltip("The amount of damage given to a player")]
    public float damage;

    [Range(0, 10), Tooltip("How far the AI can see the player in order to chase them")]
    public float sightRange;

    [Range(0, 5), Tooltip("The range in which the enemy attacks the player")]
    public float attackRange;

    [Range(0, 1), Tooltip("How fast the zombie is attacking the player")]
    public float attackRate;

    [Range(0, 10), Tooltip("If the player is in the given range, the zombie will chase the player")]
    public float detectionRange;

    [Range(0, 1), Tooltip("The minimum time for the next step")]
    public float minStepInterval;

    [Range(0f, 5f), Tooltip("The minimum interval for playing groan noise"),
    SerializeField] private float minGroanInterval;

    [Range(0f, 20f), Tooltip("The maximum interval for playing groan noise"),
    SerializeField] private float maxGroanInterval;

    /// <summary>
    /// AUDIO
    /// </summary>

    [Header("Audio")]
    [SerializeField] private string[] footstepAudioNames;
    [SerializeField] private string[] attackAudioNames;
    [SerializeField] private string[] groanAudioNames;

    [SerializeField] private string[] deathAudioNames;
    [SerializeField] private string[] hitAudioNames;

    /// <summary>
    /// BOOLEAN CHECK
    /// </summary>

    private float nextAttackTime = 0f;
    private float lookAtSpeed = 3f;
    private float timeSinceLastStep;
    private float distanceToPlayer;
    private float nextGroanTime;
    private float readyTime;
    private float randomDestroy;

    [HideInInspector] public bool IsMove;
    [HideInInspector] public bool Spawn;
    [HideInInspector] public bool canHit;
    [HideInInspector] public bool canSee;

    private bool isDead = false;
    private bool ready;

    private void Start()
    {
        StartingsStates();
        VariableSetup();
        StartCoroutine(UpdatePlayerList());
    }
    private void Update()
    {
        if (!isDead)
        {
            FetchPlayers();
            ReadyBoolSwitch();
            HearingRange();
            AttackDistanceChecker();
            AIHealth();
            VisualisePlayer();
            GetClosestPlayer();
            AngleSights();
            WithinRange();
            GroanAudio();
            CheckAttack();

            //ENUM STATES
            MovementFunction();
        }
        
        DeathFunction();
    }

    private void FetchPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms.Clear();

        foreach (GameObject player in players)
        {
            playerTransforms.Add(player.transform);
        }
    }


    private void VariableSetup()
    {
        //healthBarUI = transform.Find("HealthBar_Canvas/Slider").GetComponent<Slider>();
        originPos = transform.Find("Pos").GetComponent<Transform>();
        navMesh = GetComponent<NavMeshAgent>();
        playerTransforms = new List<Transform>();
        for (int i = 1; i <= 4; i++)
        {
            var player = GameObject.Find("Player(Clone)");
            if (player != null) playerTransforms.Add(player.transform);
        }

        nextGroanTime = Time.time + Random.Range(minGroanInterval, maxGroanInterval);
        randomDestroy = Random.Range(5f, 8f);
        canHit = false;
    }

    private void StartingsStates()
    {
        if (Spawn == false)
        {
            switch (attackingState)
            {
                case AttackingState.Enable:
                    {
                        numberAttacks = 0;
                        break;
                    }

                case AttackingState.Disable:
                    {
                        numberAttacks = 1;
                        break;
                    }
            }

            switch (movementState)
            {
                case Movement.Walk:
                    {
                        numberMovement = 0;
                        break;
                    }

                case Movement.Run:
                    {
                        numberMovement = 1;
                        break;
                    }
            }

            switch (healthBarState)
            {
                case HealthBar.Hide:
                    {
                        numberHealth = 1;
                        break;
                    }

                case HealthBar.Hidden:
                    {
                        numberHealth = 1;
                        break;
                    }
            }

            switch (destroyState)
            {
                case DestroyedState.Enable:
                    {
                        numberDestroy = 0;
                        break;
                    }

                case DestroyedState.Disable:
                    {
                        numberDestroy = 1;
                        break;
                    }
            }
        }

        if (numberAttacks == 0)
        {
            canHit = true;
        }

        if (numberAttacks == 1)
        {
            canHit = false;
        }
    }

    private void MovementFunction()
    {
        if (numberMovement == 0)
        {
            if (IsMove && !IsPlayerInAttackRange())
            {
                Transform closestPlayer = GetClosestPlayer();
                if (closestPlayer == null)
                {
                    return;
                }
                navMesh.speed = 1.5f;
                navMesh.destination = closestPlayer.position;

                if (timeSinceLastStep >= minStepInterval)
                {
                    // AUDIO MANAGER
                    timeSinceLastStep = 0f;
                }

                timeSinceLastStep += Time.deltaTime;
            }
            else
            {
                navMesh.speed = 0;
            }
        }
    }



    private void DeathFunction()
    {
        if (health <= 0f)
        {
            //DEATH AUDIO
            DuringDeath();
            isDead = true;
        }
    }

    private void HealthUIFunction()
    {
        if (numberHealth == 0 && health > 0)
        {
            healthBarUI.gameObject.SetActive(true);
        }

        if (numberHealth == 1 && health > 0)
        {
            healthBarUI.gameObject.SetActive(false);
        }
    }

    private IEnumerator UpdatePlayerList()
    {
        while (true)
        {
            FetchPlayers();
            yield return new WaitForSeconds(5f);
        }
    }

    private Transform GetClosestPlayer()
    {
        float closestDistance = float.MaxValue;
        Transform closestPlayer = null;

        foreach (var playerTransform in playerTransforms)
        {
            float currentDistance = Vector3.Distance(playerTransform.position, transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestPlayer = playerTransform;
            }
        }

        return closestPlayer;
    }

    private void AngleSights()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null)
        {
            return;
        }

        Vector3 targetDir = closestPlayer.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < 45f && distanceToPlayer < 10f && canSee == true)
        {
            canHit = true;
        }
    }

    private void LookAtTarget()
    {
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null || health <= 0f)
        {
            return;
        }

        var rotation = Quaternion.LookRotation(closestPlayer.position - transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookAtSpeed);
    }

    private IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(randomDestroy);
        Destroy(this.gameObject);
    }

    private void WithinRange()
    {
        Collider[] playerCheckRange = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider player in playerCheckRange)
        {
            if (player.gameObject.name == playerObjectName && numberMovement != 0)
            {
                numberMovement = 0;
                IsMove = true;
            }
        }
    }

    private void DuringDeath()
    {
        int RandomDeath = Random.Range(1, 5);
        this.GetComponent<NavMeshAgent>().enabled = false;
        //healthBarUI.gameObject.SetActive(false);

        if (numberDestroy == 0)
        {
            StartCoroutine(TimeToDestroy());
        }
    }

    private void AttackDistanceChecker()
    {
        if (distanceToPlayer < 2 && canHit == true)
        {
            LookAtTarget();
        }
    }

    private bool IsPlayerInAttackRange()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerMask);
        return hitPlayers.Length > 0;
    }

    private void CheckAttack()
    {
        if (IsPlayerInAttackRange())
        {
            if (Time.time >= nextAttackTime)
            {
                IsMove = false;
                lookAtSpeed = 2f;
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else
        {
            IsMove = true;
        }
    }

    public void EventAttack()
    {
        RaycastHit hit;
        float range = 1.3f;
        Debug.DrawRay(originPos.position, originPos.transform.TransformDirection(Vector3.forward)
            * range, Color.red);

        if (Physics.Raycast(originPos.position, originPos.transform.TransformDirection(Vector3.forward), 
            out hit, range))
        {
            if (hit.transform.gameObject.name == playerObjectName)
            {
                hit.transform.gameObject.SendMessage("ApplyDamage", damage);
            }
        }
    }

    public void AIHealth()
    {
        //healthBarUI.value = health / 100;

        if (health > 0)
        {
            CheckAttack();
            
        }
    }

    private void HearingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, loudness);
        foreach(Collider hitCol in hitColliders)
        {
            if(hitCol.gameObject.name == playerObjectName)
            {
                ready = true;
            }
        }
    }

    private void VisualisePlayer()
    {
        RaycastHit hit;
        Transform closestPlayer = GetClosestPlayer();
        if (closestPlayer == null)
        {
            return;
        }

        Vector3 fromPos = originPos.transform.position;
        Vector3 toPos = new Vector3(closestPlayer.position.x, closestPlayer.position.y + 1, closestPlayer.position.z);
        Vector3 dir = toPos - fromPos;

        Debug.DrawRay(originPos.position, dir, Color.cyan);

        if (Physics.Raycast(originPos.position, dir, out hit, sightRange))
        {
            if (hit.transform.gameObject.name == playerObjectName)
            {
                canSee = true;
            }
            else
            {
                canSee = false;
            }
        }
    }


    private void GroanAudio()
    {
        if (Time.time > nextGroanTime)
        {
            //GROAN AUDIO
            nextGroanTime = Time.time + Random.Range(minGroanInterval, maxGroanInterval);
        }
    }

    private void ReadyBoolSwitch()
    {
        if (readyTime > 0f)
        {
            readyTime -= Time.deltaTime;
        }
        if (readyTime <= 0f)
        {
            ready = false;
            readyTime = 1f;
        }
    }

    public void EnemyDamage(float damage)
    {
        //AUDIO BEING HIT
        health -= damage;
        IsMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, loudness);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
