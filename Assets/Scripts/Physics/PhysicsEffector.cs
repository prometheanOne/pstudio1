using UnityEngine;
using System.Collections;

public class PhysicsEffector : MonoBehaviour
{
	public float strength;

	public virtual Vector3 DeltaV(Vector3 point, Vector3 velocity) {
		return Vector3.zero;
	}

	float SquareDistance (Vector3 point) {
		return Vector3.SqrMagnitude(point - transform.position);
	}
}
