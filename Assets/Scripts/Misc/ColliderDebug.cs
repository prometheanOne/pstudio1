using UnityEngine;
using System.Collections;

public class ColliderDebug : MonoBehaviour
{
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hitInfo;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
				Debug.Log ("Click hit: "+hitInfo.collider.gameObject.name);
			}
		}
	}
}
				         
