using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
//using V_AnimationSystem;

public class StoreItems : MonoBehaviour
{            
    public enum ItemType{
        Torch,
        Gun,
        Bandage,
        Shoes,
        Oil,
        IncRadius
    }

    public static int GetCost(ItemType itemType){
        switch(itemType){
            default:
            case ItemType.Torch:        return 10;
            case ItemType.Gun:          return 110;
            case ItemType.Bandage:      return 20;
            case ItemType.Shoes:        return 200;
            case ItemType.Oil:          return 75;
            case ItemType.IncRadius:    return 50;
        }
    }


}
// banadge
// Torch
// guns
//  shoes
//  Oil