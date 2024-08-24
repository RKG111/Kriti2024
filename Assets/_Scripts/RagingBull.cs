using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Hardware;

using UnityEngine;

public class RagingBull : EnemyAI
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking,
        Cooldown
    }

    public float attackRadius = 1.5f;
    public float chaseSpeed = 7.0f; // Adjust the chase speed as needed

    public float patrolSpeed = 4f;
    private Transform playerTransform;

    [SerializeField]
    private float coolDown = 4f;
    private float currentCoolDown;

    [SerializeField]
    private LayerMask playerLayer;
    private EnemyState currentState;

    float distanceToPlayer;

    private float visionRange = 5f;

    [SerializeField]
    float smoothness = 10f;
    private Vector2Int moveTo = new Vector2Int();
    void OnEnable()
    {
        currentCoolDown = coolDown;
        currentState = EnemyState.Patrolling;
        moveTo = NextRandomPosition();
        playerTransform = DungeonState.PlayerInstance.transform;
    }

    void Update()
    {

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
                        
                        stop = false;
                    }
                    delay -= Time.deltaTime;

                }
                if(!stop)
                    Move(moveTo);
                // AiPatrol();
                break;

            case EnemyState.Chasing:
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

                    moveTo = NextClosestPositionBFS();

                }
                Move(moveTo);
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
        if (distanceToPlayer <= 5.0f && HasLineOfSightToPlayer())
        {
            // Debug.Log("Transitioning to chase state.");
            currentState = EnemyState.Chasing;

            moveTo = NextClosestPositionBFS();
            // Debug.Log("Chase position: " + moveTo);
        }

    }

    void CheckTransitionToAttack()
    {

        if (distanceToPlayer <= attackRadius)
        {
            currentState = EnemyState.Attacking;
            attackDelay = 0.5f;
        }
    }

    void CheckTransitionToPatrol()
    {
        if (currentCoolDown <= 0f)
        {
            currentState = EnemyState.Patrolling;
            currentCoolDown = coolDown;
            moveTo = NextRandomPosition();
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
    void Move(Vector2 position)
    {
        Vector2 direction = (position - (Vector2)transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * chaseSpeed * Time.deltaTime;
        transform.position = Vector2.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
    }

    void AttackPlayer()
    {   
        if(attackDelay <0.01f){

            playerTransform.GetComponent<CharacterStats>().TakeDamage(20);
            attackDelay = 1f;

        }
        attackDelay-=Time.deltaTime;
    }
}
