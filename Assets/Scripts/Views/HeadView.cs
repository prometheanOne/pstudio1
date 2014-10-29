using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class HeadView : MonoBehaviour
{
	public Transform comb;
	private RefPoint startPoint;
	private PartLine currentPartLine;
	private VectorLine currentLine;
	//private List<PartLine> partLines = new List<PartLine>();
	private List<Vector3> partLinePoints = new List<Vector3>();
	private bool parting;
	[SerializeField] private Material lineMaterial;
	[SerializeField] private Color lineColor;
	[SerializeField] private float lineWidth;
	[SerializeField] private float lineSectionLength;
	private Collider headCollider;

	void Start() {
		headCollider = HeadController.Instance.headCollider;
	}

	public void ReferencePointClicked(RefPoint refPoint) {
		if (!parting)
		{
			Comb.Instance.currentLine = refPoint.model.partLine;
			//refPoint.model.partLine.CameraFollow(true);
			startPoint = refPoint;
			currentPartLine = refPoint.model.partLine;
			HeadController.Instance.ShowAllRefPoints (false);
			refPoint.ShowConnectedPoints(true);
			partLinePoints.Add (refPoint.transform.position);
			partLinePoints.Add (refPoint.transform.position);
			currentLine = new VectorLine("Part Line", partLinePoints.ToArray (), lineColor, lineMaterial, lineWidth, LineType.Continuous, Joins.Fill);
			currentLine.Draw3DAuto();
			parting = true;
		}
	}

	public void ReferencePointCombed (RefPoint refPoint) {
		if (refPoint.model != startPoint.model && currentPartLine.PointInPartLine (refPoint.model) && parting) {
			//refPoint.model.partLine.CameraFollow(false);
			HeadController.Instance.PartComplete();
			partLinePoints[partLinePoints.Count - 1] = refPoint.transform.position;
			currentPartLine.partLinePoints = partLinePoints.ToArray ();
			currentPartLine.MakeSections ();
			parting = false;
			startPoint = null;
			partLinePoints.Clear();
			currentLine = null;
			currentPartLine = null;
			Comb.Instance.PutAway ();
			AmbientRewardController.Instance.ShowReward (refPoint.model.transform.position, "Section Complete!", Color.white);
		}
	}

	public void AddPointToLine (Vector3 point) {
		partLinePoints.Insert (partLinePoints.Count - 2, point);
		currentLine.Resize (partLinePoints.ToArray ());
	}
			
	void Update() {

		if (parting) {
			RaycastHit hitInfo;
			if (headCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 10f)) {
				Vector3 planePnt = currentPartLine.collider.ClosestPointOnBounds (hitInfo.point);
				//Vector3 headPoint = ProjectPointOntoHead(planePnt);
				Vector3 headPoint = HeadController.Instance.ClosestPointOnHead (planePnt).point;
				//partLinePoints[partLinePoints.Count - 1] = headPoint;
				Comb.Instance.MoveTo(headPoint);
			}
		}
	}
	
}