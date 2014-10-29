using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour {

	public Vector2 maxRot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = new Vector3(Screen.width - Input.mousePosition.x, Screen.height - Input.mousePosition.y, -1f);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint (mousePos);
		Vector3 lookDir = (worldPos - transform.position).normalized;
		Quaternion newRot = Quaternion.LookRotation (lookDir);
		transform.rotation = newRot;
		float xRot = (transform.localEulerAngles.x > 180f) ? transform.localEulerAngles.x - 360f : transform.localEulerAngles.x;
		float yRot = (transform.localEulerAngles.y > 180f) ? transform.localEulerAngles.y - 360f : transform.localEulerAngles.y;
		transform.localEulerAngles = new Vector3(Mathf.Clamp (xRot, -maxRot.x, maxRot.x), Mathf.Clamp (yRot, -maxRot.y, maxRot.y), 0f);
	}
	
}
