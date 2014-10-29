using UnityEngine;
using System.Collections;

public class PhysicsExclusionZone : PhysicsDisplacer
{
	private MeshCollider zone;
	[SerializeField] private Transform center;
	[SerializeField] private float multiplier = 1f;

	void Start() {
		zone = GetComponent<MeshCollider>();
	}

	public override Vector3 DeltaD(Vector3 point) {
		if (zone.bounds.Contains (point) && enabled) {
			if (WithinCollider(point)) {
				return (ClosestPointOnCollider(point) - point) * multiplier;
			}
			else return Vector3.zero;
		}
		else return Vector3.zero;
	}

	bool WithinCollider(Vector3 point) {
		RaycastHit hitInfo;
		float maxSize = Mathf.Max (new float[]{zone.bounds.size.x, zone.bounds.size.y, zone.bounds.size.z});
		if (zone.Raycast (new Ray(point, center.position - point), out hitInfo, maxSize)) {
			return false;
		}
		else return true;
	}

	Vector3 ClosestPointOnCollider(Vector3 point) {
		RaycastHit hitInfo;
		Ray ray = new Ray(point, (center.position - point).normalized);
		ray.origin = ray.origin - (10 * ray.direction);
		zone.Raycast (ray, out hitInfo, 10f);
		return hitInfo.point;
	}
}
