using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Hardware;
// using UnityEditorInternal;
using UnityEngine;

public class NewGolemScript : EnemyAI
{

    [SerializeField] private AudioSource AttackSound;

    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking,
        Cooldown
    }
    public float alpha = -90;
    public float beta = -45;
    public float attackRadius = 0.8f;
    public float chaseSpeed = 20.0f;
    public float patrolSpeed = 7.0f;// Adjust the chase speed as needed
    public int damage = 2;
    private Transform playerTransform;

    [SerializeField]
    public float coolDown = 4f;
    private float currentCoolDown;

    [SerializeField]
    private LayerMask playerLayer;
    private EnemyState currentState;
    public GameObject player;
    private CharacterStats playerStats;

    float distanceToPlayer;

    public float visionRange = 5f;

    private float attackTimer = 0f;
    public float attackInterval = 1.2f;
    public Animator animator;

    [SerializeField]
    float smoothness = 10f;
    private Vector2Int moveTo = new Vector2Int();
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentCoolDown = coolDown;
        currentState = EnemyState.Patrolling;
        moveTo = NextRandomPosition();
        faceDir(moveTo);
        animator = GetComponent<Animator>();
        playerStats = player.GetComponent<CharacterStats>();

    }

    void Update()
    {

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        // Debug.Log(currentState);
        switch (currentState)
        {
            case EnemyState.Patrolling:
                bool stop = false;
                CheckTransitionToChase();
                if (Vector2.Distance(transform.position, moveTo) < 0.01f)
                {
                    stop = true;
                    if (delay < 0)
                    {
                        delay = 2 + Random.Range(0, 2f);
                        moveTo = NextRandomPosition();
                        faceDir(moveTo);
                        stop = false;
                    }
                    delay -= Time.deltaTime;

                }
                if (!stop)
                    Move(moveTo, patrolSpeed);
                // AiPatrol();
                break;

            case EnemyState.Chasing:
                animator.SetBool("Attacking", false);
                if (!HasLineOfSightToPlayer())
                {
                    UpdateCooldown();
                    CheckTransitionToPatrol();
                }
                else
                {
                    currentCoolDown = coolDown;
                }
                // Debug.Log(chasePosition);
                if (Vector2.Distance(transform.position, moveTo) < 0.01f)
                {
                    facePlayer();
                    moveTo = NextClosestPositionBFS();
                    faceDir(moveTo);


                }
                Move(moveTo, chaseSpeed);
                CheckTransitionToAttack();

                break;

            case EnemyState.Attacking:

                if (distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                else
                    AttackPlayer();
                break;

        }
    }

    void CheckTransitionToChase()
    {
        if (HasLineOfSightToPlayer())
        {
            // Debug.Log("Transitioning to chase state.");
            currentState = EnemyState.Chasing;

            moveTo = NextClosestPositionBFS();
            //faceDir(moveTo);

            // Debug.Log("Chase position: " + moveTo);
        }

    }

    void CheckTransitionToAttack()
    {

        if (distanceToPlayer <= attackRadius)
        {
            currentState = EnemyState.Attacking;
            
        }
    }

    void CheckTransitionToPatrol()
    {
        if (currentCoolDown <= 0f)
        {
            currentState = EnemyState.Patrolling;
            currentCoolDown = coolDown;
           // facePlayer();
            moveTo = NextRandomPosition();
            //faceDir(moveTo);
            // Debug.Log("transitioning to patrol");
        }
    }

    void UpdateCooldown()
    {
        currentCoolDown -= Time.deltaTime;
    }

    bool HasLineOfSightToPlayer()
    {


        // Check if the player is within the vision range
        if (distanceToPlayer <= visionRange)
        {
            // Check if there are no obstacles between the enemy and the player
            RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.position, playerLayer);
            return hit.collider != null;
        }

        return false;
    }
    void Move(Vector2 position, float speed)
    {
        Vector2 direction = (position - (Vector2)transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;
        transform.position = Vector2.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    void AttackPlayer()
    {
        animator.SetBool("Attacking", true);
        facePlayer();
        attackTimer += Time.deltaTime;
        if (attackTimer > attackInterval)
        {
            // Debug.Log("attacking");
            //decease player health
            playerStats.TakeDamage(damage);
            //sound ad kar bsdk
            AttackSound.Play();

            attackTimer = 0;
        }
    }

    void facePlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float angle =  alpha + Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void faceDir(Vector2Int destination)                                                    
    {
        Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2 direction = (destination - pos);
        float angle = beta + Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }   

}