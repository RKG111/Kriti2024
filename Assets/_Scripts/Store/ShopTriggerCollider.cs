using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{
   [SerializeField] private UI_Shop uishop;
   private void onTriggerEnter2D(Collider2D collider){
        IShopCustomer shopCustomer = collider.GetComponent<IShopCustomer>();
        if(shopCustomer != null){
            uishop.Show(shopCustomer);
            UnityEngine.Debug.Log("Entered!!!!");
        }
    }

    private void onTriggerExit2D(Collider2D collider){
        IShopCustomer shopCustomer = collider.GetComponent<IShopCustomer>();
        if(shopCustomer != null){
            uishop.Hide();
            UnityEngine.Debug.Log("Exited!!!!");
        }
    }
}
