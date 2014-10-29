using UnityEngine;
using System.Collections;

public class HairClip : MonoBehaviour
{
	[SerializeField] private float offset;

	void Update() {
		if (InputController.Instance.buttonPressed) {
			Vector2 headCoord = HeadController.Instance.currentHeadCoord;
			if (headCoord.x > 0 && headCoord.y > 0) {
				RaycastHit headHit = HeadController.Instance.HeadToWorldVector (headCoord, offset);
				transform.position = headHit.point;
				transform.rotation = Quaternion.FromToRotation (Vector3.back, headHit.normal);
			}
		}
	}
}