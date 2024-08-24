using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{ [SerializeField]
    public GameObject ballPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float fireRate = 2f;
    public float aimSpeed = 2f;
    public float timeToDestroy=3f;
    public int x,y,z;
    private Transform player;
   
    private float nextFireTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Aim at the player
        AimAtPlayer();

        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            // Fire bullets
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void AimAtPlayer()
    {
        // Calculate the direction to the player
        Vector2 direction = -(player.position - transform.position).normalized;

        // Calculate the angle between the current forward direction and the player direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards the player
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle+90));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        // Create a bullet
         GameObject bullet = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);
    
    // Rotate the bullet to match the direction of the thrower
    bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
    
        // Disable collider
                Collider2D collider = bullet.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
                
                // Disable sprite renderer
                SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                }

        // Add force to the bullet to make it move
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
         rb.velocity = transform.up* bulletSpeed;
        //  // Attach a Collider2D component to the bullet if not already attached
        // Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        // if (bulletCollider == null)
        // {
        //     bulletCollider = bullet.AddComponent<BoxCollider2D>();
        // }

        // // Attach a script to handle bullet-grid collisions
        // bullet.AddComponent<BulletCollisionHandler>();

        Destroy(bullet, timeToDestroy);

    }
}
