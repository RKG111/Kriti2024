using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(10);

    public UIManager uIManager;
    
    public int currentIndex;

    private Transform currentSlot;

    private Vector3 highlightScale = new Vector3(1f,1f,1f);
    private Vector3 normalScale = new Vector3(0.8f,0.8f,0.8f);

    public LayerMask itemLayer ;

    // public Sprite emptySprite;

    [SerializeField]
    private float pickupRange=1.5f;
    private void OnEnable() {
        uIManager = GameObject.FindObjectOfType<UIManager>();
        if(uIManager==null){
            Debug.Log("UIManger Missing");
        }
        currentSlot = uIManager.GetInventorySlotTransform(currentIndex);
        currentSlot.localScale = highlightScale;
        itemLayer = LayerMask.GetMask("Item");
    }
    private void Update() {
        
        currentIndex+=(int)Input.mouseScrollDelta.y;
        currentIndex = Mathf.Clamp(currentIndex, 0, 9);
        currentSlot.localScale = normalScale;
        currentSlot = uIManager.GetInventorySlotTransform(currentIndex);
        currentSlot.localScale = highlightScale;
        uIManager.currentValue.text = "Collected\t:"+ GetTotalCost().ToString();
        if(Input.GetKeyDown(KeyCode.E)){
            GameObject item = GetNearByItem();
            if(item!=null){
                // Debug.Log(item.name);
                item.GetComponent<Item>().PickUp();
            }
        }
        if(Input.GetKeyDown(KeyCode.G)){
            Item item = items[currentIndex];
            if(item!=null){
                item.Drop(); 
            }
        }
    }

    public float GetTotalWeight(){
        float weight = 0f;
        foreach(var item in items){
            if(item!=null)
                weight+=item.weight;
        }
        return weight;
    }

    public float GetTotalCost(){
        float cost = 0f;
        // if(items.)
        foreach(var item in items){
            if(item!=null)
                cost+=item.value;
        }
        return cost;
    }
    // public bool canAdd()
    // {
    //     Debug.Log("canAdd function called" + items.Count);
    //     // return ((items.Count)<maxNo);
    // }

    public bool Add(Item item){
        // Debug.Log(item.name);
        int index = GetEmpySlot();
        // Debug.Log(index);
        if(index>=0){

            items[index]=item;
            uIManager.SetInventorySlot(index,item.sprite);
            return true;
        }
        return false;
    }

    public void emptyInventory()
    {
     foreach(var item in items){
            Remove(item);
        }
    }

    public void Remove(Item item){
        items.Remove(item);
        uIManager.EmptyInventorySlot(currentIndex);
    }

    GameObject GetNearByItem(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange, itemLayer);
        if(colliders.Length>0){
            return colliders[0].transform.parent.gameObject;
        }
        Debug.Log("No item nearby");
        return null;

    }
    
    int GetEmpySlot(){
        for(int i=0;i<10;i++){
            if(items[i]==null){

                return i;
            }
        }
        return -1;
    }
}


