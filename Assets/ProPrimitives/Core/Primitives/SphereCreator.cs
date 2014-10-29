/*
  Created by Juan Sebastian Munoz Arango 2013
  naruse@gmail.com
  all rights reserved
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class SphereCreator : ProPrimitive {

    #if UNITY_EDITOR
    private float radius = 0.5f;
    private int subDivisions = 10;

    public override void DrawInterface() {
        radius = EditorGUILayout.FloatField("Radius:", radius > 0.001f ? radius : 0.001f);
        subDivisions = EditorGUILayout.IntField("Sphere Divisions:", subDivisions >= 3 ? subDivisions : 3);
    }
    public override Mesh CreatePrimitive() {
        return BuildSphere(radius, subDivisions);
    }
    #endif
    public SphereCreator() { }

    public static Mesh BuildSphere(float _radius, int _subDivisions) {
        Mesh sphere = new Mesh();
        sphere.Clear();

        Mesh halfSphere = BuildHemisphere(_radius, _subDivisions, false /*inverted*/);
        Mesh otherHalfOfSphere = BuildHemisphere(_radius, _subDivisions, true /*inverted*/);
        
        sphere = Utils.SewMeshes(new Mesh[] { halfSphere, otherHalfOfSphere } );
        
        return sphere;
    }

    private static Mesh BuildHemisphere(float _radius, int _subDivisions, bool _inverted) {
        Mesh sphere = new Mesh();
        sphere.Clear();

        int numberOfVertices = (_subDivisions * _subDivisions) + (_subDivisions * 4) + 4;
        Vector3[] vertices = new Vector3[numberOfVertices];
        Vector3[] normals = new Vector3[numberOfVertices];
        Vector2[] UVs = new Vector2[numberOfVertices];
        List <int> triangles = new List<int>();

        float angle1 = 360/(float)_subDivisions;
        float angle2 = 90/(float)_subDivisions;
        
        float xPos;
        float yPos;
        float zPos;
        
        float uvFactorWidth = 1/(_subDivisions+2);
        float uvFactorHeight = 1/(_subDivisions+2);
        
        Vector3 currentDrawnPoint = Vector3.zero;

        for(int j = 0; j < _subDivisions+1; j++) {
            for(int i = 0; i < _subDivisions+1; i++) {
                xPos = _radius * Mathf.Cos(Mathf.Deg2Rad * angle1 * i) * Mathf.Sin(Mathf.Deg2Rad * angle2 * j);
                yPos = _radius * Mathf.Sin(Mathf.Deg2Rad * angle1 * i) * Mathf.Sin(Mathf.Deg2Rad * angle2 * j);
                zPos = _radius * Mathf.Cos(Mathf.Deg2Rad * angle2 * j);
                
                currentDrawnPoint = new Vector3(xPos, (_inverted?-1:1) * zPos, yPos);
                normals[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = (new Vector3(xPos,(_inverted?-1:1)*zPos,yPos)).normalized;
                vertices[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = currentDrawnPoint;
                UVs[Utils.ConvertToArrIndex(i,j,_subDivisions+2)] = new Vector2(i*uvFactorWidth, j*uvFactorHeight);

                if(i < _subDivisions && j < _subDivisions) {
                    if(_inverted) {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivisions+1));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivisions+1));                            
                    } else {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivisions+1));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivisions+1));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivisions+1)); 
                    }
                }
            }
        }

        sphere.vertices = vertices;
        sphere.normals = normals;
        sphere.uv = UVs;
        sphere.triangles = triangles.ToArray();
        return sphere;
    }
}
