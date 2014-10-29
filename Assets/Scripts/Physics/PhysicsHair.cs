using UnityEngine;
using System.Collections;

public class PhysicsHair : MonoBehaviour
{
	public Vector3[] segments;
	[SerializeField] private float segmentMassMin = 1.0f;
	[SerializeField] private float segmentCumulativeMass = 0.3f;
	private Vector3[] vs;
	private PhysicsEffector[] effectors;
	private PhysicsDisplacer[] displacers;
	public bool simulate;
	private float gravity;
	[SerializeField] private float maxDelta;
	public float segmentLength;
	public float hairLength;
	[SerializeField] private int frameAverage;
	private int frameCounter;
	private Vector3[] averages;

	void Start() {
		/*Transform[] tempSegments = GetComponentsInChildren<Transform>();
		segments = new Transform[tempSegments.Length - 1];
		for (int i = 0; i < segments.Length; i++) {
			segments[i] = tempSegments[i+1];
		}
		segmentLength = (segments[1].position - segments[0].position).magnitude;*/
		effectors = PhysicsController.Instance.effectors;
		displacers = PhysicsController.Instance.displacers;
		gravity = PhysicsController.Instance.gravity;
		segments = new Vector3[Mathf.RoundToInt (hairLength / segmentLength)];
		averages = new Vector3[segments.Length];
		for(int i = 0; i < segments.Length; i++) {
			segments[i] = i * segmentLength * -transform.up;
		}
		vs = new Vector3[segments.Length];
	}

	void Update() {
		if (simulate) {
			for (int i = 1; i < segments.Length; i++) {
				Vector3 segmentPos = transform.position + segments[i];
				Vector3 prevSegmentPos = transform.position + segments[i-1];
				float mass = ((segments.Length - i) * (segments.Length - i) * segmentCumulativeMass) + segmentMassMin;
				Vector3 deltaV = new Vector3(0f, -mass * gravity * Time.deltaTime, 0f);
				foreach (PhysicsEffector effector in effectors) {
					deltaV += effector.DeltaV (segmentPos, vs[i]);
				}
				Vector3 targetV = vs[i] + deltaV;
				Vector3 targetPos = segmentPos + (Time.deltaTime * targetV);
				Vector3 dispDir = (targetPos - prevSegmentPos).normalized;
				Vector3 newPos = prevSegmentPos + (segmentLength * dispDir);
				newPos = Vector3.MoveTowards (segmentPos, newPos, maxDelta);
				foreach (PhysicsDisplacer disp in displacers) {
					newPos += disp.DeltaD (newPos);
				}
				//vs[i] = (newPos - segments[i].position) / Time.deltaTime;
				averages[i] += newPos;
				if (frameCounter == frameAverage) {
					Vector3 oldPos = segmentPos;
					Vector3 avgPos = averages[i] / (frameAverage + 1);
					segments[i] = avgPos - transform.position;
					vs[i] = (segmentPos - oldPos) / Time.deltaTime;
					averages[i] = Vector3.zero;
				}
				//Debug.DrawLine(segments[i].position, targetPos);
				//Vector3 rotation = Quaternion.FromToRotation (segments[i].right, dispDir).eulerAngles;
				//segments[i].localEulerAngles = new Vector3(segments[i].localEulerAngles.x, rotation.y, segments[i].localEulerAngles.z);
			}
			if (frameCounter < frameAverage) frameCounter++;
			else frameCounter = 0;
		}
	}

	void Simulate(bool enable) {
		simulate = enable;
	}

}
