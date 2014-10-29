using UnityEngine;
using System.Collections;

public class SubsectionHairGroup : MonoBehaviour
{
	[SerializeField] private GameObject[] hairStrands;
	private HeadDomain domain;
	
	public void Init(HeadDomain dom) {
		domain = dom;
	}

}
