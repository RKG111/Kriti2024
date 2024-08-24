using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    [SerializeField] private KeyCode collectKey = KeyCode.Space; // Set your desired key
    private float decreasingSpeedUnit = 1; // Adjust the speed decrease value as needed
   // private collectibleItem item; // Reference to the collectibleItem component
    private Item item;
    private bool canCollect; // Flag to track whether collection is allowed

    GameObject player;

    private void Start()
    {
        // Get the collectibleItem component from the current GameObject
       // item = GetComponent<collectibleItem>();
        item = GetComponent<Item>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Check for collect key input outside of OnTriggerStay2D
        if (canCollect && Input.GetKeyDown(collectKey))
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
        // Collectible logic
        if (player.TryGetComponent(out PlayerController playerController))
        {
            // if(item.canAdd()){
                decreasingSpeedUnit = item.weight;
                playerController.movementSpeed -= decreasingSpeedUnit;
                playerController.movementSpeed = Mathf.Max(playerController.movementSpeed, 1);
                //Debug.Log("Pickup function called from collectiblescript");
                // if(item.canAddtoInventory())
                // {
                //     item.PickUp(); // Call the item's pickup();
                //     //item.Use();
                //     //Destroy(gameObject);
                //     // Disable collider
                //     Collider2D collider = GetComponent<Collider2D>();
                //     if (collider != null)
                //     {
                //         collider.enabled = false;
                //     }
                    
                //     // Disable sprite renderer
                //     SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                //     if (spriteRenderer != null)
                //     {
                //         spriteRenderer.enabled = false;
                //     }
                // }
                // else{
                //     Debug.Log("Inventory is full");
                // }

            // }
            // else{
            //     Debug.Log("Inventory Full");
            // }

        }
        else
        {
            Debug.LogWarning("PlayerController component not found on player object.");
        }
    }
}
