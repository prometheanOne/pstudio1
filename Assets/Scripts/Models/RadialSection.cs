using UnityEngine;
using System.Collections.Generic;
using ClipperLib;

public class RadialSection : HeadDomain
{
	public float angle; //Angle to the vertical of the part line plane

	public RadialSection(Transform orientation, bool front, HeadDomain parent) {
		angle = orientation.localEulerAngles.x;
		CalculateDomain(orientation, front, parent);
	}

	void CalculateDomain (Transform orient, bool front, HeadDomain parent) {
		Ray leftRay = new Ray(orient.position - (3.0f * orient.right), orient.right);
		Vector3 leftHeadPoint = HeadController.Instance.ClosestPointOnHead (leftRay);
		Vector2 leftCenter = HeadController.Instance.WorldToHeadCoordinate(leftHeadPoint);
		Ray rightRay = new Ray(orient.position + (3.0f * orient.right), -orient.right);
		Vector3 rightHeadPoint = HeadController.Instance.ClosestPointOnHead (rightRay);
		Vector2 rightCenter = HeadController.Instance.WorldToHeadCoordinate(rightHeadPoint);

		List<IntPoint> sectPoints = new List<IntPoint>();
		Clipper clipper = new Clipper();
		foreach (PolyNode poly in parent.domain.Childs) clipper.AddPath (poly.Contour, PolyType.ptSubject, true);
		if (Mathf.Abs (angle) < 90f && angle != 0f) {
			int yShift = Mathf.RoundToInt (Mathf.Tan (Mathf.Abs (angle) * Mathf.Deg2Rad) * leftCenter.x);
			if (Mathf.Sign (angle) > 0) {
				int A1 = -yShift;
				int B1 = Mathf.RoundToInt (179f - rightCenter.x);
				int C1 = Mathf.RoundToInt ((A1 * rightCenter.x) + (B1 * rightCenter.y));
				int A2 = yShift;
				int B2 = Mathf.RoundToInt (179f - leftCenter.x);
				int C2 = Mathf.RoundToInt ((A2 * leftCenter.x) + (B2 * leftCenter.y));

				double det = (A1 * B2) - (A2 * B1);
				double xInt = (B2*C1 - B1*C2)/det;
				sectPoints.Add (new IntPoint(xInt, 179));
				sectPoints.Add (new IntPoint(179, leftCenter.y + yShift));
				sectPoints.Add (new IntPoint(179, rightCenter.y - yShift));
				clipper.AddPath (sectPoints, PolyType.ptClip, true);
				if (front) clipper.Execute (ClipType.ctDifference, domain);
				else clipper.Execute (ClipType.ctIntersection, domain);
			}
			else {
				int A1 = -yShift;
				int B1 = Mathf.RoundToInt (-rightCenter.x);
				int C1 = Mathf.RoundToInt ((A1 * rightCenter.x) + (B1 * rightCenter.y));
				int A2 = yShift;
				int B2 = Mathf.RoundToInt (-leftCenter.x);
				int C2 = Mathf.RoundToInt ((A2 * leftCenter.x) + (B2 * leftCenter.y));
				
				double det = (A1 * B2) - (A2 * B1);
				double xInt = (B2*C1 - B1*C2)/det;
				sectPoints.Add (new IntPoint(xInt, 179));
				sectPoints.Add (new IntPoint(0, leftCenter.y + yShift));
				sectPoints.Add (new IntPoint(0, rightCenter.y - yShift));
				clipper.AddPath (sectPoints, PolyType.ptClip, true);
				if (front) clipper.Execute (ClipType.ctIntersection, domain);
				else clipper.Execute (ClipType.ctDifference, domain);
			}
		}
		else if (angle == 0f) {
			sectPoints.Add (new IntPoint(0,Mathf.RoundToInt (rightCenter.y)));
			sectPoints.Add (new IntPoint(179, Mathf.RoundToInt (rightCenter.y)));
			sectPoints.Add (new IntPoint(179, Mathf.RoundToInt (leftCenter.y)));
			sectPoints.Add (new IntPoint(0, Mathf.RoundToInt (leftCenter.y)));
			clipper.AddPath (sectPoints, PolyType.ptClip, true);

			if (front) clipper.Execute (ClipType.ctIntersection, domain);
			else clipper.Execute (ClipType.ctDifference, domain);
		}
		else if (Mathf.Abs(angle) == 90f) {
			sectPoints.Add (new IntPoint(0,0));
			sectPoints.Add (new IntPoint(0,359));
			sectPoints.Add (new IntPoint((int)leftCenter.x, 359));
			sectPoints.Add (new IntPoint((int)leftCenter.x, 0));
			clipper.AddPath (sectPoints, PolyType.ptClip, true);
			if (front) clipper.Execute (ClipType.ctIntersection, domain);
			else clipper.Execute (ClipType.ctDifference, domain);
		}
		CacheHairs ();
	}
	
}