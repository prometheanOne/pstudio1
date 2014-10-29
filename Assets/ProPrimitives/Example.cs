using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Create a Plane:
        // 1st Param Width of the plane: 
        // 2nd Param: Height of the plane
        // 3rd Param: (optional) Width subdivisions of the plane (Default: 0)
        // 4th Param: (optional) Height subdivisions of the plane (Default: 0)
        // 5th Param: (optional) Space where the plane is going to be drawn (Default: PrimitiveSpace.XZ)
        // 
        // to create a billboard just set subdivisions to 0 (3rd and 4th param)
        GameObject examplePlane = ProPrimitive.CreatePlane(1, 1, 1, 1, PrimitiveSpace.XY);
        examplePlane.transform.position = new Vector3(-5, 0, 0);

        //Create a Cube:
        // 1st Param: Width of the cube (along the X axis)
        // 2nd Param: Height of the cube (along the Y axis)
        // 3rd Param: Depth of the cube (along the Z axis)
        // 4th Param: (optional) width subdivisions (Default: 1)
        // 5th Param: (optional) height subdivisions (Default: 1)
        // 6th Param: (optional) depth subdivisions (Default: 1)
        GameObject exampleCube = ProPrimitive.CreateCube(1, 1, 1, 2, 10, 6);
        exampleCube.transform.position = new Vector3(-3,0,0);

        //Create a Cylinder:
        // 1st Param: Radius of the cylinder
        // 2nd Param: Height of the cylinder
        // 3rd Param: (optional) Number of subdivisions the circumference of the cylinder will have (Default: 15)
        // 4th Param: (optional) Number of subdivisions the height of the cylinder will have (Default: 0)
        // 5th Param: (optional) Space where the cylinder is going to be drawn (Default: PrimitiveSpace.XZ)
        GameObject exampleCylinder = ProPrimitive.CreateCylinder(0.8f, 2, 10, 2, PrimitiveSpace.XZ);
        exampleCylinder.transform.position = new Vector3(0,0,0);

        //Create a Sphere:
        // 1st Param: Radius of the Sphere
        // 2nd Param: (Optional) Subdivisions the sphere will have (Default: 15)
        GameObject exampleSphere = ProPrimitive.CreateSphere(0.7f, 25);
        exampleSphere.transform.position = new Vector3(2.5f,0,0);

        //Create a Capsule:
        // 1st Param: Radius if the capsule
        // 2nd Param: Height of the capsule
        // 3rd Param: (optional) Number of subdivisions the circumference of the capsule will have (Default: 15)
        // 4th Param: (optional) Number of subdivisions the height of the capsule will have (Default: 0)
        // 5th Param: (optional) Space where the capsule is going to be drawn (Default: PrimitiveSpace.XZ)
        GameObject exampleCapsule = ProPrimitive.CreateCapsule(0.8f, 2, 10, 2, PrimitiveSpace.XY);
        exampleCapsule.transform.position = new Vector3(5, 0, 0);
	}

}
