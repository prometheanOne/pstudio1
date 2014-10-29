/*
  Created by Juan Sebastian Munoz Arango 2012
  naruse@gmail.com
  all rights reserved
 */
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class PlaneCreator : ProPrimitive {
    #if UNITY_EDITOR
    private float width = 1;
    private float height = 1;
    private int subDivWidth = 1;
    private int subDivHeight = 1;
    private PrimitiveSpace planeSpace = PrimitiveSpace.XZ;

    private bool createBillboard, oldBillboardVal = false;
    private int tmpSubDivWidth, tmpSubDivHeight = 0;//used to save temporary the sub divs when setting the option to billboarding
    private bool flagChanged = false;

    public PlaneCreator() { }

    public override void DrawInterface() {
        planeSpace = (PrimitiveSpace) EditorGUILayout.EnumPopup("Plane Space: ", planeSpace);
        GUI.SetNextControlName ("Billboard");
        createBillboard = EditorGUILayout.Toggle("Billboard: ", createBillboard);
        width = EditorGUILayout.FloatField("Width:", width > 0.001f ? width : 0.001f);
        height = EditorGUILayout.FloatField("Height:", height > 0.001f ? height : 0.001f);
        
        if(flagChanged) {
            if(createBillboard) {
                tmpSubDivWidth = subDivWidth;
                tmpSubDivHeight = subDivHeight;
                subDivWidth = 0;
                subDivHeight = 0;
            } else {
                //restore old values
                subDivWidth = tmpSubDivWidth;
                subDivHeight = tmpSubDivHeight;
            }
            flagChanged = false;
            GUI.FocusControl("Billboard");//focus on the togle to release focus on fields in case they are focused
        }
        
        if(oldBillboardVal != createBillboard) {
            flagChanged = true;
            oldBillboardVal = createBillboard;
        }
        
        GUI.enabled = !createBillboard;
        subDivWidth = EditorGUILayout.IntField("Width Divisions:", subDivWidth >= 0 ? subDivWidth : 0);
        subDivHeight = EditorGUILayout.IntField("Height Divisions:", subDivHeight >=0 ? subDivHeight : 0);
        GUI.enabled = true;
    }

    public override Mesh CreatePrimitive() {
        return BuildPlane(width, height, subDivWidth, subDivHeight, planeSpace);
    }    
    #endif

    public static Mesh BuildPlane(float _width, float _height, int _subDivWidth, int _subDivHeight, PrimitiveSpace primitiveSpace = PrimitiveSpace.XZ) {
        Mesh generatedMesh = new Mesh();
        generatedMesh.Clear();
        int numberOfVertices = 4 + (_subDivWidth * 2) + (_subDivHeight * 2) + (_subDivWidth * _subDivHeight);
        Vector3[] vertices = new Vector3[numberOfVertices];
        Vector3[] normals = new Vector3[numberOfVertices];
        Vector2[] UVs = new Vector2[numberOfVertices];
        List <int> triangles = new List<int>();
        
        float uvFactorWidth = 1/(_subDivWidth+1);
        float uvFactorHeight = 1/(_subDivHeight+1);
        float widthScale = _width/(_subDivWidth+1);
        float heightScale = _height/(_subDivHeight+1);  
        
        Vector3 currentDrawnPoint = Vector3.zero;
        float wPos, hPos;//width position, height position
        
        for(int j = 0; j < _subDivHeight+2; j++) {
            for(int i = 0; i < _subDivWidth+2; i++) {
                wPos = i*widthScale - (_width/2);
                hPos = j*heightScale - (_height/2);
                
                switch(primitiveSpace) {
                case PrimitiveSpace.XY:
                    currentDrawnPoint = new Vector3(wPos, hPos, 0);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.forward;
                    break;
                case PrimitiveSpace.XZ:
                    currentDrawnPoint = new Vector3(wPos, 0, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.up;
                    break;
                case PrimitiveSpace.YZ:
                    currentDrawnPoint = new Vector3(0, wPos, hPos);
                    normals[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = Vector3.right;
                    break;   
                }
                
                vertices[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = currentDrawnPoint;
                UVs[Utils.ConvertToArrIndex(i,j,_subDivWidth+2)] = new Vector2(i*uvFactorWidth, j*uvFactorHeight);

                if(i < _subDivWidth + 1 && j < _subDivHeight + 1) {
                    if(primitiveSpace == PrimitiveSpace.XZ) {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivWidth+2));
                    } else {
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i,j+1,_subDivWidth+2));
                        
                        triangles.Add(Utils.ConvertToArrIndex(i,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j,_subDivWidth+2));
                        triangles.Add(Utils.ConvertToArrIndex(i+1,j+1,_subDivWidth+2));
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
