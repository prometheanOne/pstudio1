using UnityEngine;
using System.Collections;

public class ReferencePoint : MonoBehaviour
{
	public PartLine partLine;

	void Start() {
		partLine = transform.parent.gameObject.GetComponent<PartLine>();
	}

	public ReferencePoint GetClosestPartPoint(Vector3 position) {
		ReferencePoint[] refPoints = partLine.refPoints;
		float nearest = 1000000f;
		int index = 0;
		for (int i = 0; i < refPoints.Length; i++) {
			float distance = (refPoints[i].transform.position - position).sqrMagnitude;
			if (distance < nearest) {
				nearest = distance;
				index = i;
			}
		}
		return refPoints[index];
	}

	public void Show(bool visible) {
		GetComponent<ReferencePointView>().visible = visible;
	}

	public void ShowConnectedPoints(bool visible) {
		partLine.ShowRefPoints(visible);
	}
}
