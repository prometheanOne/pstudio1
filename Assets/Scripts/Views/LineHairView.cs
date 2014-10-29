using UnityEngine;
using System.Collections;

public class LineHairView : MonoBehaviour
{
	private LineRenderer line;
	private PhysicsHair physHair;
	private Vector3[] points;
	private bool init;
	[SerializeField] private int interpolationFactor = 1;
	private float increment;
	[SerializeField] private Color highlightTint;
	private Color highlightColor;
	[SerializeField] private Color normalColor;
	private Material lineMat;

	void Start() {
		line = GetComponent<LineRenderer>();
		physHair = GetComponent<PhysicsHair>();
		StartCoroutine (GetLinePoints());
		increment = 1.0f / (float)interpolationFactor;
		line.SetColors (normalColor, normalColor);
		highlightColor = new Color(normalColor.r + (highlightTint.a * highlightTint.r), 
		                           normalColor.g + (highlightTint.a * highlightTint.g), 
		                           normalColor.b + (highlightTint.a * highlightTint.b), 
		                           normalColor.a);
		lineMat = line.material;
	}

	void Update() {
		if (init && physHair.simulate) {
			if (interpolationFactor == 1) {
				for(int i = 0; i < points.Length; i++) {
					line.SetPosition (i, points[i]+transform.position);
				}
			}
			else {
				Vector3[] midPoints = new Vector3[points.Length - 1];
				for(int i = 0; i < midPoints.Length; i++) {
					midPoints[i] = ((points[i] + points[i+1]) * 0.5f) + transform.position;
				}
				line.SetPosition (0, points[0] + transform.position);
				int counter = 0;
				for (int j = 0; j < midPoints.Length - 1; j++) {
					for (int k = 0; k < interpolationFactor; k++) {
						float t = k * increment;
						Vector3 pos = ((1-t)*(1-t)*midPoints[j])+(2*(1-t)*t*(points[j+1]+transform.position))+(t*t*midPoints[j+1]);
						line.SetPosition ((j*interpolationFactor)+k+1, pos);
						counter++;
					}
				}
				line.SetPosition (counter, midPoints[midPoints.Length - 1]);
				line.SetPosition (counter+1, points[points.Length - 1]+transform.position);
			}
		}
	}

	IEnumerator GetLinePoints() {
		yield return new WaitForSeconds(0.1f);
		points = physHair.segments;
		line.SetVertexCount(((points.Length - 2) * interpolationFactor) + 2);
		init = true;
	}

	public void OnHover(bool isOver) {
		if (isOver) lineMat.color = highlightColor;
		else lineMat.color = normalColor;
	}
}
