using UnityEngine;
using System.Collections;

public enum RecenterTrigger {
	rightClick = 0,
	rightDoubleClick,
	leftDoubleClick
}

public class RecenterInspector : MonoBehaviour
{
	public LayerMask layerMask;
	public RecenterTrigger trigger;

	void OnClick() {
		if (trigger == RecenterTrigger.rightClick && UICamera.currentTouchID == -2) {
			CastRay ();
		}
	}

	void OnDoubleClick() {
		if (trigger == RecenterTrigger.leftDoubleClick || trigger == RecenterTrigger.leftDoubleClick) {
			switch (UICamera.currentTouchID) {
			case -1:
				if (trigger == RecenterTrigger.leftDoubleClick) CastRay();
				break;
			case -2:
				if (trigger == RecenterTrigger.rightDoubleClick) CastRay ();
				break;
			}
		}
	}

	void CastRay() {
		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hitInfo, layerMask)) {
			if (hitInfo.collider == collider) {
				InspectionOrbiter.Instance.Recenter (hitInfo.point);
			}
		}
	}
}