using UnityEngine;

[System.Serializable]

public class RefPoint
{
	public ReferencePoint model;
	public ReferencePointView view;
	public Transform transform;

	public RefPoint(ReferencePoint pnt, ReferencePointView vw) {
		model = pnt;
		view = vw;
		transform = pnt.transform;
	}

	public void ShowConnectedPoints(bool visible) {
		model.ShowConnectedPoints(visible);
	}
}

