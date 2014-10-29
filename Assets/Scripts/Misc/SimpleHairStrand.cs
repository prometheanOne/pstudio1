using UnityEngine;
using System.Collections;

public class SimpleHairStrand : MonoBehaviour
{
	private bool parted;
	//public Vector3 naturalDirection;
	public Vector2 headCoords;
	public bool hairline;
	[SerializeField] private Transform physicsHair;
	private Transform[] hairSegments;
	[SerializeField] private Transform armature;
	[SerializeField] private SkinnedMeshRenderer hairMesh;
	private Color normalColor;
	[SerializeField] private Color highlightColor;
	private Transform[] bones;
	private float boneLength;
	public bool debug;
	private bool clipped;
	[SerializeField] private Material semiTransparent;
	private Material opaque;

	void Start() {
		normalColor = hairMesh.material.color;
		opaque = hairMesh.material;
		bones = armature.GetComponentsInChildren<Transform>();
		hairSegments = physicsHair.GetComponentsInChildren<Transform>();
		StartCoroutine (SetHair());
	}

	/*void OnTriggerEnter(Collider other) {
		if (other.gameObject == Comb.Instance.gameObject && !parted) {
			//Vector3 fromRot = LR ? Vector3.right : Vector3.left;
			//Vector3 targetRotation = Quaternion.FromToRotation (fromRot, Comb.Instance.direction).eulerAngles;
			//iTween.RotateTo (gameObject, iTween.Hash ("rotation", targetRotation, "time", 0.5f));
			HeadController.Instance.CombTouchedHair(transform.position);
			parted = true;
		}
	}*/

	public void Init(Vector2 headCoordinates) {
		headCoords = headCoordinates;
	}

	public void SetHairline() {
		hairline = true;
	}

	IEnumerator SetHair() {
		yield return new WaitForSeconds (HeadController.Instance.hairSettleTime);
		int numSegs = hairSegments.Length - 1;
		int numBones = bones.Length - 1;
		float increment = (float)numSegs / (float)numBones;
		if (debug) Debug.Log ("Inc: "+increment.ToString ("F2"));
		for (int i = 1; i < numBones; i++) {
			float t = ((i-1) * increment) % 1f;
			if(debug) Debug.Log (t);
			int index = (int)((i-1) * increment)+1;
			if (debug) Debug.Log (index);
			//Vector3 origAng = bones[i].localEulerAngles;
			Quaternion newRot;
			if (index < hairSegments.Length - 1) {
				Vector3 newPos = Vector3.Lerp (hairSegments[index].position, hairSegments[index+1].position, t);
				bones[i].position = newPos;
				//if (HeadController.Instance.InsideHead(newPos)) newPos = HeadController.Instance.ClosestPointOnHead(newPos).point;
				newRot = Quaternion.Lerp (hairSegments[index].rotation, hairSegments[index+1].rotation, t);
			}
			else {
				newRot = Quaternion.Lerp (hairSegments[index].rotation, Quaternion.Euler (Vector3.zero), t);
			}
			//Vector3 newAng = Vector3.Slerp (hairSegments[index].eulerAngles, hairSegments[index+1].eulerAngles, t);
			bones[i].rotation = newRot;
			//if (origAng.x > 90) bones[i].localEulerAngles = new Vector3(bones[i].localEulerAngles.x + 180f, bones[i].localEulerAngles.y, bones[i].localEulerAngles.z); //Fudge for poor rigging!!
			//bones[i].eulerAngles = newAng;
		}
		foreach (Transform seg in hairSegments) seg.gameObject.SetActive (false);
		//collider.enabled = false;
	}

	public void Trans(bool hide) {
		//if (hide) hairMesh.material = semiTransparent;
		//else hairMesh.material = opaque;
	}

	int BonesToPoint(Vector3 point) {
		float dist = Mathf.Abs ((point - transform.position).magnitude);
		int index = 0;
		for (int i = 0; i < bones.Length; i++) {
			if (Mathf.Abs ((bones[i].transform.position - transform.position).magnitude) < dist) index = i;
		}
		return index;
	}

	public void ClipTo(Vector3 point) {
		clipped = true;
		OnHover (false);
		//Vector3 dir = (point - transform.position).normalized;
		int numBones = BonesToPoint (point);
		//if (numBones > 0) bones[numBones].localEulerAngles += new Vector3(0f, 0f, 180f);

		//Quaternion rot = Quaternion.FromToRotation (Vector3.left, dir);
		float scaleFactor = Mathf.Pow (0.2f, 1f/(float)numBones);
		if (debug) Debug.Log ("Scale factor set to: "+scaleFactor.ToString ("F2") + " Num bones set to :"+ numBones.ToString ());
		Vector3 newScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
		for (int i = 0; i < bones.Length; i++) {
			if (i < numBones) {
				Vector3 dir = (point - bones[i].position).normalized;
				bones[i].rotation = Quaternion.FromToRotation (Vector3.left, dir);
				bones[i].localScale = newScale;
			}
			else bones[i].localScale = Vector3.zero;
		}
	}

	public void OnHover(bool isOver) {
		if (isOver && !clipped) hairMesh.material.color = normalColor + (highlightColor.a * highlightColor);
		else hairMesh.material.color = normalColor;
	}
}

