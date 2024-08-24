using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon(int seed)
    {   
        UnityEngine.Random.InitState(seed);
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }
    public void GenerateDungeon()
    {   
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }
    protected abstract void RunProceduralGeneration();
}
