using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stones : Item
{
    // Start is called before the first frame update
    
    public override void Drop()
    {
        base.DropItem();
    }

    public override void PickUp()
    {
        base.PickUpItem();

    }

    public override void Use()
    {
        
    }
}
