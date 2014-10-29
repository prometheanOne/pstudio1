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

public class CubeCreator : ProPrimitive {

    private enum FaceSide {
        XZUp = 0,
        XZDown = 1,
        XYForward = 2,
        XYBackward = 3,
        YZRight = 4,
        YZLeft = 5
    }
    #if UNITY_EDITOR   
    private float width = 1;
    private float height = 1;
    private float depth = 1;

    private int subDivWidth = 1;
    private int subDivHeight = 1;
    private int subDivDepth = 1;
    
    public CubeCreator() { }

    public override void DrawInterface() {
        width = EditorGUILayout.FloatField("Width (x): ", width > 0.001f? width : 0.001f);
        height = EditorGUILayout.FloatField("Height (y): ", height > 0.001f? height : 0.001f);
        depth = EditorGUILayout.FloatField("Depth (z): ", depth > 0.001f? depth : 0.001f);

        subDivWidth = EditorGUILayout.IntField("Width Divisions (x): ", subDivWidth >= 0? subDivWidth : 0);
        subDivHeight = EditorGUILayout.IntField("Height Divisions (y): ", subDivHeight >= 0? subDivHeight : 0);
        subDivDepth = EditorGUILayout.IntField("Depth Divisions (z): ", subDivDepth >= 0? subDivDepth : 0);
    }
    public override Mesh CreatePrimitive() {
        return BuildCube(width, height, depth, subDivWidth, subDivHeight, subDivDepth);
    }
    #endif

    public static Mesh BuildCube(float _width, float _height, float _depth, int _subDivWidth = 1, int _subDivHeight = 1, int _subDivDepth = 1) {
        Mesh generatedMesh = new Mesh();
        generatedMesh.Clear();
        //displacement on each of the faces for the cube.
        float displacementWidth = _width/2;
        float displacementHeight = _height/2;
        float displacementDepth = _depth/2;

        Mesh top = CreateFace(displacementHeight, _width, _depth, _subDivWidth, _subDivDepth, FaceSide.XZUp);
        Mesh bottom = CreateFace(-displacementHeight, _width, _depth, _subDivWidth, _subDivDepth, FaceSide.XZDown);
        Mesh front = CreateFace(displacementDepth, _width, _height, _subDivWidth, _subDivHeight, FaceSide.XYForward);
        Mesh back = CreateFace(-displacementDepth, _width, _height, _subDivWidth, _subDivHeight, FaceSide.XYBackward);
        Mesh left = CreateFace(displacementWidth, _height, _depth, _subDivHeight, _subDivDepth, FaceSide.YZLeft);
        Mesh right = CreateFace(-displacementWidth, _height, _depth, _subDivHeight, _subDivDepth, FaceSide.YZRight);
        generatedMesh = Utils.SewMeshes(new Mesh[] { top, bottom, front, back, left, right });
        return generatedMesh;
    }

 

    private static Mesh CreateFace(float _displacement, float _width, float _height, int _subDivWidth, int _subDivHeight, FaceSide side) {
        Mesh generatedMesh = new Mesh();
        generatedMesh.Clear();
        int numberOfVertices = 4 + (_subDivWidth * 2) + (_subDivHeight * 2) + (_subDivWidth * _subDivHeight);
        Vector3[] vertices = new Vector3[numberOfVertices];
        Vector3[] normals = new Vector3[numberOfVertices];
        Vector2[] UVs = new Vector2[numberOfVertices];
        List <int> triangles = new List<int>();
        
        float uvFactorWidth = 1/(_subDivWidth+2);
        float uvFactorHeight = 1/(_subDivHeight+2);
        float widthScale = _width/(_subDivWidth+1);
        float heightScale = _height/(_subDivHeight+1);  
        
        Vector3 currentDrawnPoint = Vector3.zero;
        float wPos, hPos;//width position, height position
        
        for(int j = 0; j < _subDivHeight+2; j++) {
            for(int i = 0; i < _subDivWidth+2; i++) {
                wPos = i*widthScale - (_width/2);
                hPos = j*heightScale - (_height/2);
                
                switch(side) {
                case FaceSide.XZUp:
                    currentDrawnPoint = new Vector3(wPos, _displacement, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.up;
                    break;
                case FaceSide.XZDown:
                    currentDrawnPoint = new Vector3(wPos, _displacement, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = -Vector3.up;
                    break;
                case FaceSide.XYForward:
                    currentDrawnPoint = new Vector3(wPos, hPos, _displacement);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.forward;
                    break;
                case FaceSide.XYBackward:
                    currentDrawnPoint = new Vector3(wPos, hPos, _displacement);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = -Vector3.forward;
                    break;
                case FaceSide.YZRight:
                    currentDrawnPoint = new Vector3(_displacement, wPos, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = -Vector3.right;
                    break;
                case FaceSide.YZLeft:
                    currentDrawnPoint = new Vector3(_displacement, wPos, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.right;
                    break;
                }
                
                vertices[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = currentDrawnPoint;
                UVs[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = new Vector2(i*uvFactorWidth, j*uvFactorHeight);

                if(i < _subDivWidth + 1 && j < _subDivHeight + 1) {
                    if(side == FaceSide.XZUp || side == FaceSide.XYBackward || side == FaceSide.YZRight) {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivWidth+2));
                    } else {
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));

                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivWidth+2));
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
    
}
