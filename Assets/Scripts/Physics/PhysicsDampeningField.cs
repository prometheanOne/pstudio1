using System.Collections;
using UnityEngine;

public class PhysicsDampeningField : PhysicsEffector
{
	public override Vector3 DeltaV(Vector3 point, Vector3 velocity) {
		if (collider.bounds.Contains (point)) {
			return strength * -velocity;
		}
		else return Vector3.zero;
	}
}
