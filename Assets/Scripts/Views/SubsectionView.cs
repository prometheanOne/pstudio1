using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class SubsectionView : MonoBehaviour
{
	[SerializeField] private bool pivoting;
	[SerializeField] private Vector3[] orientations;
	[SerializeField] private Vector3[] movementAxes;
	private int axis;
	//[SerializeField] private BoxCollider subsectionContainer;
	private Vector3[] rotationAxis = new Vector3[2];

	private List<Section> sections = new List<Section>();
	private List<SimpleHairStrand> subsectHairs = new List<SimpleHairStrand>();
	private List<SimpleHairStrand> allHairs = new List<SimpleHairStrand>();
	private List<SimpleHairStrand> hairlineHairs = new List<SimpleHairStrand>();
	private VectorLine line;

	[SerializeField] private float lineSegments;
	[SerializeField] private float lineWidth;
	[SerializeField] private Material lineMaterial;
	[SerializeField] private float lineOffset;

	private Plane plane;
	private Vector3 startPoint;
	[SerializeField] private GameObject subsectionPrefab;
	private float currentSize;
	[SerializeField] private float rightSize;
	[SerializeField] private float wrongSize;
	[SerializeField] private Color wrongColor;
	[SerializeField] private Color medColor;
	[SerializeField] private Color rightColor;

	public void Init(Section sect) {
		plane = new Plane(transform.forward, transform.position);
		collider.enabled = true;
		//subsectionContainer.enabled = true;
		pivoting = sect.pivoting;
		axis = sect.axis;
		startPoint = sect.partLine.refPoints[0].transform.position; //This assumes that the first reference point in the array is always the one where subsectioning starts
		Vector3[] linePoints = new Vector3[2];
		line = new VectorLine("Subsection Line", linePoints, new Color(1f, 1f, 1f, 0f), lineMaterial, lineWidth, LineType.Continuous, Joins.Fill);
		line.Draw3DAuto();
	}

	public void AddSection(Section sect) {
		if (sections.Count == 0) Init (sect);
		if (sections.Count < 2 && !sections.Contains (sect)) {
			sections.Add (sect);
		}
	}

	void GetAllHairs() {
		foreach (Section section in sections.ToArray()) {
			foreach (GameObject hair in section.hairs) {
				allHairs.Add (hair.GetComponent<SimpleHairStrand>());
			}
		}
	}

	void GetSubsectHairs() {
		subsectHairs.Clear ();
		hairlineHairs.Clear ();
		foreach (SimpleHairStrand hair in allHairs) {
			if (plane.GetSide (hair.transform.position)) subsectHairs.Add (hair);
			if (hair.hairline) hairlineHairs.Add (hair);
		}
	}

	void Update() {
		if (sections.Count > 0) {
			Vector3 headHit = HeadController.Instance.ClosestPointOnHead (Camera.main.ScreenPointToRay(Input.mousePosition));
			if (headHit != Vector3.zero) {
				Vector3 delta = headHit - collider.ClosestPointOnBounds(headHit);
				transform.position += delta;
				Color lineCol;
				currentSize = plane.GetDistanceToPoint (startPoint);
				if (currentSize > 0) {
					if (currentSize > wrongSize) lineCol = wrongColor;
					else {
						float error = Mathf.Abs (rightSize - currentSize);
						float t = error / Mathf.Abs (rightSize - wrongSize);
						if (t < 0.5f) lineCol = Color.Lerp (rightColor, medColor, t * 2f);
						else if (t == 0.5f) lineCol = medColor;
						else lineCol = Color.Lerp (medColor, wrongColor, t * 2f);
					}
				}
				else lineCol = wrongColor;
				plane.SetNormalAndPosition(transform.forward, transform.position);
				float inc = 1f / lineSegments;
				//Find points on the head for partline
				Vector3 planeCenter = collider.bounds.center;
				List<Vector3> linePoints = new List<Vector3>();
				for (int i = 0; i < lineSegments; i++) {
					float r = Mathf.Lerp (0f, 2 * Mathf.PI, i * inc);
					Vector3 dir = transform.rotation * new Vector3(-Mathf.Cos (r), Mathf.Sin (r), 0f);
					Vector3 point = planeCenter + (5f * dir);
					//Debug.DrawLine (point, planeCenter);
					Ray pointRay = new Ray(point, -dir);
					Vector3 pnt = HeadController.Instance.ClosestPointOnHead(pointRay, lineOffset);
					if (HeadController.Instance.HasHair (pnt)) {
						if (pnt != Vector3.zero) linePoints.Add (pnt);
					}
				}
				if (linePoints.Count >= 2) line.Resize (linePoints.ToArray ());
				line.SetColor (lineCol);
			}
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hitInfo;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
					if (hitInfo.collider == HeadController.Instance.headCollider) OnMouseUpAsButton();
					//Debug.Log ("click hit: "+hitInfo.collider.gameObject.name);
				}
			}
		}
	}

	void OnMouseUpAsButton() {
		//Debug.Log ("Subsection collider clicked");
		GetAllHairs ();
		GetSubsectHairs ();
		GameObject newSubsection = Instantiate(subsectionPrefab, transform.position, transform.rotation) as GameObject;
		Subsection subsec = newSubsection.GetComponent<Subsection>();
		subsec.Init (plane, subsectHairs.ToArray (), line.points3, sections.ToArray (), subsectionType.horizontal);
		HeadController.Instance.currentSubsection = subsec;
		foreach (Section sect in sections) sect.collider.enabled = false;
		sections.Clear ();
		collider.enabled = false;
		VectorLine.Destroy (ref line);
		HeadController.Instance.ShowAllRefPoints (false);
		//subsectionContainer.enabled = false;
	}

	/*void OnDisable() {
		//VectorLine.Destroy (ref line);
		sections.Clear ();
	}*/

}
