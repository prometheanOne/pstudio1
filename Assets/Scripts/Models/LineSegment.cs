using UnityEngine;

public class LineSegment
{
	public Vector2 first { get; set; }
	public Vector2 second { get; set; }
	
	public Vector2[] getBoundingBox()
	{
		Vector2[] points = new Vector2[2];
		
		if (first.x <= second.x)
		{ points[0].x = first.x; points[1].x = second.x; }
		else
		{ points[1].x = first.x; points[0].x = second.x; }
		
		if (first.y <= second.y)
		{ points[0].y = first.y; points[1].y = second.y; }
		else
		{ points[1].y = first.y; points[0].y = second.y; }
		
		return points;
	}
	
	public LineSegment(Vector2 _first, Vector2 _second)
	{
		first = _first;
		second = _second;
	}
}
