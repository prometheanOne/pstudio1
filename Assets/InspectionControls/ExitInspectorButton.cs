using UnityEngine;
using System.Collections;

/// <summary>
/// Exits the inspector view.
/// </summary>

public class ExitInspectorButton : MonoBehaviour
{
	void OnClick()
	{
	    InspectionOrbiter.Instance.InspectorObject = null;
	}
}
