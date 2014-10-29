using System.Collections;
using UnityEngine;

public class OptionCollider : MonoBehaviour
{
	[SerializeField] private int function;
	[SerializeField] private float dragThreshold;
	private InfiniteScrollOption option;
	private Vector2 movement;

	void Start() {
		option = GetComponentInParent<InfiniteScrollOption>();
	}

	void OnClick() {
		switch(function) {
		case 1:
			option.ChangeSelection (true);
			break;
		case -1:
			option.ChangeSelection (false);
			break;
		}
	}

	void OnDrag(Vector2 delta) {
		movement += delta;
	}

	void OnDragEnd() {
		if (movement.y > dragThreshold) option.ChangeSelection (true);
		else if (movement.y < -dragThreshold) option.ChangeSelection(false);
	}
}
