using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.Universal;
using System.Runtime.InteropServices;
public class DungeonGenerator : RoomFirstDungeonGenerator
{
    public int seed;
    public bool useRandomSeed;

    public List<Item> commonItems;

    [Range(1,10)]
    public int commonFrequency;
    public List<Item> rareItems;

     [Range(1,10)]
    public int rareFrequency;
    public List<Item> epicItems;
    // public List<Vector2Int> roomCenters;

     [Range(1,10)]
    public int epicFrequency;
    public List<EnemyAI> minions;

    [Range(1,10)]
    public int minionFrequency;
    public List<EnemyAI> mobs;

     [Range(1,10)]
    public int mobFrequency;
    public List<EnemyAI> giants;

     [Range(1,10)]
    public int giantFrequency;
    private Transform itemHolder;
    private Transform enemyHolder;
    public void SetSeedAndGenerate( bool useRandomSeed = false)
    {
        // Debug.Log(seed.GetHashCode());
        if (useRandomSeed)
            seed = Random.Range(1, 100000);

        Random.InitState(seed);


        base.GenerateDungeon();
        DungeonState.SortRoomsByDistance();

    }

    public void DestroyDungeon(){
        foreach(Transform child in itemHolder){
            Destroy(child);
        }
        foreach(Transform child in enemyHolder){
            Destroy(child);
        }
    }
    public void SetSeedAndGenerate( int seed)
    {
        

        Random.InitState(seed);


        base.GenerateDungeon();
        DungeonState.SortRoomsByDistance();

    }
    public void PlaceItems(){
        itemHolder = new GameObject("itemHolder").transform;
        PlaceItems(commonItems, 1, DungeonState.rooms.Count, commonFrequency);
        PlaceItems(rareItems,Math.Min(3,DungeonState.rooms.Count/2),DungeonState.rooms.Count,rareFrequency, probability:0.75f);
        PlaceItems(epicItems,Math.Min(6,DungeonState.rooms.Count-1),DungeonState.rooms.Count,2,1,0.5f);
    }

    public void PlaceEnemies(){
        enemyHolder = new GameObject("enemyHolder").transform;
        PlaceEnemy(minions, 1, DungeonState.rooms.Count, minionFrequency);
        PlaceEnemy(mobs,Math.Min(3,DungeonState.rooms.Count/2),DungeonState.rooms.Count,mobFrequency, probability:0.75f);
        PlaceEnemy(giants,Math.Min(6,DungeonState.rooms.Count-1),DungeonState.rooms.Count,giantFrequency,probability: 0.5f);
        for(int i=0;i<enemyHolder.childCount;i++){
            Transform child = enemyHolder.GetChild(i);

            DungeonState.AddOpenSpace(Vector2Int.RoundToInt(child.position));

        }
    }
    private void PlaceItems(List<Item> itemList,  int roomStart, int roomEnd , int frequencyMax, int frequencyMin = 0, float probability = 1f)
    {
        if(itemList.Count==0){
            return;
        }
        for (int i = roomStart; i < roomEnd; i++)
        {


            ItemPlacementHelper helper = new ItemPlacementHelper(DungeonState.rooms[i]);
            int frequency = Random.Range(0,frequencyMax);
            foreach (Item item in itemList)
            {
                for (int j = 0; j < frequency; j++)
                {
                    Vector2Int? position = helper.GetItemPlacementPosition(item.type, 5, item.size);
                    if (position != null && ShouldSpawn(probability)){

                        DungeonState.rooms[i].RemoveTile((Vector2Int)position);
                        DungeonState.openSpaces.Remove((Vector2Int)position);

                        Instantiate(item, (Vector3Int)position + Vector3Int.forward, Quaternion.identity,itemHolder);
                    }
                    else if(position !=null){
                        DungeonState.AddOpenSpace((Vector2Int)position);
                    }
                }
            }

        }
    }


    private void PlaceEnemy(List<EnemyAI> enemyList,  int roomStart, int roomEnd , int frequencyMax, int frequencyMin = 0, float probability = 1f)
    {

        if(enemyList.Count==0){
            return;
        }
        for (int i = roomStart; i < roomEnd; i++)
        {


            ItemPlacementHelper helper = new ItemPlacementHelper(DungeonState.rooms[i]);
            int frequency = Random.Range(0,frequencyMax);
            foreach (EnemyAI enemy in enemyList)
            {
                for (int j = 0; j < frequency; j++)
                {
                    Vector2Int? position = helper.GetItemPlacementPosition(enemy.type, 5, enemy.size);
                    
                    if (position != null && ShouldSpawn(probability)){

                        DungeonState.rooms[i].RemoveTile((Vector2Int)position);
                        DungeonState.openSpaces.Remove((Vector2Int)position);

                        Instantiate(enemy, (Vector3Int)position + Vector3Int.back, Quaternion.identity,enemyHolder);
                    }
                    else if(position !=null){
                        DungeonState.AddOpenSpace((Vector2Int)position);
                    }
                }
                
            }

        }
    }
    private bool ShouldSpawn(float probability){
        return Random.value < probability;
    }
    private void PlaceRandomItems(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> corridorPositions)
    {
        HashSet<Vector2Int> floorWithCorridors = new HashSet<Vector2Int>(floorPositions);
        floorPositions.ExceptWith(corridorPositions);
        HashSet<Vector2Int> floorNoCorridors = new HashSet<Vector2Int>(floorPositions);
        ItemPlacementHelper helper = new ItemPlacementHelper(floorWithCorridors, floorNoCorridors);

        // for (int i = 0; i < frequency; i++)
        // {
        //     Vector2Int? position = helper.GetItemPlacementPosition(PlacementType.OpenSpace, 5, new Vector2Int(1, 1));
        //     if (position != null)
        //         floorPositions.Remove((Vector2Int)position);
        //     Instantiate(items, (Vector3Int)position + Vector3Int.back, Quaternion.identity);
        // }
    }
    
}
