/*
  Created By Juan Sebastian Munoz Arango
  naruse@gmail.com
  all rights reserved
 */
using UnityEngine;
using System.Collections;

public abstract class ProPrimitive {

    #if UNITY_EDITOR
    public abstract void DrawInterface();

    public abstract Mesh CreatePrimitive();
    #endif

    public static GameObject CreatePlane(float width, float height, int subDivWidth = 0, int subDivHeight = 0, PrimitiveSpace primitiveSpace = PrimitiveSpace.XZ) {
        GameObject createdObject = new GameObject("Plane");
        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = PlaneCreator.BuildPlane(width, height, subDivWidth, subDivHeight, primitiveSpace);
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        return createdObject;
    }
    
    public static GameObject CreateCube(float width, float height, float depth, int subDivWidth = 1, int subDivHeight = 1, int subDivDepth = 1) {
        GameObject createdObject = new GameObject("Cube");
        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = CubeCreator.BuildCube(width, height, depth, subDivWidth, subDivHeight, subDivDepth);
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        return createdObject;        
    }
    
    public static GameObject CreateCylinder(float radius, float height, int subDivCircumference = 15, int subDivHeight = 0, PrimitiveSpace primitiveSpace = PrimitiveSpace.XZ) {
        GameObject createdObject = new GameObject("Cylinder");
        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = CylinderCreator.BuildCylinder(radius, height, subDivCircumference, subDivHeight, primitiveSpace);
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        return createdObject;
    }
    
    public static GameObject CreateSphere(float radius, int subDivisions = 15) {
        GameObject createdObject = new GameObject("Sphere");
        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = SphereCreator.BuildSphere(radius, subDivisions);
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        return createdObject;
    }

    public static GameObject CreateCapsule(float radius, float height, int subDivCircumference = 15, int subDivHeight = 0, PrimitiveSpace primitiveSpace = PrimitiveSpace.XZ) {
        GameObject createdObject = new GameObject("Capsule");
        createdObject.AddComponent<MeshRenderer>();
        createdObject.AddComponent<MeshFilter>().mesh = CapsuleCreator.BuildCapsule(radius, height, subDivCircumference, subDivHeight, primitiveSpace);
        createdObject.renderer.material = new Material(Shader.Find("Diffuse"));
        return createdObject;
    }
}
