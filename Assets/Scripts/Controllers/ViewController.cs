using UnityEngine;
using System.Collections;

public class ViewController : SingletonComponent<ViewController>
{
	[SerializeField] private GameObject[] loadingScreens;

	void Start() {
		loadingScreens[0].SetActive (true);
	}

	public void Subsection() {
	}
}