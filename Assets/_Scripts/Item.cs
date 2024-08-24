using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public float value;

    public GameObject itemObject;
    public float weight;

    public Sprite sprite;
    public Vector2Int size = new Vector2Int();

    public PlacementType type ;
    private float textRange = 3f;

    [SerializeField]

    public TMP_Text nameText;
    [SerializeField]
    //private GameObject textObject;
    LayerMask playerLayer;

    public Inventory PlayerInventory;
    private void OnEnable() {
        
        // PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerLayer = LayerMask.GetMask("Player");
        PlayerInventory = DungeonState.PlayerInstance.GetComponent<Inventory>();
        
    }

    public abstract void PickUp();

    public abstract void Drop();
    public abstract void Use();

    // public bool canAddtoInventory()
    // {
    //     return PlayerInventory.canAdd();
    // }

    public  void PickUpItem(){
       // Debug.Log("Add function called from Item abstract class");
        if(PlayerInventory.Add(this)){

        itemObject.SetActive(false);
        DungeonState.AddOpenSpace(Vector2Int.RoundToInt(transform.position));
        }
    }

    public void DropItem(){
        PlayerInventory.Remove(this);
        itemObject.SetActive(true);
        Transform player = DungeonState.PlayerInstance.transform;
        transform.position = Vector3Int.RoundToInt(player.position+player.forward);
        DungeonState.RemoveSpace(Vector2Int.RoundToInt(transform.position));
        // itemObject.transform= 
    }

    
}



