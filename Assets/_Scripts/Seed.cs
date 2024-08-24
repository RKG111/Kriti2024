using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public string gameString = "Default";
    
    public AbstractDungeonGenerator generator;
    public int currentSeed;

    private void Awake() {
        currentSeed = gameString.GetHashCode();
        UnityEngine.Random.InitState(currentSeed);
        generator.GenerateDungeon();
    }   
}
