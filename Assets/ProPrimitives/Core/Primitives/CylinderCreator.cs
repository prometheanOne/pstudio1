/*
  Created by Juan Sebastian Munoz Arango 2013
  naruse@gmail.com
  all rights reserved.
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class CylinderCreator : ProPrimitive {

    #if UNITY_EDITOR
    private float height = 1;
    private float radius = 0.5f;
    private int subDivHeight = 1;
    private int subDivCircumference = 15;
    private PrimitiveSpace cylinderSpace = PrimitiveSpace.XZ;

    public CylinderCreator() { }

    public override void DrawInterface() {
        cylinderSpace = (PrimitiveSpace) EditorGUILayout.EnumPopup("Cylinder base space: ", cylinderSpace);

        radius = EditorGUILayout.FloatField("Radius: ", radius > 0.001f ? radius : 0.001f);
        height = EditorGUILayout.FloatField("Height: ", height > 0.001f ? height : 0.001f);

        subDivCircumference = EditorGUILayout.IntField("Circumference Divisions:", subDivCircumference>=3 ? subDivCircumference:3);
        subDivHeight = EditorGUILayout.IntField("Height Divisions:", subDivHeight >= 0 ? subDivHeight : 0);
    }
    public override Mesh CreatePrimitive() {
        return BuildCylinder(radius, height, subDivCircumference, subDivHeight, cylinderSpace); 
    }
    #endif

    public static Mesh BuildCylinder(float _radius, float _height, int _subDivCircumference, int _subDivHeight, PrimitiveSpace _primitiveSpace = PrimitiveSpace.XZ) {
        Mesh generatedCylinder = new Mesh();
        generatedCylinder.Clear();

        float displacementHeight = _height/2;

        Mesh cover = BuildCylinderCover(_radius, _height, _subDivCircumference, _subDivHeight, _primitiveSpace);
        Mesh topFace = BuildCircle(displacementHeight, _radius, _subDivCircumference, _primitiveSpace, false/*not inverted face*/);
        Mesh bottomFace = BuildCircle(-displacementHeight, _radius, _subDivCircumference, _primitiveSpace, true/*inverted face*/);

        return Utils.SewMeshes(new Mesh[] { cover, topFace, bottomFace });
    }
    


    private static Mesh BuildCylinderCover(float _radius, float _height, int _subDivCircumference, int _subDivHeight, PrimitiveSpace _primitiveSpace = PrimitiveSpace.XZ) {
        Mesh generatedMesh = new Mesh();
        generatedMesh.Clear();
        int numberOfVertices = 4 + (_subDivCircumference * 2) + (_subDivHeight * 2) + (_subDivCircumference * _subDivHeight);
        Vector3[] vertices = new Vector3[numberOfVertices];
        Vector3[] normals = new Vector3[numberOfVertices];
        Vector2[] UVs = new Vector2[numberOfVertices];
        List <int> triangles = new List<int>();
        
        float angle = 360/(float)_subDivCircumference;

        float uvFactorWidth = 1/(_subDivCircumference+2);
        float uvFactorHeight = 1/(_subDivHeight+2);
        float heightScale = _height/(_subDivHeight+1);  
        
        Vector3 currentDrawnPoint = Vector3.zero;
        float cirPosX, cirPosY, hPos;//circumference position, height position
        
        for(int j = 0; j < _subDivHeight+2; j++) {
            for(int i = 0; i < _subDivCircumference+2; i++) {
                cirPosX = _radius * Mathf.Cos(Mathf.Deg2Rad * (angle * (float)(i)));
                cirPosY = _radius * Mathf.Sin(Mathf.Deg2Rad * (angle * (float)(i)));
                hPos = j*heightScale - (_height/2);
                
                switch(_primitiveSpace) {
                case PrimitiveSpace.YZ:
                    currentDrawnPoint = new Vector3(hPos, cirPosX, cirPosY);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivCircumference+2)] = (new Vector3(0, cirPosX, cirPosY)).normalized;
                    break;
                case PrimitiveSpace.XY:
                    currentDrawnPoint = new Vector3(cirPosX, cirPosY, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivCircumference+2)] = (new Vector3(cirPosX, cirPosY, 0)).normalized;
                    break;
                case PrimitiveSpace.XZ:
                    currentDrawnPoint = new Vector3(cirPosX, hPos, cirPosY);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivCircumference+2)] = (new Vector3(cirPosX, 0, cirPosY)).normalized;
                    break;   
                }
                
                vertices[Utils.ConvertToArrIndex(i,j,_subDivCircumference+2)] = currentDrawnPoint;
                UVs[Utils.ConvertToArrIndex(i,j,_subDivCircumference+2)] = new Vector2(i*uvFactorWidth, j*uvFactorHeight);

                if(i < _subDivCircumference + 1 && j < _subDivHeight + 1) {
                    if(_primitiveSpace == PrimitiveSpace.XZ) {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivCircumference+2));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivCircumference+2));
                    } else {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivCircumference+2));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivCircumference+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivCircumference+2)); 
                    }
                }
            }
        }
        generatedMesh.vertices = vertices;
        generatedMesh.normals = normals;
        generatedMesh.uv = UVs;
        generatedMesh.triangles = triangles.ToArray();
        return generatedMesh;
    }


    private static Mesh BuildCircle(float _displacementHeight, float _radius, int _subDivCircumference, PrimitiveSpace _primitiveSpace = PrimitiveSpace.XZ, bool _invertedFace = false) {
        Mesh circleMesh = new Mesh();
        circleMesh.Clear();
        int numberOfVertices = _subDivCircumference;
        Vector3[] vertices = new Vector3[numberOfVertices];
        Vector3[] normals = new Vector3[numberOfVertices];
        Vector2[] UVs = new Vector2[numberOfVertices];
        List<int> triangles = new List<int>();
        
        float angle = 360/(float)_subDivCircumference;
        float xCoord = -1;
        float yCoord = -1;

        Vector3 currentDrawnPoint = Vector3.zero;

        for(int i = 0; i < numberOfVertices; i++) {
            xCoord = _radius * Mathf.Cos(Mathf.Deg2Rad * (angle * (float)(i)));
            yCoord = _radius * Mathf.Sin(Mathf.Deg2Rad * (angle * (float)(i)));
            
            switch(_primitiveSpace) {
                case PrimitiveSpace.XY:
                    currentDrawnPoint = new Vector3(xCoord, yCoord, _displacementHeight);
                    normals[i] = _invertedFace ? -Vector3.forward : Vector3.forward;
                    break;
                case PrimitiveSpace.XZ:
                    currentDrawnPoint = new Vector3(xCoord, _displacementHeight, yCoord);
                    normals[i] = _invertedFace ? -Vector3.up : Vector3.up;
                    break;
                case PrimitiveSpace.YZ:
                    currentDrawnPoint = new Vector3(_displacementHeight, xCoord, yCoord);
                    normals[i] = _invertedFace ? -Vector3.right : Vector3.right;
                    break;   
            }

            vertices[i] = currentDrawnPoint;
            UVs[i] = new Vector2(0,0);//TODO!!!!     

            if(i > 0 && i < numberOfVertices-1) {
                if((_primitiveSpace == PrimitiveSpace.XZ && !_invertedFace) ||
                   (_primitiveSpace == PrimitiveSpace.YZ && _invertedFace) ||
                   (_primitiveSpace == PrimitiveSpace.XY && _invertedFace)) {
                    triangles.Add(0);
                    triangles.Add((i+1) % numberOfVertices);
                    triangles.Add(i);
                } else {
                    triangles.Add(0);
                    triangles.Add(i);
                    triangles.Add((i+1) % numberOfVertices);
                }
            }
        }
        
        circleMesh.vertices = vertices;
        circleMesh.normals = normals;
        circleMesh.uv = UVs;
        circleMesh.triangles = triangles.ToArray();
        return circleMesh;
    }
}
