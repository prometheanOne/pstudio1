using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClipperLib;

public class Haircut : MonoBehaviour
{
	public HeadDomain domain;
	public List<HeadDomain> sections = new List<HeadDomain>();
	public Texture2D headMap;
	public Texture2D userMap;
	public Color[,] hairVals = new Color[180, 360];
	[SerializeField] private HeadMapWidgetView headmapWidget;
	private bool sectionPressActive;
	private HeadDomain pressingSection;

	void Start() {
		Color[] hairCols = headMap.GetPixels ();
		for (int i = 0; i < 360; i++) {
			for (int j = 0; j < 180; j++) {
				hairVals[j,i] = hairCols[(i*180) + j];
			}
		}

		//headmapWidget.UpdateWidget ();

		PolyTree dmn = new PolyTree();
		IntPoint[] edgePoints = new IntPoint[362];
		for (int i = 0; i < 360; i++) {
			int j = 179;
			while (hairVals[j,i].a < 0.5f) j--;
			edgePoints[i].X = j;
			edgePoints[i].Y = i;
		}
		edgePoints[360].X = 0;
		edgePoints[360].Y = 359;
		edgePoints[361].X = 0;
		edgePoints[361].Y = 0;

		IntPoint[] boundary = new IntPoint[4];
		boundary[0].X = 0;
		boundary[0].Y = 0;
		boundary[1].X = 179;
		boundary[1].Y = 0;
		boundary[2].X = 179;
		boundary[2].Y = 359;
		boundary[3].X = 0;
		boundary[3].Y = 359;

		Clipper clipper = new Clipper(0);
		clipper.AddPath (new List<IntPoint>(edgePoints), PolyType.ptClip, true);
		clipper.AddPath (new List<IntPoint>(boundary), PolyType.ptSubject, true);
		clipper.Execute (ClipType.ctIntersection, dmn);
		domain = new HeadDomain(dmn);
	}

	public void AddSection(HeadDomain section) {
		sections.Add (section);
		headmapWidget.HighlightDomain (section, 3f);
	}

	public void AddSection(HeadDomain section, HeadDomain parent) {
	}

	public void OnClick(Vector2 headCoord) {
		if (!sectionPressActive) {
			sectionPressActive = true;
			foreach (HeadDomain section in sections) {
				if (section.Contains (headCoord) != 0) {
					section.OnClick();
					pressingSection = section;
				}
			}
		}
	}

	public void OnHoverEnter(Vector2 headCoord) {
		foreach(HeadDomain section in sections) {
			if (section.Contains (headCoord) != 0) section.OnHover(true);
			else section.OnHover(false);
		}
	}

	public void OnHoverOut() {
		foreach (HeadDomain section in sections) {
			section.OnHover(false);
		}
	}

	void Update() {
		if (sectionPressActive && !InputController.Instance.buttonPressed) {
			sectionPressActive = false;
			pressingSection.OnRelease();
		}
	}
}
