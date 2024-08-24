using System.Collections;
using System.Collections.Generic;
// using UnityEditorInternal;
using UnityEngine;

public class Script : EnemyAI
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking,
        Cooldown
    }

    //private GameObject player;
    private Rigidbody2D rb;
    //public float force;


    public GameObject stone;
    public Transform stonePos;

    private float timer;
    public float attackRadius = 1.5f;
    public float chaseSpeed = 7.0f; // Adjust the chase speed as needed
    public Transform playerTransform;

    [SerializeField]
    private float coolDown = 4f;
    private float currentCoolDown;

    [SerializeField]
    private LayerMask playerLayer;
    private EnemyState currentState;

    float distanceToPlayer;

    private float visionRange = 5f;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentCoolDown = coolDown;
        currentState = EnemyState.Patrolling;
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        switch (currentState)
        {
            case EnemyState.Patrolling:
                AiPatrol();
                CheckTransitionToChase();
                break;

            case EnemyState.Chasing:
                //ChasePlayer();
                CheckTransitionToAttack();
                if (!HasLineOfSightToPlayer())
                {
                    UpdateCooldown();
                    CheckTransitionToPatrol();
                }
                else
                {
                    currentCoolDown = coolDown;
                }
                break;

            case EnemyState.Attacking:

                if (distanceToPlayer > attackRadius)
                {
                    currentState = EnemyState.Chasing;
                }
                else
                    AttackPlayer();
                break;

            case EnemyState.Cooldown:
                UpdateCooldown();
                CheckTransitionToPatrol();
                break;
        }
    }

    void CheckTransitionToChase()
    {

        if (distanceToPlayer <= 5.0f && HasLineOfSightToPlayer())
        {
            currentState = EnemyState.Chasing;
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
        }
    }

    void UpdateCooldown()
    {
        currentCoolDown -= Time.deltaTime;
        Debug.Log("cooldown");
    }

    bool HasLineOfSightToPlayer()
    {


        // Check if the player is within the vision range
        if (distanceToPlayer <= visionRange)
        {
            // Check if there are no obstacles between the enemy and the player
            /*  RaycastHit2D hit = Physics2D.Linecast(transform.position, playerTransform.position, playerLayer);
              return hit.collider != null;*/

            return true;
        }

        return false;
    }
    /* void ChasePlayer()
     {
         // Move towards the player
         Vector2 direction = (playerTransform.position - transform.position).normalized;
         Vector2 movement = direction * chaseSpeed * Time.deltaTime;
         transform.Translate(movement);
       //  float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;


     }*/

    void AttackPlayer()
    {

        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            stoneThrow();
        }
        Debug.Log("Under attack");
        // Implement your attack logic here
    }


    void stoneThrow()
    {
        Instantiate(stone, stonePos.position, Quaternion.identity);
    }
}
