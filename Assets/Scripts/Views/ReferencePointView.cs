using UnityEngine;
using System.Collections;
using Vectrosity;

public class ReferencePointView : MonoBehaviour
{
	private ReferencePoint refPoint;
	private MeshCollider headCollider;
	private bool _visible;
	public bool visible
	{
		set {
			_visible = value;
			ShowRefPoint();
		}

		get { return _visible; }

	}

	private Vector3[] ringPoints = new Vector3[20];
	private VectorLine ring;
	[SerializeField] private GameObject pointSphere;
	private Color normal = Color.white;
	private Color hover = Color.red;
	private Color click = Color.red;
	[SerializeField] private float ringWidth;
	[SerializeField] private Material ringMaterial;
	[SerializeField] private float ringMaxRadius;
	[SerializeField] private float ringGrowPeriod;
	[SerializeField] private float zOffset;
	private bool hovering;
	private bool clicked;

	void Start() {
		refPoint = GetComponent<ReferencePoint>();
		headCollider = HeadController.Instance.headCollider;
		//Snap reference point to head's surface
		/*RaycastHit hitInfo;
		if (Physics.Linecast (transform.position, headCollider.transform.position, out hitInfo, HeadController.Instance.headLayer)) {
			if (hitInfo.collider == headCollider) {
				transform.position = hitInfo.point;
				transform.rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
			}
		}
		transform.position = headCollider.ClosestPointOnBounds(transform.position);*/
		RaycastHit closestPoint = HeadController.Instance.ClosestPointOnHead(transform.position);
		transform.position = closestPoint.point;
		transform.rotation = Quaternion.FromToRotation (Vector3.forward, closestPoint.normal);
	}

	void ShowRefPoint() {
		//Debug.Log ("Showing or hiding ref point");
		pointSphere.SetActive (_visible);
		if (_visible) ring = new VectorLine("ring",ringPoints, ringMaterial, ringWidth, LineType.Continuous, Joins.Weld);
		else VectorLine.Destroy (ref ring);
	}

	void Update() {
		if (_visible && ring != null) {
			float radius = ringMaxRadius + (ringMaxRadius * 2.0f * ((Time.time / ringGrowPeriod) - Mathf.FloorToInt (0.5f + (Time.time / ringGrowPeriod))));
			ring.MakeCircle (transform.position + (transform.rotation * Vector3.forward * zOffset), transform.rotation * Vector3.forward, radius, 19);
			float step = (ringMaxRadius - radius) / ringMaxRadius;
			ring.SetColor (new Color(ring.color.r, ring.color.g, ring.color.b, step));
			float lw = step * ringWidth;
			ring.SetWidths (new float[]{lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw,lw});
			ring.Draw3D();
		}
	}

	void OnMouseUpAsButton() {
		Debug.Log ("ref point clicked");
		HeadController.Instance.ReferencePointClicked(new RefPoint(refPoint, this));
	}

	void OnTriggerEnter(Collider other) {
		if (other == Comb.Instance.collider) HeadController.Instance.CombEnteredPoint(new RefPoint(refPoint, this));
	}
}
