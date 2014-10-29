using UnityEngine;
using System.Collections;
using ClipperLib;

public class HeadMapWidgetView : MonoBehaviour
{
	private Material widgetMat;
	private Haircut haircut;
	private Texture2D userMap;
	[SerializeField] private Color highlightColor = new Color(0.5f, 0f, 0.5f, 0f);

	void Start() {
		widgetMat = GetComponent<MeshRenderer>().material;
		haircut = HeadController.Instance.haircut;
		userMap = new Texture2D(180,360);
		StartCoroutine (InitWidget());
	}

	public void UpdateWidget() {
		Color[] hairCols = Flatten(HeadController.Instance.haircut.hairVals);
		userMap.SetPixels (hairCols);
		userMap.Apply ();
		widgetMat.mainTexture = userMap;
	}

	public void HighlightDomain(HeadDomain domain, float duration) {
		for (int i = 0; i < 360; i++) {
			for (int j = 0; j < 180; j++) {
				if (domain.Contains (new Vector2(j, i)) != 0) {
					Color currentCol = userMap.GetPixel (j,i);
					userMap.SetPixel (j,i,currentCol - highlightColor);
				}
			}
		}
		userMap.Apply ();
		StartCoroutine(ClearHighlight(domain, duration));
	}

	Color[] Flatten(Color[,] colors) {
		Color[] flatCols = new Color[64800];
		for (int i = 0; i < 360; i++) {
			for (int j = 0; j < 180; j++) {
				flatCols[(i * 180) + j] = colors[j,i];
			}
		}
		return flatCols;
	}

	IEnumerator ClearHighlight(HeadDomain domain, float duration) {
		yield return new WaitForSeconds(duration);
		Debug.Log ("Clearing highlight");
		for (int i = 0; i < 360; i++) {
			for (int j = 0; j < 180; j++) {
				if (domain.Contains (new Vector2(j, i)) != 0) {
					Color currentCol = userMap.GetPixel (j,i);
					userMap.SetPixel (j,i,currentCol + highlightColor);
				}
			}
		}
		userMap.Apply ();
	}

	IEnumerator InitWidget() {
		yield return new WaitForSeconds(0.1f);
		UpdateWidget();
	}
}