using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Vector3 origin = Vector3.zero;
        float fov = 90f;
        int rayCount = 2;
        float angle = 0f;
        float angleIncrease = fov/rayCount;

        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount+1+1];
        Vector2[] uv = new Vector2[vertices.Length];

        int[] triangles = new int[rayCount*3];


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        vertices[0]=origin;
        // for(int i=0;i<=rayCount;i++){
        //     Vector3 vertex = origin+UtilsClass
        // }
    }

    
}
