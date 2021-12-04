using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoverSpotsGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfSpotsPerUnit = 5;
    
    Mesh mesh;
    Vector3[] vertices;
    public List<Vector3> posAroundCube = new List<Vector3>();
    public Vector3[] worldVerts = new Vector3[4];
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        List<Vector3> checkedVerts = new List<Vector3>();
        float sameY = vertices[0].y;

        int vertFoundIndex = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            bool keepGoing = true;
            
            foreach (var vert in checkedVerts)
            {
                if (vertices[i] == vert)
                {
                    keepGoing = false;
                }
            }
            
            if (keepGoing && Math.Abs(vertices[i].y - sameY) < 0.01f)
            {
                Vector3 worldSpaceVert = vertices[i];
                worldSpaceVert.x *= transform.localScale.x;
                worldSpaceVert.z *= transform.localScale.z;

                worldSpaceVert += transform.position;
                
                worldVerts[vertFoundIndex] = worldSpaceVert;
                vertFoundIndex++;
                checkedVerts.Add(vertices[i]);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Vector3 localVert = worldVerts[i] - transform.position;
            Vector3 newVector = Quaternion.Euler(0, transform.eulerAngles.y, 0) * localVert;
            newVector += transform.position;
            worldVerts[i] = newVector;
        }
        
        //switch 3rd and 4th place so it follows the side of the cube
        Vector3 tempVector = worldVerts[2];
        Vector3 tempVector2 = worldVerts[3];
        worldVerts[2] = tempVector2;
        worldVerts[3] = tempVector;

        for (int i = 0; i < 4; i++)
        {
            int secondVert = (i + 1) % 4;
            int numberOfPoints = (int)(Vector3.Distance(worldVerts[i], worldVerts[secondVert]) * numberOfSpotsPerUnit);
            Vector3 dir = (worldVerts[secondVert] - worldVerts[i]) / numberOfPoints;
            Vector3 currentPos = worldVerts[i];
            //Debug.Log(HandleUtility.DistancePointLine(FindObjectOfType<Ally>().transform.position, worldVerts[i], worldVerts[secondVert]));

            int numberOfOperations = 0;
            while (numberOfOperations < numberOfPoints)
            {
                posAroundCube.Add(currentPos);
                currentPos = currentPos + dir;
                numberOfOperations++;
            }
        }
    }
}
