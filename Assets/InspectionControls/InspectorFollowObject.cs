using UnityEngine;
using System.Collections;

public class InspectorFollowObject : InspectorObject
{
	[SerializeField] private Transform[] path;
	[SerializeField] private Transform follow;
	private bool _move;
	public bool move
	{
		set
		{
			if (!_move && value) OnMouseUpAsButton ();
			_move = value;
		}
		get { return _move; }
	}

	//public void Init(Transform fol,Transform[] pathPoints) {
	//	follow = fol;
	//	path = pathPoints;
	//}

	void Update() {
		if (path != null && follow != null) {
			transform.position = follow.position;
			float distanceFromStart = (path[0].position - transform.position).magnitude;
			float distanceToEnd = (path[1].position - transform.position).magnitude;
			float pathFraction = distanceFromStart / (distanceFromStart + distanceToEnd);
			Vector3 newDir = Vector3.Slerp (path[0].forward, path[1].forward, pathFraction);
			Quaternion newRot = Quaternion.FromToRotation (Vector3.right, newDir);
			transform.rotation = newRot;
		}
	}
}