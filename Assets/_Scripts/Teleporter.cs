using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Item
{
    public override void PickUp()
    {
        
    }
    public override void Drop()
    {
        
    }
    public override void Use()
    {
        Transform playerTransform = DungeonState.PlayerInstance.transform;

        playerTransform.position = DungeonState.GetSpawn();
    }
}
