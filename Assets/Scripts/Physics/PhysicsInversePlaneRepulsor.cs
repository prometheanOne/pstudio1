using UnityEngine;
using System.Collections;

public class PhysicsInversePlaneRepulsor : PhysicsEffector
{
	private Plane plane;
	[SerializeField] float pushThreshold;
	void Start() {
		plane = new Plane(transform.forward, transform.position);
	}
	
	public override Vector3 DeltaV(Vector3 point, Vector3 velocity) {
		if (enabled) {
			plane.SetNormalAndPosition (transform.forward, transform.position);
			float dist = plane.GetDistanceToPoint (point);
			if (plane.GetSide (point) && Mathf.Abs (dist) < pushThreshold){
				//Debug.DrawRay (point, -transform.forward, Color.red);
				return transform.forward * strength * Mathf.Pow(dist, 4f);
			}
			else return Vector3.zero;
		}
		else return Vector3.zero;
	}
}
