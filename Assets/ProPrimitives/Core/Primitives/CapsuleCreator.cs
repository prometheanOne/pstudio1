/*
  Created by Juan Sebastian Munoz Arango 2013
  narse@gmail.com
  all rights reserved
 */

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class CapsuleCreator : ProPrimitive {
    #if UNITY_EDITOR
    private float radius = 0.5f;
    private float height = 1;
    private int subDivCircumference = 10;
    private int subDivHeight = 5;
    private PrimitiveSpace capsuleSpace = PrimitiveSpace.XZ;

    public CapsuleCreator() { }

    public override void DrawInterface() {
        capsuleSpace = (PrimitiveSpace) EditorGUILayout.EnumPopup("Capsule Space: ", capsuleSpace);
    
        radius = EditorGUILayout.FloatField("Radius: ", radius > 0.001f ? radius : 0.001f);
        height = EditorGUILayout.FloatField("Height: ", height > 0.001f ? height : 0.001f);

        subDivCircumference = EditorGUILayout.IntField("Circumference Divisions:", subDivCircumference>=3 ? subDivCircumference:3);
        subDivHeight = EditorGUILayout.IntField("Height Divisions:", subDivHeight >= 0 ? subDivHeight : 0);
    }
    public override Mesh CreatePrimitive() {
        return BuildCapsule(radius, height, subDivCircumference, subDivHeight, capsuleSpace);
    }
    #endif

    public static Mesh BuildCapsule(float _radius, float _height, int _subDivCircumference, int _subDivHeight, PrimitiveSpace _primitiveSpace = PrimitiveSpace.XZ) {
        float hemispheresDisplacement = _height/2;
        Mesh capsuleMesh = new Mesh();
        Mesh cylinder = BuildCylinderCover(_radius, _height, _subDivCircumference, _subDivHeight, _primitiveSpace);
        Mesh topHemisphere = BuildHemisphere(_radius, _subDivCircumference, hemispheresDisplacement, false /*not inverted*/, _primitiveSpace);
        Mesh bottomHemisphere = BuildHemisphere(_radius, _subDivCircumference, hemispheresDisplacement, true /*not inverted*/, _primitiveSpace);

        capsuleMesh = Utils.SewMeshes(new Mesh[] { topHemisphere, cylinder, bottomHemisphere });
        return capsuleMesh;
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

    private static Mesh BuildHemisphere(float _radius, int _subDivisions, float _hemisphereDisplacement, bool _inverted, PrimitiveSpace _hemisphereSpace = PrimitiveSpace.XZ) {
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

                switch(_hemisphereSpace) {
                case PrimitiveSpace.XZ:
                    currentDrawnPoint = new Vector3(xPos,(_inverted?-1:1) * (zPos+_hemisphereDisplacement), yPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = (new Vector3(xPos,(_inverted?-1:1)*zPos,yPos)).normalized;
                    break;
                case PrimitiveSpace.XY:
                    currentDrawnPoint = new Vector3(xPos,  yPos, (_inverted?-1:1) * (zPos+_hemisphereDisplacement));
                    normals[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = (new Vector3(xPos,yPos,(_inverted?-1:1)*zPos)).normalized;
                    break;
                case PrimitiveSpace.YZ:
                    currentDrawnPoint = new Vector3((_inverted?-1:1) * (zPos+_hemisphereDisplacement),xPos,yPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = (new Vector3((_inverted?-1:1)*zPos,xPos,yPos)).normalized;
                    break;
                }
                
                vertices[Utils.ConvertToArrIndex(i,j,_subDivisions+1)] = currentDrawnPoint;
                UVs[Utils.ConvertToArrIndex(i,j,_subDivisions+2)] = new Vector2(i*uvFactorWidth, j*uvFactorHeight);

                if(i < _subDivisions && j < _subDivisions) {
                    if((_hemisphereSpace == PrimitiveSpace.XZ && _inverted) || 
                       (_hemisphereSpace != PrimitiveSpace.XZ && !_inverted)) {

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
