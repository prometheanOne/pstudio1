/*
  Created by Juan Sebastian Munoz Arango 2013
  naruse@gmail.com
  all rights reserved
 */
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class MainMenu : EditorWindow {

    private int primitiveIndex = 0;

    private readonly ProPrimitiveType[] primitiveOptions = new ProPrimitiveType[] {ProPrimitiveType.Plane, ProPrimitiveType.Cube,
                                                                           ProPrimitiveType.Cylinder, ProPrimitiveType.Sphere,
                                                                           ProPrimitiveType.Capsule};

    //gets filled on the OnEnable function with the primitiveOptions[] enum
    private string[] primitiveOptionsStrArray;

    private PlaneCreator pln = new PlaneCreator();
    private CubeCreator cub = new CubeCreator();
    private CylinderCreator cyl = new CylinderCreator();
    private SphereCreator sph = new SphereCreator();
    private CapsuleCreator cap = new CapsuleCreator();
    private ProPrimitive selectedPrimitive;

    
    [MenuItem("Window/ProPrimitives")]
    private static void Init() {
        MainMenu window = (MainMenu) EditorWindow.GetWindow(typeof(MainMenu));
        window.Show();

    }

    void OnEnable() {
        primitiveOptionsStrArray = Array.ConvertAll(primitiveOptions, value => value.ToString());
    }
    
    void OnGUI() {
        primitiveIndex = GUILayout.SelectionGrid(primitiveIndex, primitiveOptionsStrArray, 5);
        
        switch(primitiveOptions[primitiveIndex]) {
        case ProPrimitiveType.Plane:
            selectedPrimitive = pln;
            break;
        case ProPrimitiveType.Cube:
            selectedPrimitive = cub;
            break;
        case ProPrimitiveType.Cylinder:
            selectedPrimitive = cyl;
            break;
        case ProPrimitiveType.Sphere:
            selectedPrimitive = sph;
            break;
        case ProPrimitiveType.Capsule:
            selectedPrimitive = cap;
            break;
        }
        selectedPrimitive.DrawInterface();

        if(GUILayout.Button("Create Primitive")) {            
            CreateGameObject(selectedPrimitive);
        }
    }

    private void CreateGameObject(ProPrimitive p) {
        Undo.RegisterSceneUndo("Create new GameObject");
        GameObject createdObject = new GameObject("GameObject");
 
        if(p.GetType() == typeof(PlaneCreator)) {
            createdObject.name = "Plane";
        } else if(p.GetType() == typeof(CubeCreator)) {
            createdObject.name = "Cube";
        } else if(p.GetType() == typeof(CylinderCreator)) {
            createdObject.name = "Cylinder";
        } else if(p.GetType() == typeof(SphereCreator)) {
            createdObject.name = "Sphere";
        } else if(p.GetType() == typeof(CapsuleCreator)) {
            createdObject.name = "Capsule";
        }

        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = p.CreatePrimitive();
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        createdObject.AddComponent<MeshCollider>();
        Selection.activeGameObject = createdObject;
    }
}
