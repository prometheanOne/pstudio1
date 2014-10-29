using System.Collections;
using UnityEngine;

public class Comb : SingletonComponent<Comb>
{
	public PartLine currentLine;
	public Vector3 direction;
	//private Vector3 lastPos;
	private Vector3 homePos;
	[SerializeField] private float segmentLength;
	private float movedBy;
	[SerializeField] private float maxDelta;
	private bool firstMove = true;

	void Start() {
		homePos = transform.parent.position;
	}

	public void MoveTo(Vector3 pos) {
		if (currentLine != null) {
			float delta;
			if (!firstMove) delta = Mathf.Clamp ((pos - transform.position).magnitude, 0f, maxDelta);
			else delta = (pos - transform.position).magnitude;
			direction = (pos - transform.position).normalized;
			transform.parent.position += delta * direction;
			movedBy += delta;
			firstMove = false;
			
			if (movedBy >= segmentLength) {
				//Debug.Log ("Adding line point");
				HeadController.Instance.AddLinePoint(transform.position);
				movedBy = 0f;
			}
		}
	}

	/*void Update() {
		if (currentLine != null) {
			float delta = (transform.position - lastPos).magnitude;
			if (delta > 0) {
				direction = (transform.position - lastPos).normalized;
				lastPos = transform.position;
				movedBy += delta;
			}

			if (movedBy >= segmentLength) {
				Debug.Log ("Adding line point");
				HeadController.Instance.AddLinePoint(transform.position);
				movedBy = 0f;
			}
		}
	}*/

	public void PutAway() {
		transform.parent.position = homePos;
		currentLine = null;
		//lastPos = homePos;
		direction = Vector3.zero;
		firstMove = true;
	}
}
