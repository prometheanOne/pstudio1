using UnityEngine;
using System.Collections;

public class ScissorController : SingletonComponent<ScissorController>
{
	[SerializeField] private GameObject[] positions;
	[SerializeField] private GameObject handPositionWidget;
	[SerializeField] private float strandStartOffset;
	[SerializeField] private float xSensitivity;
	[SerializeField] private float ySnapThreshold;
	private Transform strand;
	private Transform guide;
	private int currentPosition;
	private bool inUse;
	private Vector2 lastPos;
	private Vector2 delta;
	private float yChange;
	private float hairLength;

	public void Use(Transform strnd) {
		strand = strnd;
		transform.position = strand.position + (strandStartOffset * -strand.right);
		transform.rotation = Quaternion.FromToRotation (transform.up, strand.right);
		handPositionWidget.SetActive (true);
		positions[0].SetActive (true);
		inUse = true;
	}

	public void Use(Transform strand, Transform guide) {
		//Overdirect strand to guide position unless guide is last cut strand (or in same subsection?)

	}

	public void OnChange(int selection) {
		for (int i = 0; i < positions.Length; i++) {
			positions[i].SetActive (i == selection);
			currentPosition = selection;
		}
	}

	void Update() {
		if (inUse && InputController.Instance.buttonPressed) {
			if (lastPos == null) {
				lastPos = InputController.Instance.pointerPosition;
				delta = Vector2.zero;
			}
			else {
				delta = InputController.Instance.pointerPosition - lastPos;
				yChange += delta.y;
				lastPos = InputController.Instance.pointerPosition;
			}
			RaycastHit hitInfo;
			if (collider.Raycast (Camera.main.ScreenPointToRay (InputController.Instance.pointerPosition), out hitInfo, Mathf.Infinity)) {
				if (Mathf.Abs (yChange) > ySnapThreshold) {
					if (Mathf.Sign (yChange) > 0) strand.gameObject.SendMessage ("IncreaseElevation");
					else strand.gameObject.SendMessage ("DecreaseElevation");
					yChange = 0f;
				}
				if (delta.x > 0f) {
					hairLength -= xSensitivity * delta.x;
					UpdatePosition();
				}
			}

		}
	}

	void UpdatePosition() {
	}




}
