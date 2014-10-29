using UnityEngine;
using System.Collections;

public class PhysicsRepulsor : PhysicsEffector
{
	[SerializeField] private float cutoffDistance;
	private float inverseCutoff;
	[SerializeField] private float friction;

	void Start() {
		//inverseCutoff = 1f/cutoffDistance;
	}

	public override Vector3 DeltaV(Vector3 point, Vector3 velocity) {
		if (enabled) {
			inverseCutoff = 1f/cutoffDistance;
			Vector3 disp = point - transform.position;
			float sqrDist = disp.sqrMagnitude;
			//if (sqrDist <= cutoffDistance) {
				Vector3 dir = disp.normalized;
				//Vector3 vel = -Vector3.Project (velocity, dir);
				//Debug.DrawRay(point, dir, Color.green);
				Vector3 fric = friction * -velocity;
				return (dir * strength * (1f/(Mathf.Pow(1 - (inverseCutoff*sqrDist),4)))) + fric;

				//return vel * strength;
			//}
			//else return Vector3.zero;
		}
		else return Vector3.zero;
	}
}