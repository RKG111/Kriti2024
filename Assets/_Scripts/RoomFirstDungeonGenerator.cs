using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    [SerializeField]
    private GameObject items;
    public int frequency = 5;
    
    public Dictionary<Vector2Int,HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> floorPositions =new HashSet<Vector2Int>();
    private HashSet<Vector2Int>  corridorPositions;

    public List<Vector2Int> roomCenters;
    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        // PlaceRandomItems();
        // DungeonState.openSpaces = new HashSet<Vector2Int>(floorPositions);
        DungeonState.dungeonMap = new Graph(DungeonState.openSpaces);
    }

    // private void PlaceRandomItems(){
    //     HashSet<Vector2Int> floorWithCorridors = new HashSet<Vector2Int>(floorPositions);
    //     floorPositions.ExceptWith(corridorPositions);
    //     HashSet<Vector2Int> floorNoCorridors = new HashSet<Vector2Int>(floorPositions);
    //     ItemPlacementHelper helper = new ItemPlacementHelper(floorWithCorridors,floorNoCorridors);

    //     for(int i=0;i<frequency;i++){
    //         Vector2Int? position = helper.GetItemPlacementPosition(PlacementType.OpenSpace, 5, new Vector2Int(1,1));
    //         if(position!= null)
    //             floorPositions.Remove((Vector2Int)position);
    //             Instantiate(items, (Vector3Int)position+Vector3Int.back, Quaternion.identity);
    //     }
    // }
    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        

        List<Vector2Int> room_Centers = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            room_Centers.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        roomCenters = new List<Vector2Int>(room_Centers);
        HashSet<Vector2Int> corridors = ConnectRooms(room_Centers);
        floor.UnionWith(corridors);
        // DebugWithObjects(floor);
        tilemapVisualizer.PaintFloorTiles(floor);
        DungeonState.openSpaces = new HashSet<Vector2Int>(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }


    // void DebugWithObjects(HashSet<Vector2Int> list){
    //     GameObject tempParent = new GameObject("Parent");
    //     foreach(Vector2Int item in list){
    //         GameObject temp = new GameObject();
    //         temp.transform.position = (Vector3Int)item;
    //         temp.transform.parent = tempParent.transform;
    //         temp.AddComponent<Light2D>();
    //     }
    //     tempParent.transform.position+=new Vector3(0.5f,0.5f,0);
    // }
    // private List<Vector2Int> CreateRooms()
    // {
    //     var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

    //     HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

    //     if (randomWalkRooms)
    //     {
    //         floor = CreateRoomsRandomly(roomsList);
    //     }
    //     else
    //     {
    //         floor = CreateSimpleRooms(roomsList);
    //     }
        

    //     List<Vector2Int> room_Centers = new List<Vector2Int>();
    //     foreach (var room in roomsList)
    //     {
    //         room_Centers.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
    //     }
    //     // Debug.Log(room_Centers.Count);
    //     HashSet<Vector2Int> corridors = ConnectRooms(room_Centers);
    //     floor.UnionWith(corridors);

    //     tilemapVisualizer.PaintFloorTiles(floor);
    //     WallGenerator.CreateWalls(floor, tilemapVisualizer);
    //     return room_Centers;
    // }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            HashSet<Vector2Int> validFloor = new HashSet<Vector2Int>();
            
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                    validFloor.Add(position);

                }
            }

            SaveRoomData(roomCenter,validFloor);
            Room room  = new Room(roomCenter,validFloor);
            DungeonState.AddRoom(room);
        }
        return floor;
    }
    private void ClearRoomData(){
        roomsDictionary.Clear();
    }
    private void SaveRoomData(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor){
        roomsDictionary[roomCenter] = roomFloor;
        floorPositions.UnionWith(roomFloor);
    }
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        corridorPositions = new HashSet<Vector2Int>(corridors);

        DungeonState.corridors = new HashSet<Vector2Int>(corridors);
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
