using UnityEngine;
using System.Collections;
using ClipperLib;

public class Subsection : MonoBehaviour
{
	public PolyTree domain;
	private Section[] sections;
	public Plane plane;
	public SimpleHairStrand[] hairs;
	[SerializeField] private GameObject lateralArmature;
	private Transform[] lateralBones;
	private Vector3[] headPoints;
	private GameObject[] strands;
	private subsectionType type;
	private bool trackMouse;
	private Collider lastHover;

	void Start() {
		//lateralBones = lateralArmature.GetComponentsInChildren<Transform>();
	}

	public void Init(Plane pln, SimpleHairStrand[] hrs, Vector3[] headPnts, Section[] sectns, subsectionType tp) {
		plane = pln;
		hairs = hrs;
		headPoints = headPnts;
		sections = sectns;
		type = tp;
		//lateralBones = lateralArmature.GetComponentsInChildren<Transform>();
		//ShapeMesh();
		CalculateDomain();
		PlaceHairStrands();
		//HeadController.Instance.haircut.subsections.Add (this);
	}

	void ShapeMesh() {
		float[] lineLengths = new float[headPoints.Length - 1];
		for (int i = 0; i < lineLengths.Length; i++) {
			lineLengths[i] = (headPoints[i+1] - headPoints[i]).magnitude;
		}
		float pointsPerBone = (float)(headPoints.Length) / (float)(lateralBones.Length - 1);
		for (int i = 1; i < lateralBones.Length; i++) {
			float numPoints = i * pointsPerBone;
			int upper = Mathf.CeilToInt (numPoints);
			int lower = Mathf.FloorToInt (numPoints);
			float t = numPoints % 1f;
			//Vector3 dir = (upper - lower).normalized;
			//Quaternion rot = Quaternion.FromToRotation (Vector3.left, dir);
			lateralBones[i].position = Vector3.Lerp (headPoints[Mathf.Clamp (lower, 0, headPoints.Length - 1)], headPoints[Mathf.Clamp (upper, 0, headPoints.Length - 1)], t);
		}
	}

	void PlaceHairStrands() {
		GameObject hairStrandPrefab = HeadController.Instance.subsectionHairGroupPrefab;
		strands = new GameObject[headPoints.Length - 1];
		for (int i = 0; i < strands.Length; i++) {
			Vector3 delta = headPoints[i + 1] - headPoints[i];
			RaycastHit hitInfo = HeadController.Instance.ClosestPointOnHead (headPoints[i] + (delta * 0.5f));
			Vector3 pos = hitInfo.point;
			Quaternion rot = Quaternion.FromToRotation (Vector3.left, Vector3.down);
			GameObject newStrand = GameObject.Instantiate (hairStrandPrefab, pos, rot) as GameObject;
			newStrand.transform.localRotation = Quaternion.FromToRotation (newStrand.transform.up, new Vector3(hitInfo.normal.x, 0f, hitInfo.normal.z)) * newStrand.transform.localRotation;
			newStrand.transform.localScale = new Vector3(newStrand.transform.localScale.x, newStrand.transform.localScale.y, 10f * delta.magnitude);
			newStrand.transform.parent = transform;
			strands[i] = newStrand;
		}
		trackMouse = true;
	}

	public void StartCutting(Transform strand) {
		//strand.localEulerAngles += new Vector3(0f, 0f, -30f);
		ScissorController.Instance.Use (strand);
	}

	public void StartCutting(Transform strand, Transform guide) {
		ScissorController.Instance.Use (strand, guide);
	}

	void CalculateDomain() {

	}

	void Update() {
		if (trackMouse) {
			RaycastHit hitInfo;
			if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
				if (hitInfo.collider.transform.IsChildOf (transform)) {
					if (lastHover == null) {
						lastHover = hitInfo.collider;
						hitInfo.collider.gameObject.SendMessage ("OnHover", true);
					}
					else if (hitInfo.collider != lastHover) {
						hitInfo.collider.gameObject.SendMessage ("OnHover", true);
						lastHover.gameObject.SendMessage ("OnHover", false);
						lastHover = hitInfo.collider;
					}
				}
				else gameObject.BroadcastMessage("OnHover", false);
			}
			else gameObject.BroadcastMessage("OnHover", false, SendMessageOptions.DontRequireReceiver);
		}
	}
}	