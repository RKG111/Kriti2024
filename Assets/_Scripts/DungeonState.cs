using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
public static class DungeonState 
{
    public static HashSet<Vector2Int> openSpaces;

    public static GameObject PlayerInstance;

    public static List<Room> rooms = new List<Room>();

    public static HashSet<Vector2Int> corridors ;

    public static Graph dungeonMap;

    public static int DungeonSeed;
    public static bool isEmptySpace(Vector2Int pos){
        return openSpaces.Contains(pos);
    }

    public static void AddOpenSpace(Vector2Int pos){
        openSpaces.Add(pos);
    }
    public static void RemoveSpace(Vector2Int pos){
        openSpaces.Remove(pos);
    }
    public static void swapPos(Vector2Int toRemove, Vector2Int toAdd){
        openSpaces.Remove(toRemove);
        openSpaces.Add(toAdd);

    }

    public static Room GetRoom(Vector2Int center){
        foreach(Room room in rooms){
            if(room.roomCenter == center){
                return room;
            }
        }
        return null;
    }

    public static void AddRoom(Room room){
        rooms.Add(room);
    }

    public static void SortRoomsByDistance()
    {
        if (rooms.Count <= 1)
        {
            // No need to sort if there is only one room or no rooms.
            return;
        }

        // Sort rooms by distance from the first room (rooms[0])
        rooms = rooms.OrderBy(room => DungeonState.dungeonMap.GetDistance8Directions(rooms[0].roomCenter, room.roomCenter)).ToList();
    }

    public static Vector3Int GetSpawn(){

        ItemPlacementHelper helper = new ItemPlacementHelper(rooms[0]);
        Vector2Int? position = helper.GetItemPlacementPosition(PlacementType.OpenSpace,10,new Vector2Int(1,1));
        openSpaces.Add((Vector2Int)position);
        return (Vector3Int)position;
    }

    public static void DebugWithObjects(IEnumerable<Vector2Int> list, GameObject parent){
        
        foreach(Vector2Int item in list){
            GameObject temp = new GameObject();
            temp.transform.position = (Vector3Int)item;
            temp.transform.parent = parent.transform;
            temp.AddComponent<Light2D>();
        }
        parent.transform.position+=new Vector3(0.5f,0.5f,0);
    }

    public static void DebugPositions(){
        GameObject tempParent = new GameObject("Parent");
        foreach(Room room in rooms){
            DebugWithObjects(room.emptyRoomFloor,tempParent);
        }
    }

    
}



public class Room{

    public readonly Vector2Int roomCenter;

    public readonly HashSet<Vector2Int> roomFloor;

    public HashSet<Vector2Int> emptyRoomFloor;

    public Room(Vector2Int center, HashSet<Vector2Int> floor){
        roomCenter = new Vector2Int(center.x,center.y);
        roomFloor = new HashSet<Vector2Int>(floor);


        emptyRoomFloor = new HashSet<Vector2Int>(floor);

        HashSet<Vector2Int> cardinalSet = WallGenerator.FindWallsInDirections(roomFloor,Direction2D.cardinalDirectionsList);
        HashSet<Vector2Int> diagonalSet = WallGenerator.FindWallsInDirections(roomFloor,Direction2D.diagonalDirectionsList);
        emptyRoomFloor.ExceptWith(cardinalSet);
        emptyRoomFloor.ExceptWith(diagonalSet);
    }

    public void RemoveTile(Vector2Int position){
        emptyRoomFloor.Remove(position);
    }

}
