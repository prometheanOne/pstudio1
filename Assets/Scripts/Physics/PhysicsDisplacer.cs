using UnityEngine;
using System.Collections;

public class PhysicsDisplacer : MonoBehaviour
{	
	public virtual Vector3 DeltaD(Vector3 point) {
		return Vector3.zero;
	}
}
