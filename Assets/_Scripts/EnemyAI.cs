using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public float health;

    public float delay = 2.0f;

    public float attackDelay = 2f;

    public PlacementType type;

    public Vector2Int size = new Vector2Int();
    GameObject Player = DungeonState.PlayerInstance;


    void takeDamage(float damage)
    {
        health -= damage;
    }


    public void AiPatrol() //!!deprecated
    {
        delay -= Time.deltaTime;
        if (delay < 0.01f)
        {
            delay = 2 + Random.Range(0, 2f);
            Vector2Int currentPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);


            int index = Random.Range(0, 4);
            Vector2Int destination = currentPos + Direction2D.cardinalDirectionsList[index];
            if (DungeonState.isEmptySpace(destination))
            {
                Move(destination);
                // DungeonState.swapPos(destination, currentPos);
            }
        }


    }



    public Vector2Int NextClosestPosition()
    {
        int minDistance = int.MaxValue;
        Vector2Int currentPos = Vector2Int.RoundToInt(transform.position);
        Vector2Int playerPos = Vector2Int.RoundToInt(Player.transform.position);

        Vector2Int closestPosition = currentPos; // Initialize to current position if no valid positions are found

        List<Vector2Int> validPos = new List<Vector2Int>();
        foreach (var direction in Direction2D.cardinalDirectionsList)
        {
            if (DungeonState.isEmptySpace(currentPos + direction))
            {
                validPos.Add(currentPos + direction);
            }
        }

        foreach (var pos in validPos)
        {
            int distance = ManhattanDistance(pos, playerPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = pos;
            }
        }
        // Debug.Log(currentPos.ToString()+" "+closestPosition.ToString());
        // Return the closest position found, or the current position if no valid positions are found
        // DungeonState.swapPos(closestPosition,currentPos);
        return closestPosition;
    }

    public Vector2Int NextClosestPositionBFS(){
        Vector2Int currentPos = Vector2Int.RoundToInt(transform.position);
        Vector2Int playerPos = Vector2Int.RoundToInt(Player.transform.position);
        Vector2Int closestPos = currentPos;
        List<Vector2Int>path = DungeonState.dungeonMap.GetPath4Directions(currentPos,playerPos);
        if(path.Count>1){

            closestPos=path[1];
        }
        return closestPos;
    }
    public Vector2Int NextRandomPosition()
    {
        Vector2Int currentPos = Vector2Int.RoundToInt(transform.position);

        // Debug.Log(currentPos);
        List<Vector2Int> validPos = new List<Vector2Int>();
        Vector2Int position = currentPos;
        foreach (var direction in Direction2D.cardinalDirectionsList)
        {
            if (DungeonState.isEmptySpace(currentPos + direction))
            {
                validPos.Add(currentPos + direction);

            }
        }

        if (validPos.Count > 0)
        {
            position = validPos[Random.Range(0, validPos.Count)];
            // Debug.Log(position);
        }
        // DungeonState.swapPos(position,currentPos);
        
        return position;
        

    }
    int ManhattanDistance(Vector2Int pointA, Vector2Int pointB)
    {
        return Mathf.Abs(pointA.x - pointB.x) + Mathf.Abs(pointA.y - pointB.y);
    }
    void Move(Vector2Int destination)
    {
        transform.position = new Vector3(destination.x, destination.y, 0);
    }


    private void FixedUpdate()
    {

    }
}
