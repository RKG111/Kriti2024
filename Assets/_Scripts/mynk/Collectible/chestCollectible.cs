using UnityEngine;

public class chestCollectible : MonoBehaviour
{
    private bool canCollect; // Flag to track whether collection is allowed
    [SerializeField]
    private int coinValue = 100;
    [SerializeField]
    private AudioClip collectSound; // Sound to play when collected
    private AudioSource audioSource; // Reference to the AudioSource component
    private GameObject player;
    private bool collected = false; // Flag to track whether coins have been collected
      private Animator animator; // Reference to the Animator component

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
           animator = GetComponent<Animator>(); // Get the Animator component
       
        // Set the collect sound clip for the AudioSource
        if (collectSound != null)
        {
            audioSource.clip = collectSound;
        }
    }

    private void Update()
    {
        // Check for collect key input outside of OnTriggerStay2D
        if (canCollect && !collected && Input.GetKeyDown(KeyCode.Space))
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
        // Check if the player has the "key" item
        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory != null )
        {
            // Play the collect sound if available
            if (collectSound != null)
            {
                audioSource.Play();
            }
            // Add coins to the player's stats
         
            int itemCount=inventory.items.Count;
              Debug.Log("item count "+itemCount);
              
             for (int i = 0; i < itemCount; i++)
        {
            // Check if the current item is a chestKey
            if (inventory.items[i] is chestKey)
            {
                inventory.Remove(inventory.items[i]);
                player.GetComponent<CharacterStats>().AddCoin(coinValue);
        if (animator != null)
        {
            animator.SetTrigger("Open"); // Assuming you have a trigger parameter named "Open"
        }
                Debug.Log("ChestKey removed from inventory.");
                break; // Exit the loop after removing the first chestKey item
            }
        }
    
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            // Set collected flag to true
            collected = true;
        }
    }
}
