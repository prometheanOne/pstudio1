using UnityEngine;
using System.Collections;

public class PhysicsPlaneAttractor : PhysicsEffector
{
	private Plane plane;
	[SerializeField] private float pullFraction;
	[SerializeField] private float pushFraction;
	[SerializeField] private float pushThreshold;

	void Start() {
		plane = new Plane(transform.forward, transform.position);
	}

	public override Vector3 DeltaV(Vector3 point, Vector3 velocity) {
		if (enabled) {
			plane.SetNormalAndPosition (transform.forward, transform.position);
			Vector3 disp = transform.position - point;
			float sqrDist = disp.sqrMagnitude;
			Vector3 dir = disp.normalized;
			if (plane.GetSide (point)) {
				//Debug.DrawRay(point, dir, Color.green);
				return dir * pullFraction * strength * (1f/(sqrDist * sqrDist));
			}
			else if (Mathf.Abs (plane.GetDistanceToPoint (point)) < pushThreshold){
				Debug.DrawRay (point, -transform.forward, Color.red);
				return -transform.forward * pushFraction * strength * (1f/(sqrDist * sqrDist));
			}
			else return Vector3.zero;
		}
		else return Vector3.zero;
	}
}
