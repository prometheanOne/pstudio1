using UnityEngine;
using System.Collections;

public class HairSegment : MonoBehaviour
{
	private float weight = 0f;
	private int mass;
	private Vector2 stiffness;
	[HideInInspector] public HairStrand strand;
	public HairSegment neighbour;
	private Quaternion lastRot;
	private Vector2 a; //Angular acceleration around y- and z-axes
	private Vector2 v; //Current angular velocity around y- and z-axes
	public bool debug;

	void Start() {
		HairSegment[] segmentsBelow = GetComponentsInChildren<HairSegment>();
		mass = Mathf.Clamp (segmentsBelow.Length - 1, 1, 100000);
		lastRot = transform.rotation;
	}


	public void Move(Transform messenger) {
		if (strand != null) {
			if (weight == 0f) weight = mass * strand.gravity;
			//Remove local effect of last segment's rotation
			transform.rotation = lastRot; 

			//Calculate gravity
			Vector3 g = transform.InverseTransformDirection (Vector3.down);
			Vector3 f = g * weight;
			if (debug) {
				Debug.Log ("gx: "+g.x.ToString ("F5")+", gy: "+g.y.ToString ("F5")+", gz: "+g.z.ToString ("F5"));
			}

			//Calculate drag force
			if (stiffness == Vector2.zero) stiffness = strand.stiffness;
			f = new Vector3(f.x, f.y - (stiffness.x * v.x), f.z - (stiffness.y * v.y));

			//float dragMag = stiffness * Mathf.Abs (Quaternion.Angle (messenger.rotation,transform.rotation));

			//Debug.Log (drag);
			//Calculate acceleration, velocity and rotation
			float ay = f.z / mass;
			//float az = 0f;
			float az = -f.y / mass;
			a = new Vector2 (ay,az);
			float vy = v.x + (a.x * Time.deltaTime);
			float vz = v.y + (a.y * Time.deltaTime);
			v = new Vector2 (vy,vz);
			float ry = v.x * Time.deltaTime;
			float rz = v.y * Time.deltaTime;

			//Apply rotations
			transform.localEulerAngles += new Vector3(0f, ry, rz);
			lastRot = transform.rotation;
			if (neighbour != null) neighbour.Move(transform);
		}
	}

	//void OnCollisionEnter(Collision collision) {
		//if (collision.gameObject.tag == "head") {

}
