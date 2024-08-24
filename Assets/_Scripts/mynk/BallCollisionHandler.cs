using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    public LayerMask collidingLayer; // Specify the grid layer in the Inspector.
    public float overlapRadius = 0.1f; // Adjust the radius based on your bullet size.
    public int fireballDamage = 10; // Set the damage amount for the fireball.

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, collidingLayer);

        // Check if any of the colliders belong to the grid layer.
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Grid"))
            {
                // Destroy the bullet upon collision with the grid.
                Destroy(gameObject);
                return; // Exit the loop after destroying the bullet.
            }

            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player Collided");
                // Destroy the bullet upon collision with the player.
                                // Disable collider
                Collider2D fireballcollider = GetComponent<Collider2D>();
                if (fireballcollider != null)
                {
                    fireballcollider.enabled = false;
                }
                
                // Disable sprite renderer
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }

                // Access the PlayerStats component attached to the player object.
                CharacterStats playerStats = collider.GetComponent<CharacterStats>();
                
                // Check if the PlayerStats component exists.
                if (playerStats != null)
                {
                    // Call the TakeDamage function with the fireball damage amount.
                    playerStats.TakeDamage(fireballDamage);
        
                }
                else{
                    Debug.Log("playerstats not found");
                }

                return; // Exit the loop after dealing damage to the player.
            }
        }
    }
}
