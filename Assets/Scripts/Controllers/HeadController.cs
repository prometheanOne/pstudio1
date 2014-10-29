using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum subsectionType {
	horizontal = 0,
	vertical,
	pivoting
}

public class HeadController : SingletonComponent<HeadController>
{
	private HeadView headView;
	private ReferencePointView[] refPointViews;
	public MeshCollider headCollider;
	[SerializeField] private GameObject tempHeadColliders;
	public LayerMask headLayer;
	public LayerMask hiddenLayer;
	//[SerializeField] private Texture2D hairlineTexture;
	private bool[,] hairlineArray = new bool[180,360];
	public Transform headPointer;
	[SerializeField] private float crownAngle = 50.0f;
	[SerializeField] private int yHairDensity = 50;
	[SerializeField] private float xHairDensity = 2f;
	[SerializeField] private GameObject hairPrefab;
	public List<GameObject> hairs = new List<GameObject>();
	public float hairSettleTime = 2.0f;
	public GameObject hairKnotPrefab;
	public SubsectionView subsectView;
	public Subsection currentSubsection;
	public GameObject subsectionHairGroupPrefab;
	public Haircut haircut;
	public Vector2 currentHeadCoord = new Vector2(-1f, -1f);
	public Vector3 currentWorldCoord = Vector3.zero;
	public bool enableSections;
	[SerializeField] private UILabel debug;

	void Start() {
		headView = GetComponent<HeadView>();
		haircut = GetComponent<Haircut>();
		refPointViews = GetComponentsInChildren<ReferencePointView>();
		//ShowAllRefPoints(true);
		Color[] hairlineCols = haircut.headMap.GetPixels ();
		Debug.Log (hairlineCols.Length);
		for (int i = 0; i < 360; i++) {
			for (int j = 0; j < 180; j++) {
				hairlineArray[j,i] = hairlineCols[(i*180) + j].a > 0.5f;
			}
		}
		GrowHair();
	}
	
	public void ShowAllRefPoints(bool visible) {
		foreach (ReferencePointView refPointView in refPointViews) refPointView.visible = visible;
	}

	public void ReferencePointClicked(RefPoint refPoint) {
		HideHairs(true);
		headView.ReferencePointClicked(refPoint);
	}

	public void CombEnteredPoint(RefPoint refPoint) {
		//HideHairs(false);
		headView.ReferencePointCombed(refPoint);
	}

	public void CombTouchedHair(Vector3 position) {
		headView.AddPointToLine(Comb.Instance.transform.position);
	}

	public void AddLinePoint(Vector3 point) {
		headView.AddPointToLine(point);
	}

	public RaycastHit ClosestPointOnHead(Vector3 point) {
		RaycastHit hitInfo;
		Ray ray = new Ray(point, (headPointer.position - point).normalized);
		if (headCollider.Raycast(ray, out hitInfo, (headPointer.position - point).magnitude)) {
			return hitInfo;
		}
		else {
			ray.origin = ray.origin - (10 * ray.direction);
			headCollider.Raycast (ray, out hitInfo, 10f);
			return hitInfo;
		}
	}

	public Vector3 ClosestPointOnHead(Vector3 point, float offset) {
		RaycastHit hitInfo;
		Ray ray = new Ray(point, (headPointer.position - point).normalized);
		if (headCollider.Raycast(ray, out hitInfo, (headPointer.position - point).magnitude)) {
		}
		else {
			ray.origin = ray.origin - (10 * ray.direction);
			headCollider.Raycast (ray, out hitInfo, 10f);
		}
		Vector3 hitPoint = hitInfo.point + (hitInfo.normal * offset);
		return hitPoint;
	}

	public Vector3 ClosestPointOnHead(Ray ray) {
		RaycastHit hitInfo;
		if (headCollider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
			return hitInfo.point;
		}
		else {
			return Vector3.zero;
		}
	}

	public Vector3 ClosestPointOnHead(Ray ray, float offset) {
		RaycastHit hitInfo;
		if (headCollider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
			return hitInfo.point - (offset * ray.direction);
		}
		else {
			return Vector3.zero;
		}
	}

	public bool InsideHead(Vector3 point) {
		RaycastHit hitInfo;
		Ray ray = new Ray(point, (headPointer.position - point).normalized);
		if (headCollider.Raycast(ray, out hitInfo, (headPointer.position - point).magnitude)) {
			return false;
		}
		else return true;
	}

	public bool HasHair(Vector3 point) {
		/*Vector3 dir = point - headPointer.transform.position;
		float mag = dir.magnitude;
		//Debug.DrawRay (point, -dir, Color.green, 1f);
		float yRot = Mathf.Acos (-dir.z / mag) * Mathf.Rad2Deg;
		float xRot = Mathf.Asin (dir.y / mag) * Mathf.Rad2Deg;
		if (yRot >= 360f) yRot -= 360f;
		if (yRot < 0f) yRot += 360f;
		yRot = Mathf.Clamp (Mathf.Abs (yRot), 0f, 359f);
		xRot = Mathf.Clamp (90f - xRot, 0f, 179f);
		//Debug.Log ("xRot: "+xRot.ToString ()+" yRot: "+yRot.ToString ());*/
		Vector2 headCoord = WorldToHeadCoordinate(point);
		//return hairlineArray[Mathf.RoundToInt (xRot), Mathf.RoundToInt (yRot)];
		return hairlineArray[(int)headCoord.x, (int)headCoord.y];
	}

	public bool HasHair(Vector2 headCoords) {
		return hairlineArray[(int)headCoords.x, (int)headCoords.y];
	}

	public Vector2 WorldToHeadCoordinate(Vector3 point) {
		Vector3 dir = point - headPointer.transform.position;
		float magX = dir.magnitude;
		float magY = Mathf.Sqrt ((dir.x * dir.x) + (dir.z * dir.z));
		float yRot = Mathf.Acos (dir.z / magY) * Mathf.Rad2Deg;
		float xRot = Mathf.Asin (dir.y / magX) * Mathf.Rad2Deg;
		if (dir.x < 0) yRot = 360f - yRot;
		if (yRot > 360f) yRot -= 360f;
		if (yRot < 0f) yRot += 360f;
		yRot = Mathf.Clamp (yRot, 0f, 359f);
		xRot = Mathf.Clamp (90f - xRot, 0f, 179f);
		return new Vector2(Mathf.RoundToInt (xRot), Mathf.RoundToInt(yRot));
	}

	public Vector3 HeadToWorldCoordinate(Vector2 headCoord) {
		return ClosestPointOnHead(headPointer.position + (Quaternion.Euler (new Vector3(headCoord.x, headCoord.y, 0f)) * Vector3.up)).point;
	}

	public RaycastHit HeadToWorldVector(Vector2 headCoord, float offset) {
		RaycastHit hit = ClosestPointOnHead(headPointer.position + (Quaternion.Euler (new Vector3(headCoord.x, headCoord.y, 0f)) * Vector3.up));
		hit.point += hit.point + (offset * hit.normal);
		return hit;
	}

	public void PartComplete() {
		HideHairs (false);
		ShowAllRefPoints (false);
	}

	void HideHairs(bool hide) {
		foreach (GameObject hair in hairs) hair.SendMessage ("Trans", hide, SendMessageOptions.DontRequireReceiver);
	}

	/*void GrowHair() {
		float xInc = xHairDensity;
		float i = 0f;
		//Quaternion crownRot = Quaternion.Euler (new Vector3(-crownAngle, 0f, 0f));
		GameObject lastHairL = new GameObject();
		GameObject lastHairR = new GameObject();
		bool inHair = true;
		while (i <= 179f) {
			float yInc = 179f / Mathf.Clamp(yHairDensity * Mathf.Sin (i * Mathf.Deg2Rad),1, 100000);
			float j = 180f;
			while (j <= 359f) {
				bool hairPresent = hairlineArray[Mathf.RoundToInt (i), Mathf.RoundToInt (j)];
				if (hairPresent) {
					headPointer.eulerAngles = new Vector3 (-i, j, 0f);
					Vector3 rayDir = -(headPointer.rotation * Vector3.up);
					Vector3 rayOrg = headPointer.position - (10 * rayDir);
					Ray ray = new Ray (rayOrg, rayDir);
					RaycastHit hitInfo;
					if (headCollider.Raycast (ray, out hitInfo, 10f)) {
						Quaternion rot = Quaternion.FromToRotation (Vector3.left, new Vector3(hitInfo.normal.x, 0f, Mathf.Clamp(hitInfo.normal.z, -1f, 0f)));
						GameObject newHair = Instantiate(hairPrefab, hitInfo.point, rot) as GameObject;
						newHair.transform.parent = transform;
						newHair.SendMessage ("Init", new Vector2(Mathf.RoundToInt (i), Mathf.RoundToInt (j)));
						if (!inHair) newHair.SendMessage ("SetHairline");
						lastHairL = newHair;
						hairs.Add (newHair);
					}
					headPointer.eulerAngles = new Vector3(-i, -j, 0f);
					//headPointer.rotation *= crownRot;
					ray.direction = -(headPointer.rotation * Vector3.up);
					ray.origin = headPointer.position - (10 * ray.direction);
					if (headCollider.Raycast (ray, out hitInfo, 10f)) {
						Quaternion rot = Quaternion.FromToRotation (Vector3.left, new Vector3(hitInfo.normal.x, 0f, Mathf.Clamp(hitInfo.normal.z, -1f, 0f)));
						GameObject newHair = Instantiate(hairPrefab, hitInfo.point, rot) as GameObject;
						newHair.transform.parent = transform;
						newHair.SendMessage ("Init", new Vector2(Mathf.RoundToInt (i), Mathf.RoundToInt (j)));
						if (!inHair) newHair.SendMessage ("SetHairline");
						lastHairR = newHair;
						hairs.Add (newHair);
					}
					inHair = true;
				}
				else if (inHair) {
					lastHairL.SendMessage ("SetHairline");
					lastHairR.SendMessage ("SetHairline");
					inHair = false;
				}
				j += yInc;
			}
			i += xInc;
		}
		StartCoroutine (DisableTempColliders());
	}*/

	void GrowHair() {
		float xInc = xHairDensity;
		float i = 0f;
		while (i <= 179f) {
			float yInc = 179f / Mathf.Clamp (yHairDensity * Mathf.Sin (i * Mathf.Deg2Rad), 1, 100000);
			float j = 0f;
			while (j <= 359f) {
				bool hairPresent = hairlineArray[Mathf.RoundToInt (i), Mathf.RoundToInt (j)];
				if (hairPresent) {
					Vector3 rayDir = Quaternion.Euler (new Vector3(i, j, 0f)) * Vector3.down;
					Vector3 rayOrg = headPointer.position - (3 * rayDir);
					Ray ray = new Ray(rayOrg, rayDir);
					Debug.DrawRay (rayOrg, rayDir, Color.green, 10f);
					RaycastHit hitInfo;
					if (headCollider.Raycast (ray, out hitInfo, 10f)) {
						Quaternion rot = Quaternion.FromToRotation (Vector3.down, new Vector3(hitInfo.normal.x, 0f, Mathf.Clamp(hitInfo.normal.z, -1f, 0f)));
						GameObject newHair = Instantiate(hairPrefab, hitInfo.point + (0.03f * rayDir), rot) as GameObject;
						newHair.transform.parent = transform;
						newHair.SendMessage ("Init", new Vector2(Mathf.RoundToInt (i), Mathf.RoundToInt (j)),SendMessageOptions.DontRequireReceiver);
						hairs.Add (newHair);
					}
				}
				j += yInc;
			}
			i += xInc;
		}
		StartCoroutine (DisableTempColliders ());
	}

	public GameObject PlaceKnot(Vector3 location) {
		Vector3 direction = (location - headPointer.transform.position).normalized;
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, direction);
		GameObject newKnot = Instantiate(hairKnotPrefab, location, rot) as GameObject;
		return newKnot;
	}

	IEnumerator DisableTempColliders() {
		yield return new WaitForSeconds(hairSettleTime);
		tempHeadColliders.SetActive (false);
	}

	public void Subsection(Section sect) {
		subsectView.AddSection (sect);
	}

	public void StartCutting(Transform strand) {
		if (currentSubsection != null && ToolBeltController.Instance.currentTool == BeltTool.shears) {
			if (haircut.sections.Count > 0) SelectGuideSubsection();
			else currentSubsection.StartCutting(strand);
		}
	}

	void SelectGuideSubsection() {
		//Select a previously cut subsection strand to use as guide, then call StartCutting(strand  , guide)
	}
	 
	public int SnapToHairlineX(int xCoord) {
		int snapped = xCoord;
		if (HasHair(new Vector2((float)xCoord, 0f))) {
			while (HasHair(new Vector2((float)snapped, 0f))) snapped++;
		}
		else {
			while (HasHair(new Vector2((float)snapped, 0f))) snapped--;
		}
		return snapped;
	}

	public int SnapToHairlineY(int xCoord, int yCoord) {
		int sign = (int)Mathf.Sign ((float)yCoord);
		int snapped = yCoord;
		while (HasHair(new Vector2((float)xCoord, Mathf.Abs ((float)yCoord)))) snapped -= sign;
		return snapped;
	}

	public void SectionClicked (HeadDomain section) {
		//switch dependent on tool
	}
	
	void Update() {
		if (haircut.sections.Count > 1) {
			Ray probe = Camera.main.ScreenPointToRay(InputController.Instance.pointerPosition);
			Vector3 worldCoord = ClosestPointOnHead(probe);
			if (worldCoord != Vector3.zero) {
				currentWorldCoord = worldCoord;
				currentHeadCoord = WorldToHeadCoordinate(worldCoord);
				if (InputController.Instance.buttonPressed) haircut.OnClick(currentHeadCoord);
				else haircut.OnHoverEnter (currentHeadCoord);
			}
			else {
				currentHeadCoord = new Vector2(-1f, -1f);
				haircut.OnHoverOut ();
			}
			//debug.text = currentHeadCoord.ToString ();
		}
	}
}

