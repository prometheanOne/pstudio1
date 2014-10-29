using UnityEngine;
using System.Collections;

public class InputController : SingletonComponent<InputController>
{
	public bool mode;
	public Vector2 pointerPosition {
		get {
			if (mode) return Input.touches[0].position;
			else return Input.mousePosition;
		}
	}

	public bool buttonPressed {
		get {
			if (mode) return Input.touchCount > 0;
			else return Input.GetMouseButton (0);
		}
	}
}
