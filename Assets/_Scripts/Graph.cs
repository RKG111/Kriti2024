using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Graph 
{
    // private static List<Vector2Int> neighbours4directions = new List<Vector2Int>{
    //     new Vector2Int(0,1), //UP
    //     new Vector2Int(1,0), //RIGHT
    //     new Vector2Int(0, -1), // DOWN
    //     new Vector2Int(-1, 0) //LEFT
    // };

    // private static List<Vector2Int> neighbours8directions = new List<Vector2Int>{
    //     new Vector2Int(0,1), //UP
    //     new Vector2Int(1,1), //UP-RIGHT
    //     new Vector2Int(1,0), //RIGHT
    //     new Vector2Int(1,-1), //RIGHT-DOWN
    //     new Vector2Int(0, -1), // DOWN
    //     new Vector2Int(-1, -1), // DOWN-LEFT
    //     new Vector2Int(-1, 0), //LEFT
    //     new Vector2Int(-1, 1) //LEFT-UP
    // };

    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices){
        graph = new List<Vector2Int>(vertices);
    }

    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPosition){
        return GetNeighbours(startPosition, Direction2D.cardinalDirectionsList);
    }

    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPosition){
        return GetNeighbours(startPosition, Direction2D.eightDirectionsList);
    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neighboursOffsetList){
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach(var neighbourDirections in neighboursOffsetList){
            Vector2Int potentialNeighbour = startPosition+neighbourDirections;

            if(graph.Contains(potentialNeighbour)){
                neighbours.Add(potentialNeighbour);
            }
        }
        return neighbours;
    }

    public int GetDistance4Directions(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = GetShortestPathBFS(start, end, GetNeighbours4Directions);
        return path.Count - 1; // Number of edges is the distance
    }

    // Distance function considering edges in 8 directions using BFS
    public int GetDistance8Directions(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = GetShortestPathBFS(start, end, GetNeighbours8Directions);
        return path.Count - 1; // Number of edges is the distance
    }

    public List<Vector2Int> GetPath4Directions(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = GetShortestPathBFS(start, end, GetNeighbours4Directions);
        return path; // Number of edges is the distance
    }

    // Distance function considering edges in 8 directions using BFS
    public List<Vector2Int> GetPath8Directions(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = GetShortestPathBFS(start, end, GetNeighbours8Directions);
        return path; // Number of edges is the distance
    }

    // Helper function to get the shortest path using BFS algorithm
    public List<Vector2Int> GetShortestPathBFS(Vector2Int start, Vector2Int end, Func<Vector2Int, List<Vector2Int>> getNeighbours)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var neighbour in getNeighbours(current))
            {
                if (!visited.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    cameFrom[neighbour] = current;

                    if (neighbour == end)
                    {
                        // Reconstruct the path
                        return ReconstructPath(cameFrom, start, end);
                    }
                }
            }
        }

        // If no path found
        return new List<Vector2Int>();
    }

    // Helper function to reconstruct the path
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

}
