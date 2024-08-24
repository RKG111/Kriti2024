
using UnityEngine;

public class collectibleItem : Item
{
    // Constructor to initialize fields
    public void OnEnable()
    {
        itemName = "Collectible";
        value = 1;
        weight = 1;
        PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
    }

    public override void PickUp(){
        // Debug.Log("pickup");
    }
    public override void Drop(){

    }
        public override void Use()
    {
        // Implement the specific use behavior for collectibleItem
        Debug.Log("collectibleItem used.");
    }
}
