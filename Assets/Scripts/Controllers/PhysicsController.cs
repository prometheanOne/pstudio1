using UnityEngine;
using System.Collections;

public class PhysicsController : SingletonComponent<PhysicsController>
{
	public PhysicsEffector[] effectors;
	public PhysicsDisplacer[] displacers;
	public float gravity;

	void Awake() {
		effectors = FindObjectsOfType<PhysicsEffector>();
		displacers = FindObjectsOfType<PhysicsDisplacer>();
	}
}
