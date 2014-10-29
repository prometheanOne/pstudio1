using UnityEngine;
using System.Collections;

public class GridProbe : MonoBehaviour
{
	private bool touching;
	private MeshCollider headCollider;

	void Start() {
		headCollider = HeadController.Instance.headCollider;
	}

	void OnTriggerEnter(Collider other) {
		if (other == headCollider) touching = true;
		else touching = false;
	}

	void OnTriggerExit() {
		touching = false;
	}

	public Vector3 Probe() {
		RaycastHit hitInfo;
		Ray ray = new Ray(transform.position, headCollider.transform.position - transform.position);
		if (headCollider.Raycast(ray, out hitInfo, (headCollider.transform.position - transform.position).magnitude)) {
			return hitInfo.point;
		}
		else {
			ray.origin = ray.GetPoint(-10);
			ray.direction = -ray.direction;
			if (headCollider.Raycast (ray, out hitInfo, 10f)) {
				return hitInfo.point;
			}
			else return Vector3.zero;
		}
	}
}