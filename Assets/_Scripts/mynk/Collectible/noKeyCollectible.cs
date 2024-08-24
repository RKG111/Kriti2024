using UnityEngine;

public class noKeyCollectible : MonoBehaviour
{
    private bool canCollect; // Flag to track whether collection is allowed
    [SerializeField]
    private int coinValue = 10;
    [SerializeField]
    private AudioClip collectSound; // Sound to play when collected
    private AudioSource audioSource; // Reference to the AudioSource component
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        // Set the collect sound clip for the AudioSource
        if (collectSound != null)
        {
            audioSource.clip = collectSound;
        }
    }

    private void Update()
    {
        // Check for collect key input outside of OnTriggerStay2D
        if (canCollect)
        {
            Collect(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = true; // Set flag when player enters trigger
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = false; // Reset flag when player exits trigger
        }
    }

    private void Collect(GameObject player)
    {
        // Play the collect sound if available
        if (collectSound != null)
        {
            audioSource.Play();
        }
        // Add coins to the player's stats
        player.GetComponent<CharacterStats>().AddCoin(coinValue);
        // Disable collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        // Disable sprite renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
