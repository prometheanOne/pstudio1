using UnityEngine;
using System.Collections;

public class MousePointer : MonoBehaviour {

	[SerializeField] private Camera GUICam;

	void Update () 
	{
		Vector3 newPos = GUICam.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
	}

}
