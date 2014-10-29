using UnityEngine;
using System.Collections.Generic;
using ClipperLib;

public class ProfileSection : HeadDomain
{
	public float width; 

	public ProfileSection(bool left, HeadDomain parent) {
		width = 0f;
		Clipper clipper = new Clipper();
		List<IntPoint> sectPoints = new List<IntPoint>();
		if (left) {
			sectPoints.Add (new IntPoint(0, 180));
			sectPoints.Add (new IntPoint(0, 359));
			sectPoints.Add (new IntPoint(179, 359));
			sectPoints.Add (new IntPoint(179, 180));
		}
		else {
			sectPoints.Add (new IntPoint(179, 0));
			sectPoints.Add (new IntPoint(0, 0));
			sectPoints.Add (new IntPoint(0, 179));
			sectPoints.Add (new IntPoint(179, 179));
		}

		clipper.AddPath (sectPoints, PolyType.ptClip, true);
		foreach(PolyNode poly in parent.domain.Childs) clipper.AddPath (poly.Contour, PolyType.ptSubject, true);
		clipper.Execute (ClipType.ctIntersection, domain);
		CacheHairs();
	}

	public ProfileSection(float wdth, HeadDomain parent) {
		width = wdth;
		Vector3 leftSource = HeadController.Instance.headPointer.position - (HeadController.Instance.transform.right * 0.5f * width);
		Vector3 rightSource = HeadController.Instance.headPointer.position + (HeadController.Instance.transform.right * 0.5f * width);
		List<IntPoint> sectPoints = new List<IntPoint>();
		sectPoints.Add (new IntPoint(0,359));
		float xRot = 180f;
		Vector3 headPoint = Vector3.zero;
		while (headPoint == Vector3.zero) {
			Vector3 dir = Quaternion.Euler (xRot, 0f, 0f) * HeadController.Instance.transform.up;
			Ray probe = new Ray(leftSource - (3.0f * dir), dir);
			headPoint = HeadController.Instance.ClosestPointOnHead (probe);
			xRot--;
		}
		while (headPoint != Vector3.zero) {
			Vector3 dir = Quaternion.Euler (xRot, 0f, 0f) * HeadController.Instance.transform.up;
			Ray probe = new Ray(leftSource - (3.0f * dir), dir);
			headPoint = HeadController.Instance.ClosestPointOnHead (probe);
			if (headPoint != Vector3.zero) {
				sectPoints.Add (new IntPoint(HeadController.Instance.WorldToHeadCoordinate (headPoint)));
				xRot++;
			}
		}
		while (headPoint == Vector3.zero) {
			Vector3 dir = Quaternion.Euler (xRot, 0f, 0f) * HeadController.Instance.transform.up;
			Ray probe = new Ray(rightSource - (3.0f * dir), dir);
			headPoint = HeadController.Instance.ClosestPointOnHead (probe);
			if (headPoint != Vector3.zero) {
				sectPoints.Add (new IntPoint(HeadController.Instance.WorldToHeadCoordinate (headPoint)));
				xRot--;
			}
		}
		sectPoints.Add (new IntPoint(0,0));
		//Calculate intersection
		Clipper clipper = new Clipper();
		clipper.AddPath (sectPoints, PolyType.ptClip, true);
		foreach(PolyNode poly in parent.domain.Childs) clipper.AddPath (poly.Contour, PolyType.ptSubject, true);
		clipper.Execute (ClipType.ctIntersection, domain);
		CacheHairs ();
	}
}

