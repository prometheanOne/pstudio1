using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClipperLib;

public class Section : MonoBehaviour
{
	public PolyTree domain;
	[HideInInspector] public List<Vector3> hairlinePnts = new List<Vector3>();
	[HideInInspector] public GameObject[] hairs;
	public float zHeight;
	private Mesh mesh;
	[SerializeField] private Vector3 apexOffset;
	[SerializeField] bool disableHairs;
	[SerializeField] private MeshFilter targetMesh;
	private bool activated;
	private GameObject hairKnot;
	[SerializeField] private float knotOffset;
	[SerializeField] private int subsectAxis;
	public bool pivoting;
	public int axis;
	private bool clipped;
	public PartLine partLine;


	void Start() {
		//hairlinePnts = new Vector3[hairLinePoints.Length];
		//for (int i = 0; i < hairlinePnts.Length; i++) {
			//hairlinePnts[i] = transform.InverseTransformPoint (hairLinePoints[i].position);
		//}
		//mesh = GetComponent<MeshFilter>().mesh;
		//mesh = targetMesh.mesh;
		collider.enabled = false;
		partLine = GetComponentInParent<PartLine>();
	}

	void GetHairs() {
		collider.enabled = true;
		GameObject[] allHairs = HeadController.Instance.hairs.ToArray();
		List<GameObject> hrs = new List<GameObject>();
		foreach (GameObject hair in allHairs) {
			if (collider.bounds.Contains (hair.transform.position)) {
				hrs.Add (hair);
				//if (hair.GetComponent<SimpleHairStrand>().hairline) hairlinePnts.Add (transform.InverseTransformPoint (hair.transform.position));
				if (hair.GetComponent<SimpleHairStrand>().hairline) hairlinePnts.Add (hair.transform.position);
			}
		}
		hairs = hrs.ToArray ();
		//collider.enabled = false;
	}

	public void ActivateSection() {
		GetHairs ();
		activated = true;
	}

	void Update() {
		if (activated) {
			RaycastHit hitInfo;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
				if (hitInfo.collider == collider) OnHover(true);
				else OnHover(false);
			}
			else OnHover(false);
		}
	}

	void OnHover(bool isOver) {
		if (isOver && activated) {
			foreach (GameObject hair in hairs) hair.SendMessage ("OnSectionHover", true);
		}
		else foreach (GameObject hair in hairs) hair.SendMessage ("OnSectionHover", false);
	}

	void OnMouseUpAsButton() {
		Debug.Log ("Section clicked");
		if (ToolBeltController.Instance.currentTool == BeltTool.clip) {
			ClipSection();
		}
		if (ToolBeltController.Instance.currentTool == BeltTool.comb && clipped) {
			HeadController.Instance.Subsection (this);
			collider.enabled = false;
		}
	}

	void ClipSection() {
		if (!clipped) {
		activated = false;
		//collider.enabled = false;
		clipped = true;
		Vector3 sum = Vector3.zero;
		foreach (GameObject hair in hairs) sum += hair.transform.position;
		Vector3 centroid = sum / (hairs.Length - 1);
		Vector3 sectionNormal = transform.rotation * Vector3.forward;
		Vector3 apex = centroid + (sectionNormal * zHeight) + apexOffset;
		foreach (GameObject hair in hairs) hair.SendMessage ("ClipTo", apex);
		hairKnot = HeadController.Instance.PlaceKnot(apex);
		hairKnot.transform.position -= hairKnot.transform.up * knotOffset;
		hairKnot.transform.parent = transform;
		}
	}


	public void BuildMesh(Vector3[] linePoints) {
		GetHairs();
		/*Vector3[] vertices = new Vector3[hairlinePnts.Length + linePoints.Length + 1];
		for (int i = 1; i <= linePoints.Length; i++) {
			vertices[i] = transform.InverseTransformPoint (linePoints[i - 1]);
		}
		for (int i = linePoints.Length + 1; i < vertices.Length; i++) {
			vertices[i] = hairlinePnts[i - linePoints.Length - 1];
		}*/

		//foreach (Vector3 point in linePoints) hairlinePnts.Add (transform.InverseTransformPoint (point));
		foreach (Vector3 point in linePoints) hairlinePnts.Add (point);
		List<Vector3> vertexList = new List<Vector3>();
		vertexList.Add (hairlinePnts[0]);
		vertexList.Add (hairlinePnts[1]);
		for (int i = 2; i < hairlinePnts.Count; i++) {
			float minDist = 100000f;
			int ind = 0;
			for (int j = 0; j < vertexList.Count; j++) {
				float dist = Mathf.Abs ((hairlinePnts[i] - vertexList[j]).magnitude);
				if (dist < minDist) {
					minDist = dist;
					ind = j;
				}
			}
			vertexList.Insert (ind+1, hairlinePnts[i]);
		}
		vertexList.Insert (0, Vector3.zero);
		Vector3[] vertices = vertexList.ToArray();

		Vector3 sum = Vector3.zero;
		foreach (Vector3 vertex in vertices) sum += vertex;
		Vector3 centroid = sum / (vertices.Length - 1);
		Vector3 sectionNormal = transform.rotation * Vector3.forward;
		Vector3 apex = centroid + (sectionNormal * zHeight) + apexOffset;
		vertices[0] = apex;

		//Vector3[] normals = new Vector3[vertices.Length];
		//normals[0] = sectionNormal;
		//normals[1] = -(((vertices[vertices.Length-1] - vertices[1]) + (vertices[2] - vertices[1])) / 2f).normalized;
		//for (int i = 2; i < normals.Length - 1; i++) {
			//normals[i] = -(((vertices[i-1] - vertices[i]) + (vertices[i+1] - vertices[i])) / 2f).normalized;
		//}
		//normals[vertices.Length-1] = -(((vertices[vertices.Length - 2] - vertices[vertices.Length - 1]) + (vertices[1] - vertices[vertices.Length - 1])) / 2f).normalized;

		Vector2[] uvs = new Vector2[vertices.Length];
		uvs[0] = new Vector2(0.5f, 0.5f);
		float angleInc = (2 * Mathf.PI) / (uvs.Length - 2);
		for (int i = 1; i < uvs.Length - 1; i++) {
			float u = (Mathf.Cos (angleInc * (i - 1)) / 2f) + 0.5f;
			float v = (Mathf.Sin (angleInc * (i - 1)) / 2f) + 0.5f;
			uvs[i] = new Vector2 (u,v);
		}
		uvs[uvs.Length - 1] = uvs[1];

		int[] triangles = new int[vertices.Length * 3];
		for (int i = 0; i < vertices.Length - 1; i++) {
			triangles[i * 3] = 0;
			triangles[i * 3 + 1] = i;
			triangles[i * 3 + 2] = i +1;
		}
		triangles[(vertices.Length - 1) * 3] = 0;
		triangles[(vertices.Length - 1) * 3 + 1] = vertices.Length - 1;
		triangles[(vertices.Length - 1) * 3 + 2] = 1;

		if (disableHairs) foreach (GameObject hair in hairs) hair.SetActive (false);

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		//mesh.normals = normals;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
	}
}