using UnityEngine;
using System.Collections;

public class HairCluster : MonoBehaviour
{
	[SerializeField] private int numHairs;
	[SerializeField] private GameObject hairPrefab;
	[SerializeField] private float patchWidth;

	void Start() {
		for (int i = 0; i < numHairs; i++) {
			Vector3 pos = new Vector3 (Random.Range (transform.position.x - (patchWidth / 2), transform.position.x + (patchWidth / 2)), transform.position.y, Random.Range (transform.position.z - (patchWidth / 2), transform.position.z + (patchWidth / 2)));
			GameObject.Instantiate (hairPrefab,pos, transform.rotation);
		}
	}
}
