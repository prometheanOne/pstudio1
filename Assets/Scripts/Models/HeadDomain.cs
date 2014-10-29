using UnityEngine;
using System.Collections.Generic;
using ClipperLib;

[System.Serializable]

public class HeadDomain
{
	public PolyTree domain = new PolyTree();
	private List<GameObject> hairs = new List<GameObject>();
	public bool clipped;
	public bool cut;
	private bool hovering;

	public HeadDomain() {
	}

	public HeadDomain(PolyTree dmn) {
		domain = dmn;
	}

	public HeadDomain(PolyNode polygon) {
		Clipper clipper = new Clipper();
		List<IntPoint> sectPoints = new List<IntPoint>();
		sectPoints.Add (new IntPoint(0,0));
		sectPoints.Add (new IntPoint(179,0));
		sectPoints.Add (new IntPoint(179,359));
		sectPoints.Add (new IntPoint(0, 359));
		clipper.AddPath (sectPoints, PolyType.ptSubject, true);
		clipper.AddPath (polygon.Contour, PolyType.ptClip, true);
		clipper.Execute (ClipType.ctIntersection, domain);
		CacheHairs ();
	}

	public int Contains(Vector2 pnt) {
		int pop = 0;
		foreach (PolyNode poly in domain.Childs) {
			int val = Clipper.PointInPolygon (new IntPoint(pnt),poly.Contour);
			if (!poly.IsHole && !poly.IsOpen && val != 0) pop = val;
		}
		return pop;
	}

	public PolyTree Difference(PolyTree clip) {
		Clipper clipper = new Clipper();
		foreach (PolyNode polygon in clip.Childs) clipper.AddPath (polygon.Contour, PolyType.ptClip, true);
		foreach (PolyNode polygon in domain.Childs) clipper.AddPath (polygon.Contour, PolyType.ptSubject, true);
		PolyTree result = new PolyTree();
		clipper.Execute (ClipType.ctDifference, result);
		return result;
	}

	public PolyTree Intersection(PolyTree clip) {
		Clipper clipper = new Clipper();
		foreach (PolyNode polygon in clip.Childs) clipper.AddPath (polygon.Contour, PolyType.ptClip, true);
		foreach (PolyNode polygon in domain.Childs) clipper.AddPath (polygon.Contour, PolyType.ptSubject, true);
		PolyTree result = new PolyTree();
		clipper.Execute (ClipType.ctIntersection, result);
		return result;
	}

	protected void CacheHairs() {
		foreach (GameObject hair in HeadController.Instance.hairs) {
			if (Contains(HeadController.Instance.WorldToHeadCoordinate (hair.transform.position)) != 0) hairs.Add (hair);
		}
		Simulate (false);
	}

	public void Simulate(bool enable) {
		foreach (GameObject hair in hairs) {
			hair.SendMessage ("Simulate", enable);
		}
	}

	public void AddSection(HeadDomain section) {
		HeadController.Instance.haircut.AddSection (section, this);
	}

	public void OnHover(bool isOver) {
		if (isOver && !hovering) {
			foreach(GameObject hair in hairs) hair.SendMessage ("OnHover", true);
			hovering = true;
		}
		else if (!isOver && hovering) {
			foreach(GameObject hair in hairs) hair.SendMessage ("OnHover", false);
			hovering = false;
		}
	}

	public void OnClick() {
		//HeadController.Instance.SectionClicked(this);
		Simulate(true);
	}

	public void OnRelease() {
		Simulate(false);
	}

	/*
	
	float crossProduct(Vector2 a, Vector2 b)
	{
		return a.x * b.y - b.x * a.y;
	}
	
	bool doBoundingBoxesIntersect(Vector2[] a, Vector2[] b)
	{
		return a[0].x <= b[1].x &&
			a[1].x >= b[0].x &&
				a[0].y <= b[1].y &&
				a[1].y >= b[0].y;
	}
	
	bool isPointOnLine(LineSegment a, Vector2 b)
	{
		LineSegment aTmp = new LineSegment(new Vector2(0, 0), new Vector2(a.second.x - a.first.x, a.second.y - a.first.y));
		
		Vector2 bTmp = new Vector2(b.x - a.first.x, b.y - a.first.y);
		
		float r = crossProduct(aTmp.second, bTmp);
		
		return Mathf.Abs(r) < Mathf.Epsilon;
	}
	
	bool isPointRightOfLine(LineSegment a, Vector2 b)
	{
		LineSegment aTmp = new LineSegment(Vector2.zero, new Vector2(a.second.x - a.first.x, a.second.y - a.first.y));
		Vector2 bTmp = new Vector2(b.x - a.first.x, b.y - a.first.y);
		return crossProduct(aTmp.second, bTmp) < 0;
	}
	
	bool lineSegmentTouchesOrCrossesLine(LineSegment a, LineSegment b)
	{
		return isPointOnLine(a, b.first) ||
			isPointOnLine(a, b.second) ||
				(isPointRightOfLine(a, b.first) ^
				 isPointRightOfLine(a, b.second));
	}
	
	bool linesIntersect(LineSegment a, LineSegment b)
	{
		Vector2[] box1 = a.getBoundingBox();
		Vector2[] box2 = b.getBoundingBox();
		return doBoundingBoxesIntersect(box1, box2)
			&& lineSegmentTouchesOrCrossesLine(a, b)
				&& lineSegmentTouchesOrCrossesLine(b, a);
	}
	*/
}
