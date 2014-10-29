using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClipperLib;

public class PartLine : MonoBehaviour
{
	public ReferencePoint[] refPoints;
	public Vector3[] partLinePoints; //The points traced out that make up the partline itself
	private Section[] sections;
	[SerializeField] private InspectorFollowObject cameraFollower;
	private Plane plane;

	void Start() {
		refPoints = GetComponentsInChildren<ReferencePoint>();
		sections = GetComponentsInChildren<Section>();
		plane = new Plane(transform.forward, transform.position);
	}

	public void ShowRefPoints(bool visible) {
		foreach (ReferencePoint refPoint in refPoints) refPoint.Show (visible);
	}

	public bool PointInPartLine(ReferencePoint refPoint) {
		bool exists = false;
		foreach (ReferencePoint refPnt in refPoints) if (refPoint == refPnt) exists = true;
		return exists;
	}

	public void MakeSections() {
		//foreach (Section sec in sections) sec.BuildMesh(partLinePoints);
		//CalculateDomains();
		//foreach (Section sec in sections) sec.ActivateSection();
		HeadController.Instance.haircut.AddSection (new ProfileSection(true, HeadController.Instance.haircut.domain));
		HeadController.instance.haircut.AddSection (new ProfileSection(false, HeadController.Instance.haircut.domain));
	}

	public void CameraFollow(bool follow) {
		if (cameraFollower != null) cameraFollower.move = follow;
		if (!follow) InspectionOrbiter.Instance.InspectorObject = null;
	}

	void CalculateDomains() {


	}

	List<IntPoint> OrderList( List<IntPoint> list) {
		List<IntPoint> orderedList = new List<IntPoint>();
		for (int i = 0; i < list.Count; i++) {
			float minDist = 100000f;
			int ind = 0;
			for (int j = 0; j < orderedList.Count; j++) {
				Vector2 pointA = new Vector2((float)list[i].X, (float)list[i].Y);
				Vector2 pointB = new Vector2((float)orderedList[j].X, (float)orderedList[j].Y);
				float dist = Mathf.Abs ((pointA - pointB).magnitude);
				if (dist < minDist) {
					minDist = dist;
					ind = j;
				}
			}
			orderedList.Insert (ind+1, list[i]);
		}
		return orderedList;
	}
}
