using UnityEngine;
using System.Collections;

public class HairStrand : MonoBehaviour {

	[SerializeField] private HairSegment[] segments;
	public float gravity;
	public Vector2 stiffness;
	public float segmentLength;

	
	void Start () {
		segments = GetComponentsInChildren<HairSegment>();
		for (int i = 0; i < segments.Length; i++) {
			segments[i].strand = this;
			if (i != segments.Length - 1) segments[i].neighbour = segments[i+1];
		}
	}
	
	// Update is called once per frame
	void Update () {
		segments[0].Move (transform);
	}
}
